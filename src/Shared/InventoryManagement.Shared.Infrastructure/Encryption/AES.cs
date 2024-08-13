using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InventoryManagement.Shared.Infrastructure.Encryption
{
    public class AES : IAES
    {
        private static readonly byte[] _key = Encoding.ASCII.GetBytes("b14ca5898a4e4133bbce2ea2315a1916");
        private static readonly byte[] _iv = Encoding.ASCII.GetBytes("RadyaLab$Mant4pU"); //should be 16 digit

        public string EncryptStringToBase64String(string value)
        {

            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Key = _key;
            aes.IV = _iv;

            ICryptoTransform encryptor = aes.CreateEncryptor();

            byte[] encrypted;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(value);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return Convert.ToHexString(encrypted);
        }

        public string Decrypt(string value)
        {
            string plaintext = null;

            //for add Hex string to char '-' to 2, 4, 6 and so on
            value = Regex.Replace(value, ".{2}", "$0-");
            value = value.Remove(value.Length - 1);
            int length = (value.Length + 1) / 3;

            //for string to byte array
            byte[] encrypted = new byte[length];
            for (int i = 0; i < length; i++)
                encrypted[i] = Convert.ToByte(value.Substring(3 * i, 2), 16);
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                HashAlgorithm hash = MD5.Create();
                aes.Key =_key;
                aes.IV = _iv;

                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(encrypted))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

    }
}
