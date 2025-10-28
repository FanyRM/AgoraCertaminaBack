using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.Services.Interfaces;
using AgoraCertaminaBack.UseCases.SchemaContests;
using AgoraCertaminaBack.UseCases.Shared;
using AgoraCertaminaBack.UseCases.Tenants;
using ROP;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class CreateContest(IMongoRepository<Contest> _mongoRepository, UserRequestContext _userContext, IFileManager _fileManager, GetByIdTenant _getByIdTenant, GetByIdSchema _getByIdSchema)
    {

        internal static readonly HashSet<int> fileTypes = new int[] { (int)FieldTypeEnum.Image, (int)FieldTypeEnum.Archive }.ToHashSet();

        public async Task<Result<string>> Execute(CreateContestRequest request)
        {
            var result = await _getByIdTenant.Execute(_userContext.OrganizationId)
                .Bind(customer => _getByIdSchema.Execute(request.SchemaId)
                    .Map(schema => (customer, schema)))
                .Bind(tuple => AddContestWithoutFiles(tuple.customer, request))
                .Bind(contest => UploadContestImage(contest, request))
                .Bind(idContest => UploadFilesFromFields(idContest, request));

            return result;
        }

        public async Task<Result<Contest>> AddContestWithoutFiles(Tenant tenant, CreateContestRequest request)
        {
            var contest = request.ConvertToContest();

            contest.ReferenceNumber = StringUtilities.CreateReferenceNumber("ACC");
            contest.OrganizationId = tenant.Id;
            contest.OrganizationName = tenant.TenantName;
            contest.ImageUrl = string.Empty;

            //Ignorar los campos que no son de tipo imagen o archivo, se registrarán al final, despues del guardado
            var fieldsWithoutFiles = contest.Fields
                .Where(file => !fileTypes.Contains((int)file.Type))
                .ToList();

            contest.Fields = fieldsWithoutFiles;

            await _mongoRepository.InsertOneAsync(contest);

            //Obtener el id del contest creado
            return contest.Success();
        }

        public async Task<Result<Contest>> UploadContestImage(Contest contestRegistered, CreateContestRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ImageUrl))
                return contestRegistered.Success();

            try
            {
                var imageFile = JsonSerializer.Deserialize<FieldFileRequest>(request.ImageUrl);

                if (imageFile == null)
                    return Result.Failure<Contest>("Error al deserializar la imagen del contest");

                string safeName = SanitizeFileName(imageFile.Name);

                string name = $"{request.SchemaName}_{contestRegistered.ReferenceNumber}_{safeName}";
                string pathBase = $"{contestRegistered.OrganizationName}";
                string imagePath = $"{pathBase}/contest-image_{name}";

                bool saved = await _fileManager.UploadFileAsync(imagePath, imageFile.ContentStream);

                if (!saved)
                    return Result.Failure<Contest>("Error al guardar la imagen del contest");

                contestRegistered.ImageUrl = imagePath;
                return contestRegistered.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure<Contest>($"Error al procesar la imagen del contest: {ex.Message}");
            }
        }

        public async Task<Result<string>> UploadFilesFromFields(Contest contestRegistered, CreateContestRequest request)
        {
            string pathBase = $"{contestRegistered.OrganizationName}";

            Contest contestWithFiles = contestRegistered;

            List<FieldDTO> filesFromFields = request.Fields
                .Where(file => fileTypes.Contains((int)file.Type))
                .ToList();

            foreach (var field in filesFromFields)
            {
                var files = JsonSerializer.Deserialize<IEnumerable<FieldFileRequest>>(field.Value);

                if (files == null)
                {
                    return Result.Failure<string>("Error at deserialize field values");
                }

                try
                {
                    List<string> allPaths = [];

                    foreach (var file in files)
                    {
                        string safeName = SanitizeFileName(file.Name);
                        string name = $"{contestRegistered.ReferenceNumber}_{safeName}";
                        string savePath = pathBase + "/" + name;
                        bool saved = await _fileManager.UploadFileAsync(savePath, file.ContentStream);

                        if (saved) allPaths.Add(savePath);
                    }

                    contestWithFiles.Fields.Add(new FieldValue
                    {
                        Id = field.Id,
                        FieldName = field.Name,
                        Value = string.Join(",", allPaths),
                        Type = field.Type,
                        IsBase = field.IsBase
                    });
                }
                catch (Exception)
                {
                    continue;
                }
            }

            await _mongoRepository.ReplaceOneAsync(contestWithFiles);

            return contestWithFiles.Id.Success(HttpStatusCode.Created);
        }
        private static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            // Normaliza (quita acentos)
            string normalized = fileName.Normalize(NormalizationForm.FormD);
            var chars = normalized.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark);
            string noAccents = new string(chars.ToArray());

            // Reemplaza espacios por "_"
            noAccents = noAccents.Replace(" ", "_");

            // Elimina caracteres especiales no válidos para archivos
            foreach (char c in Path.GetInvalidFileNameChars())
                noAccents = noAccents.Replace(c.ToString(), "");

            // Retorna limpio y normalizado
            return noAccents;
        }

    }
}
