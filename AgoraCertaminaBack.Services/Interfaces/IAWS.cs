namespace AgoraCertaminaBack.Services.Interfaces
{
    public interface IAWS
    {
        /// <summary>
        /// Funcion para guardar documentos en AWS S3
        /// </summary>
        /// <param name="key">Cadena que representa la direccion del archivo en la que se guardará</param>
        /// <param name="documento">Archivo en tipo Stream que será guardado</param>
        /// <returns>Retorna un booleano el cual representa si el archivo se guardo correctamente</returns>
        Task<bool> UploadFileToS3(string key, Stream documento);

        /// <summary>
        /// Funcion para eliminar un archivo en el S3
        /// </summary>
        /// <param name="key">Cadena que representa la direccion del archivo en la que se guardo</param>
        /// <returns>Retorna un booleano el cual representa si el archivo se elimino correctamente</returns>
        Task<bool> DeleteFileFromS3(string key);

        /// <summary>
        /// Funcion para obtener un archivo en formato pdf desde s3
        /// </summary>
        /// <param name="key">Cadena que representa la direccion del archivo en la que se guardo</param>
        /// <returns> Retornar el archivo en un arreglo de bits </returns>
        Task<byte[]> GetFileFromS3(string key);
    }
}
