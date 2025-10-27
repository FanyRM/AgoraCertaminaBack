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
            // Si no hay ImageUrl en el request, mantener vacío y continuar
            if (string.IsNullOrWhiteSpace(request.ImageUrl))
            {
                return contestRegistered.Success();
            }

            try
            {
                // Deserializar la imagen del contest
                var imageFile = JsonSerializer.Deserialize<FieldFileRequest>(request.ImageUrl);

                if (imageFile == null)
                {
                    return Result.Failure<Contest>("Error al deserializar la imagen del contest");
                }

                // Construir la ruta base para la imagen
                var encodedFileName = Uri.EscapeDataString(imageFile.Name);
                string pathBase = $"{contestRegistered.OrganizationName}/{request.SchemaName}/{contestRegistered.ReferenceNumber}";
                string imagePath = $"{pathBase}/contest-image/{encodedFileName}";

                // Subir el archivo
                bool saved = await _fileManager.UploadFileAsync(imagePath, imageFile.ContentStream);

                if (!saved)
                {
                    return Result.Failure<Contest>("Error al guardar la imagen del contest");
                }

                // Actualizar el contest con la ruta de la imagen
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
            string pathBase = $"{contestRegistered.OrganizationName}/{request.SchemaName}/{contestRegistered.ReferenceNumber}";

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
                        var encodedFileName = Uri.EscapeDataString(file.Name);
                        string savePath = pathBase + "/" + encodedFileName;
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
    }
}
