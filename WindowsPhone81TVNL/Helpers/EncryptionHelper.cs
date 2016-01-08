using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace WindowsPhone81TVNL.Helpers
{
    public class EncryptionHelper
    {
        private static byte[] a = Encoding.UTF8.GetBytes("884c3c06f46f3fa0ec8c81a21687d5c3");
        //5aca9a08e3dd680c447078b37eca8dbe18e499790925f21e947b7f8a14e83744 Windows 8 key

        public static string DecodeAndDecrypt(string cipherText, string iv)
        {
            return AesDecrypt(cipherText, iv);
        }

        public static string AesDecrypt(string ciphertextString, string iv)
        {
            IBuffer keyBuffer = CryptographicBuffer.ConvertStringToBinary("884c3c06f46f3fa0ec8c81a21687d5c3", BinaryStringEncoding.Utf8);
            IBuffer ivBuffer = CryptographicBuffer.ConvertStringToBinary(iv, BinaryStringEncoding.Utf8);
            IBuffer cipherBuffer = CryptographicBuffer.DecodeFromBase64String(ciphertextString);

            SymmetricKeyAlgorithmProvider symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            // create symmetric key from derived password material
            CryptographicKey symmKey = symProvider.CreateSymmetricKey(keyBuffer);

            // encrypt data buffer using symmetric key and derived salt material
            IBuffer resultBuffer = CryptographicEngine.Decrypt(symmKey, cipherBuffer, ivBuffer);
            string result = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, resultBuffer);
            
            return result;
        }
    }
}
