using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.IO;

namespace NitroSniper
{
    class Security
    {
        public static byte[] GetFileHash(string fileName)
        {
            var md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(File.ReadAllBytes(fileName));
            md5.Dispose();
            return hash;
        }
        public static byte[] Encrypt(string token, string AESKey, byte[] saltBytes)
        {
            byte[] Data = Encoding.ASCII.GetBytes(token);
            RijndaelManaged aesEncryption = new RijndaelManaged()
            {
                KeySize = 256,
                BlockSize = 128
            };
            var key = new Rfc2898DeriveBytes(AESKey, saltBytes, 1000);
            aesEncryption.IV = key.GetBytes(aesEncryption.BlockSize / 8);
            aesEncryption.Key = key.GetBytes(aesEncryption.KeySize / 8);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            return crypto.TransformFinalBlock(Data, 0, Data.Length);
        }
        public static string Decrypt(byte[] token, string AESKey, byte[] saltBytes)
        {
            try
            {
                RijndaelManaged aesEncryption = new RijndaelManaged()
                {
                    KeySize = 256,
                    BlockSize = 128
                };
                var key = new Rfc2898DeriveBytes(AESKey, saltBytes, 1000);
                aesEncryption.IV = key.GetBytes(aesEncryption.BlockSize / 8);
                aesEncryption.Key = key.GetBytes(aesEncryption.KeySize / 8);
                ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
                return Encoding.ASCII.GetString(decrypto.TransformFinalBlock(token, 0, token.Length));
            }
            catch
            {
                return "invalid";
            }
        }

    }
}
