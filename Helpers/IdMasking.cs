using DropSpace.Helpers.HelperHidden;
using System.Text;

namespace DropSpace.Helpers
{
    public static class IdMasking
    {
        #region Encryption With Crypto



        #region 32 Byte Encryption
        public static string Encode(string plainText)
        {

            byte[] Key = Encoding.UTF8.GetBytes("1674878443police"); // 16 bytes key
            byte[] IV = Encoding.UTF8.GetBytes("opusbd1777177780"); // 16 bytes IV

            byte[] encryptedData = BuildMasker.EncryptStringToBytes_Aes(plainText, Key, IV);

            string base64EncryptedData = Convert.ToBase64String(encryptedData);

            // Make the base64 string URL-safe
            string urlSafeEncryptedData = base64EncryptedData.Replace("/", "_").Replace("+", "-").Replace("=", "");

            return urlSafeEncryptedData;

        }
        public static string Decode(string data)
        {
            byte[] Key = Encoding.UTF8.GetBytes("1674878443police");
            byte[] IV = Encoding.UTF8.GetBytes("opusbd1777177780");

            string base64DecryptedData = data.Replace("_", "/").Replace("-", "+");
            // Add padding if necessary
            int padding = 4 - (base64DecryptedData.Length % 4);
            if (padding != 4)
            {
                base64DecryptedData = base64DecryptedData.PadRight(base64DecryptedData.Length + padding, '=');
            }
            byte[] encryptedDataBytes = Convert.FromBase64String(base64DecryptedData);

            string decryptedText = BuildMasker.DecryptStringFromBytes_Aes(encryptedDataBytes, Key, IV);

            return decryptedText;
        }
        #endregion

        #endregion

    }

}
