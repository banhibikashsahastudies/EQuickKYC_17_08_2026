using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace EQuickKYC.Infrastructure.Security
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;

        public EncryptionService(IConfiguration configuration)
        {
            var key = configuration["Encryption:Key"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("Encryption key is not configured.");
            }

            _key = Convert.FromBase64String(key);

            if (_key.Length != 32)
            {
                throw new InvalidOperationException("Encryption key must be 32 bytes.");
            }
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }

            using var aes = Aes.Create();

            aes.Key = _key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();

            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Store IV + encrypted data together.
            var result = new byte[aes.IV.Length + encryptedBytes.Length];

            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);

            Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return cipherText;
            }

            var encryptedData = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();

            aes.Key = _key;

            var ivLength = aes.BlockSize / 8;

            var iv = new byte[ivLength];
            var encryptedBytes = new byte[encryptedData.Length - ivLength];

            Buffer.BlockCopy(encryptedData, 0, iv, 0, ivLength);

            Buffer.BlockCopy(encryptedData, ivLength, encryptedBytes, 0, encryptedBytes.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();

            var plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}