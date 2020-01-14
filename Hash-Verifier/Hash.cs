using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Security.Policy;

namespace Hash_Verifier {
    class Hash : iHash {
       
        public string CalculateHash(string data, string selectedAlgorithm) {
            HashAlgorithm algorithm = HashAlgorithm.Create(selectedAlgorithm);
            FileStream fileStream = null;

            try {
                FileInfo fileInfo = new FileInfo(data);
                fileStream = fileInfo.Open(FileMode.Open);
            } catch (IOException ex) {
                MessageBox.Show(ex.Message);
            } catch (UnauthorizedAccessException ex) {
                MessageBox.Show(ex.Message);
            }

            
            // Safety check
            if(algorithm == null || fileStream == null) {
                throw new NullReferenceException("nullres");
            }
            
            byte[] hashBytes = algorithm.ComputeHash(fileStream);
            fileStream.Dispose();
            return selectedAlgorithm + ": "+ ConvertBytesToString(hashBytes);
        }

        public string ConvertBytesToString(byte[] hashBytes) {
            string hashValue = string.Empty;

            foreach (var byteIndex in hashBytes)
                hashValue += byteIndex.ToString("x2");

            return hashValue;
        }

        public bool VerifyHash(string hashToVerify, List<TextBox> hashValues) {
            bool doesMatch = false;

            foreach (TextBox hashValue in hashValues) {
                if (hashValue.Text == hashToVerify) 
                    doesMatch = true;
            }

            return doesMatch;
        }
    }
}
