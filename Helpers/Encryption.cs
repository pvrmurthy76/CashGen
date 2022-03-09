using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CashGen.Helpers
{
    internal class Encryption
    {
        private string Encrypt(
          string PlainText,
          string Password,
          string Salt = "Kosher",
          string HashAlgorithm = "SHA1",
          int PasswordIterations = 2,
          string InitialVector = "-A+5v[wB>2bXN?gv",
          int KeySize = 256)
        {
            if (string.IsNullOrEmpty(PlainText))
                return "";
            byte[] bytes1 = Encoding.ASCII.GetBytes(InitialVector);
            byte[] bytes2 = Encoding.ASCII.GetBytes(Salt);
            byte[] bytes3 = Encoding.UTF8.GetBytes(PlainText);
            byte[] bytes4 = ((DeriveBytes)new PasswordDeriveBytes(Password, bytes2, HashAlgorithm, PasswordIterations)).GetBytes(KeySize / 8);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            ((SymmetricAlgorithm)rijndaelManaged).Mode = CipherMode.CBC;
            byte[] inArray = (byte[])null;
            using (ICryptoTransform encryptor = ((SymmetricAlgorithm)rijndaelManaged).CreateEncryptor(bytes4, bytes1))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        ((Stream)cryptoStream).Write(bytes3, 0, bytes3.Length);
                        cryptoStream.FlushFinalBlock();
                        inArray = memoryStream.ToArray();
                        ((Stream)memoryStream).Close();
                        ((Stream)cryptoStream).Close();
                    }
                }
            }
          ((SymmetricAlgorithm)rijndaelManaged).Clear();
            return Convert.ToBase64String(inArray);
        }

        private string Decrypt(
          string CipherText,
          string Password,
          string Salt = "Kosher",
          string HashAlgorithm = "SHA1",
          int PasswordIterations = 2,
          string InitialVector = "-A+5v[wB>2bXN?gv",
          int KeySize = 256)
        {
            try
            {
                if (string.IsNullOrEmpty(CipherText))
                    return "";
                byte[] bytes1 = Encoding.ASCII.GetBytes(InitialVector);
                byte[] bytes2 = Encoding.ASCII.GetBytes(Salt);
                byte[] numArray1 = Convert.FromBase64String(CipherText);
                byte[] bytes3 = ((DeriveBytes)new PasswordDeriveBytes(Password, bytes2, HashAlgorithm, PasswordIterations)).GetBytes(KeySize / 8);
                RijndaelManaged rijndaelManaged = new RijndaelManaged();
                ((SymmetricAlgorithm)rijndaelManaged).Mode = CipherMode.CBC;
                byte[] numArray2 = new byte[numArray1.Length];
                int num = 0;
                using (ICryptoTransform decryptor = ((SymmetricAlgorithm)rijndaelManaged).CreateDecryptor(bytes3, bytes1))
                {
                    using (MemoryStream memoryStream = new MemoryStream(numArray1))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            num = ((Stream)cryptoStream).Read(numArray2, 0, numArray2.Length);
                            ((Stream)memoryStream).Close();
                            ((Stream)cryptoStream).Close();
                        }
                    }
                }
              ((SymmetricAlgorithm)rijndaelManaged).Clear();
                return Encoding.UTF8.GetString(numArray2, 0, num);
            }
            catch
            {
                return "Invalid encryption key supplied";
            }
        }
    }
}
