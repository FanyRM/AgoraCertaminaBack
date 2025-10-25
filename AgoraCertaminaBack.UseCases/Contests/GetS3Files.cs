using AgoraCertaminaBack.Services.Interfaces;
using AgoraCertaminaBack.UseCases.Shared;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class GetS3Files(IAWS _awsService)
    {
        private static readonly HashSet<string> InvalidPaths = new(StringComparer.OrdinalIgnoreCase)
        {
            "", "null", "BsonNull", "undefined", "bjsonnull"
        };

        public async Task<Result<FieldFileResponse>> Execute(FileRequest fileRequest)
        {

            if (fileRequest == null || string.IsNullOrWhiteSpace(fileRequest.KeyPath) || InvalidPaths.Contains(fileRequest.KeyPath.Trim()))
            {
                return Result.Failure<FieldFileResponse>("La ruta del archivo es inválida o está vacía.");
            }

            try
            {
                var file = await _awsService.GetFileFromS3(fileRequest.KeyPath.Trim());

                if (file == null || file.Length == 0)
                {
                    return Result.Failure<FieldFileResponse>("El archivo no fue encontrado.");
                }

                return new FieldFileResponse
                {
                    Content = Convert.ToBase64String(file),
                    Name = Path.GetFileName(fileRequest.KeyPath),
                    Type = Path.GetExtension(fileRequest.KeyPath)
                }.Success(HttpStatusCode.OK);
            }
            catch
            {
                return Result.Failure<FieldFileResponse>($"RequestS3Error");
            }
        }
    }
}