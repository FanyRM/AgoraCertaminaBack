namespace AgoraCertaminaBack.UseCases.Shared
{
    internal static class FileUtilities
    {

        /// <summary>
        /// Transform an Base64DataUrl to Base64 string
        /// </summary>
        /// <param name="dataUrl">File in B64 DataURL</param>
        /// <returns>B64 Pure</returns>
        internal static string Base64DataUrlToBase64(string dataUrl)
        {
            if (string.IsNullOrWhiteSpace(dataUrl))
                return string.Empty;

            int commaIndex = dataUrl.IndexOf(',');
            if (commaIndex < 0)
                throw new FormatException("Coma (,) separator is missing on B64DataUrl");

            string base64 = dataUrl[(commaIndex + 1)..].Trim();

            // Limpieza adicional
            base64 = base64.Replace("\r", "").Replace("\n", "").Replace(" ", "");

            // Padding, si falta
            if (base64.Length % 4 != 0)
                base64 = base64.PadRight(base64.Length + (4 - base64.Length % 4), '=');

            return base64;
        }

        /// <summary>
        /// Transforms an bytes[] to a Stream
        /// </summary>
        /// <param name="bytes">Bytes to transform</param>
        /// <returns>Bytes transformed to Stream</returns>
        internal static Stream BytesToStream(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("Bytes array is empty", nameof(bytes));
            }

            var stream = new MemoryStream(bytes);
            stream.Position = 0; 

            return stream;
        }
    }
}
