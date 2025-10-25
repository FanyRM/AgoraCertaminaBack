using System.Text.Json.Serialization;

namespace AgoraCertaminaBack.UseCases.Shared
{
    public class FieldFileRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;

        public Stream ContentStream
        {
            get
            {
                var b64 = FileUtilities.Base64DataUrlToBase64(Content);
                var bytes = Convert.FromBase64String(b64);
                return FileUtilities.BytesToStream(bytes);
            }
        }
    }

    public class FieldFileResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;

    }

    public class FileRequest
    {
        public required string KeyPath { get; set; } = null!;
    };

}
