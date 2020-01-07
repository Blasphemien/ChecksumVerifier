using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;

namespace Hash_Verifier {
    class Hash : iHash {
       
        public string CalculateHash(string data, string selectedAlgorithm) {
            FileStream fileStream = null;

            try {
                FileInfo fileInfo = new FileInfo(data);
                fileStream = fileInfo.Open(FileMode.Open);
            } catch (IOException ex) {
                MessageBox.Show(ex.ToString());
            } catch (UnauthorizedAccessException ex) {
                MessageBox.Show(ex.ToString());
            }

            HashAlgorithm algorithm = HashAlgorithm.Create(selectedAlgorithm);

            // Safety check
            if(algorithm == null || fileStream == null)
                throw new NullReferenceException();

            byte[] hashBytes = algorithm.ComputeHash(fileStream);
            fileStream.Dispose();
            return selectedAlgorithm + ": "+ ConvertToHex(hashBytes);
        }

        public string ConvertToHex(byte[] hashBytes) {
            string hashValue = "";

            // Convert bytes to string in hex format
            foreach (var byteIndex in hashBytes)
                hashValue += byteIndex.ToString("x2").ToUpper();

            return hashValue;
        }
    }
}
