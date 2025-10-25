using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AgoraCertaminaBack.Services.Interfaces;
using AgoraCertaminaBack.Services.Settings;

namespace AgoraCertaminaBack.Services
{
    public class LocalFileManagerConfigurable : IFileManager
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceSettings _settings;

        public LocalFileManagerConfigurable(HttpClient httpClient, IServiceSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<bool> UploadFileAsync(string path, Stream document)
        {
            return await UploadToLocalS3(path, document, _settings.LocalSimulatorUrl);
        }

        private async Task<bool> UploadToLocalS3(string path, Stream document, string baseUrl)
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

                var response = await _httpClient.PostAsync($"{baseUrl}/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Local S3 upload failed: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload to local S3: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteFileAsync(string path)
        {
            return await DeleteFromLocalS3(path, _settings.LocalSimulatorUrl);
        }

        private async Task<bool> DeleteFromLocalS3(string path, string baseUrl)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(path);

                var response = await _httpClient.DeleteAsync($"{baseUrl}/file/{bucket}/{fileName}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Archivo no existe - consideramos éxito para idempotencia
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Local S3 delete failed: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete from local S3: {ex.Message}", ex);
            }
        }

        public async Task<string> GetJsonFromFileAsync(string path)
        {
            return await GetJsonFromLocalS3(path, _settings.LocalSimulatorUrl);
        }

        private async Task<string> GetJsonFromLocalS3(string path, string baseUrl)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(path);

                var response = await _httpClient.GetAsync($"{baseUrl}/file/{bucket}/{fileName}");

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
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new FileNotFoundException($"JSON file not found: {path}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Local S3 get failed: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get JSON from local S3: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> GetPdfAsync(string path)
        {
            return await GetPdfFromLocalS3(path, _settings.LocalSimulatorUrl);
        }

        private async Task<byte[]> GetPdfFromLocalS3(string path, string baseUrl)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(path);

                // Usar el endpoint directo que devuelve el archivo binario
                var response = await _httpClient.GetAsync($"{baseUrl}/{bucket}/{fileName}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new FileNotFoundException($"PDF file not found: {path}");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Local S3 get PDF failed: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get PDF from local S3: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Método adicional para obtener cualquier archivo como byte[]
        /// </summary>
        public async Task<byte[]> GetFileAsync(string path)
        {
            return await GetPdfFromLocalS3(path, _settings.LocalSimulatorUrl);
        }

        /// <summary>
        /// Método adicional para verificar si un archivo existe
        /// </summary>
        public async Task<bool> FileExistsAsync(string path)
        {
            try
            {
                var (bucket, fileName) = ParseS3Path(path);
                var response = await _httpClient.GetAsync($"{_settings.LocalSimulatorUrl}/file/{bucket}/{fileName}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private (string bucket, string fileName) ParseS3Path(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty");

            var parts = path.Split('/', 2);
            if (parts.Length < 2)
                throw new ArgumentException($"Invalid S3 path format. Expected 'bucketName/filePath', got: {path}");

            return (parts[0], parts[1]);
        }

        #region IDisposable
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
