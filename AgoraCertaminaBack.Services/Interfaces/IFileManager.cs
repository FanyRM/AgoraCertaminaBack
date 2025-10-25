namespace AgoraCertaminaBack.Services.Interfaces
{
    public interface IFileManager
    {
        /// <summary>
        /// Function to upload files
        /// </summary>
        /// <param name="path">String that represents the file path where it will be saved</param>
        /// <param name="document">File as a Stream that will be uploaded</param>
        /// <returns>Returns a boolean indicating whether the file was successfully uploaded</returns>
        Task<bool> UploadFileAsync(string path, Stream document);

        /// <summary>
        /// Function to delete a file
        /// </summary>
        /// <param name="path">String that represents the file path where it was saved</param>
        /// <returns>Returns a boolean indicating whether the file was successfully deleted</returns>
        Task<bool> DeleteFileAsync(string path);

        /// <summary>
        /// Function to retrieve a JSON file
        /// </summary>
        /// <param name="path">String that represents the file path where it was saved</param>
        /// <returns>Returns the JSON content of the requested file</returns>
        Task<string> GetJsonFromFileAsync(string path);

        /// <summary>
        /// Function to retrieve a PDF file
        /// </summary>
        /// <param name="path">String that represents the file path where it was saved</param>
        /// <returns>Returns the file as a byte array</returns>
        Task<byte[]> GetPdfAsync(string path);
    }
}
