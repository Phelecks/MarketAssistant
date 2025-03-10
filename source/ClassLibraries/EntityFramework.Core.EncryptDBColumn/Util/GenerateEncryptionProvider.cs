using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using EntityFrameworkCore.EncryptColumn.Interfaces;

namespace EntityFrameworkCore.EncryptColumn.Util
{
    public class GenerateEncryptionProvider(string key) : IEncryptionProvider
    {
        private readonly string key = key;

        public string Encrypt(string dataToEncrypt)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "Please initialize your encryption key.");

            if (string.IsNullOrEmpty(dataToEncrypt))
                return string.Empty;
                
            byte[] array;
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(dataToEncrypt);
                }
                array = memoryStream.ToArray();
            }
            string result = Convert.ToBase64String(array);
            return result;
        }

        public string Decrypt(string dataToDecrypt)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "Please initialize your encryption key.");

            if (string.IsNullOrEmpty(dataToDecrypt))
                return string.Empty;
                
            byte[] iv = new byte[16];

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            var buffer = Convert.FromBase64String(dataToDecrypt);
            using MemoryStream memoryStream = new(buffer);

            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);

            using StreamReader streamReader = new(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
