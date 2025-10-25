using System.Net;
using System.Text;
using System.Text.Json;
using AgoraCertaminaBack.Services.Interfaces;
using AgoraCertaminaBack.Services.Settings;

namespace AgoraCertaminaBack.Services
{
    public class LocalAWSService : IAWS
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceSettings _settings;

        public LocalAWSService(HttpClient httpClient, IServiceSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<bool> UploadFileToS3(string key, Stream documento)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(key);

                string base64Content;
                using (var memoryStream = new MemoryStream())
                {
                    await documento.CopyToAsync(memoryStream);
                    base64Content = Convert.ToBase64String(memoryStream.ToArray());
                }

                var uploadRequest = new
                {
                    bucket,
                    fileName,
                    base64Content
                };

                var json = JsonSerializer.Serialize(uploadRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_settings.LocalSimulatorUrl}/upload", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Upload to local S3 failed: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteFileFromS3(string key)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(key);

                var response = await _httpClient.DeleteAsync($"{_settings.LocalSimulatorUrl}/file/{bucket}/{fileName}");

                return response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound;
            }
            catch (Exception ex)
            {
                throw new Exception($"Delete from local S3 failed: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> GetFileFromS3(string key)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(key);

                var response = await _httpClient.GetAsync($"{_settings.LocalSimulatorUrl}/{bucket}/{fileName}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    throw new Exception($"File not found: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Get from local S3 failed: {ex.Message}", ex);
            }
        }

        private (string bucket, string fileName) ParseS3Path(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty");

            var parts = path.Split('/', 2);
            if (parts.Length < 2)
                throw new ArgumentException($"Invalid S3 path format: {path}");

            return (parts[0], parts[1]);
        }
    }
}
