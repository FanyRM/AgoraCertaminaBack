namespace AgoraCertaminaBack.UseCases.Shared
{
    internal static class StringUtilities
    {
        /// <summary>
        /// Create a reference number based on a prefix and a GUID using only 16 firstly characters
        /// </summary>
        /// <param name="prefix">Prefix of reference number</param>
        /// <returns>String reference number</returns>
        internal static string CreateReferenceNumber(string prefix)
        {
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            string referenceNumber = $"{prefix}-{guid[..4]}-{guid.Substring(4, 4)}-{guid.Substring(8, 4)}-{guid.Substring(12, 4)}";

            return referenceNumber;
        }
    }
}
