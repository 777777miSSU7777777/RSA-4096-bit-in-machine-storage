using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RSA
{
    class CryptService
    {
        private static RSACryptoServiceProvider RSA;
        private const int KeySize = 4096;
        private const int PartSize = 16;
        private const bool DoOAEP = true;

        static CryptService()
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            RSA = new RSACryptoServiceProvider(KeySize);
        }

        public static string GetInfo(bool includePrivate)
        {
            var info = "";
            var rsaParams = RSA.ExportParameters(includePrivate);

            info += "\r\r\r\nN: " + ConvertByteArrayToString(rsaParams.Modulus, " ") +
                    "\r\r\r\ne: " + ConvertByteArrayToString(rsaParams.Exponent, " ");

            if (includePrivate)
            {
                info += "\r\r\r\nP: " + ConvertByteArrayToString(rsaParams.P, " ") +
                        "\r\r\r\nQ: " + ConvertByteArrayToString(rsaParams.Q, " ") +
                        "\r\r\r\nd: " + ConvertByteArrayToString(rsaParams.D, " ");
            }

            var keys = RSA.LegalKeySizes;
            info += "\r\nMin key size: " + keys[0].MinSize +
                    "\r\nSkip key size: " + keys[0].SkipSize +
                    "\r\nMax key size: " + keys[0].MaxSize;

            return info;

        }

        private static byte[] ConvertStringToByteArray(string str, string separator, string trim)
        {
            var strings = str.Trim(trim.ToCharArray()).Split(separator.ToCharArray());
            byte[] result = new byte[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                result[i] = Convert.ToByte(strings[i]);
            }
            return result;
        }

        private static string ConvertByteArrayToString(byte[] bytes, string separator)
        {
            var result = "";
            foreach (byte b in bytes)
            {
                result += Convert.ToString(b) + separator;
            }
            return result.TrimEnd(separator.ToCharArray());
        }

        public static String Encrypt(string source)
        {
            var bytes = ConvertStringToByteArray(source, " ", " ");
            int iterCount = bytes.Length / PartSize;
            List<byte[]> cipherBytes = new List<byte[]>();
            for (int i = 0; i <= iterCount; i++)
            {
                byte[] temp = null;
                if (i == iterCount && PartSize * iterCount < bytes.Length)
                {
                    temp = new byte[bytes.Length - PartSize * iterCount];
                }
                else
                {
                    temp = new byte[PartSize];
                }
                Array.Copy(bytes, i * PartSize, temp, 0, temp.Length);
                byte[] encrypted = RSA.Encrypt(temp, false);
                cipherBytes.Add(encrypted);
            }

            var cipherString = "";
            foreach (var part in cipherBytes)
            {
                cipherString += ConvertByteArrayToString(part, " ");
            }

            cipherString.TrimEnd();

            return cipherString;
        }

        public static String Decrypt(string cipher)
        {
            var cipherBytes = ConvertStringToByteArray(cipher, " ", " ");
            int iterCount = cipherBytes.Length / PartSize;
            List<byte[]> sourceBytes = new List<byte[]>();
            byte[] decrypted = RSA.Decrypt(cipherBytes, false);
            return ConvertByteArrayToString(decrypted, " ");
        }
    }
}
