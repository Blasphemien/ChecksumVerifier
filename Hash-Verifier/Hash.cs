using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace Hash_Verifier
{
    class Hash : iHash
    {
        public string CalculateHash(string data, string selectedAlgorithm)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create(selectedAlgorithm);

            if (algorithm != null) {
                FileInfo fileInfo = new FileInfo(data);

                FileStream fileStream = fileInfo.Open(FileMode.Open);

                byte[] hashBytes = algorithm.ComputeHash(fileStream);

                fileStream.Close();

                return selectedAlgorithm + ": " + ConvertBytesToString(hashBytes);
            }

            return "Could not create " + "\"" + selectedAlgorithm + "\"";
        }
        public string ConvertBytesToString(byte[] hashBytes)
        {
            string hashValue = string.Empty;
            foreach (var byteIndex in hashBytes)
            {
                hashValue += byteIndex.ToString("x2");
            }
            return hashValue;
        }
        public bool VerifyHash(string hashToVerify, List<TextBox> hashValues)
        {
            bool doesMatch = false;
            foreach (TextBox hashValue in hashValues)
            {
                if (hashValue.Text == hashToVerify)
                {
                    doesMatch = true;
                }
            }

            return doesMatch;
        }
    }
}
