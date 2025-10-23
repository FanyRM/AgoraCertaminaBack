using System.Net;
using System.Text;
using AgoraCertaminaBack.Services.Interfaces;
using System.Text.Json;
using AgoraCertaminaBack.Services.Settings;

namespace AgoraCertaminaBack.Services
{
    public class LocalFileManager : IFileManager
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceSettings _settings;

        public LocalFileManager(HttpClient httpClient, IServiceSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<bool> UploadFileAsync(string path, Stream document)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(path);

                string base64Content;
                using (var memoryStream = new MemoryStream())
                {
                    await document.CopyToAsync(memoryStream);
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

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Upload failed: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong at upload file: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteFileAsync(string path)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(path);

                var response = await _httpClient.DeleteAsync($"{_settings.LocalSimulatorUrl}/file/{bucket}/{fileName}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return true; // Idempotencia - archivo ya no existe
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Delete failed: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong at delete file: {ex.Message}", ex);
            }
        }

        public async Task<string> GetJsonFromFileAsync(string path)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(path);

                var response = await _httpClient.GetAsync($"{_settings.LocalSimulatorUrl}/file/{bucket}/{fileName}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    using var document = JsonDocument.Parse(jsonResponse);

                    var base64Content = document.RootElement
                        .GetProperty("data")
                        .GetProperty("base64Content")
                        .GetString();

                    if (string.IsNullOrEmpty(base64Content))
                        throw new Exception("No content found in file");

                    var jsonBytes = Convert.FromBase64String(base64Content);
                    return Encoding.UTF8.GetString(jsonBytes);
                }
                else
                {
                    throw new Exception($"File not found: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong at get json file: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> GetPdfAsync(string path)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(path);

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
                throw new Exception($"Something went wrong at get pdf file: {ex.Message}", ex);
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
