using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace Hash_Verifier {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        public void FileDropped(object sender, DragEventArgs e) {         
            // Check if the data matches the windows drop format
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                // Get data that is in the windows drop format
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);           
                textBlock_SHA256.Text = "sha256: " + GetHash(data[0], "sha256");
                textBlock_SHA1.Text = ("sha1: " + GetHash(data[0], "sha1"));
                textBlock_MD5.Text = ("md5: " + GetHash(data[0], "md5"));
            }
        }

        private string GetHash(String data, String hashType) {
            string checksumType = hashType;
            Byte[] hashValue = null;
            try {    
                // Get the data from the file
                FileInfo fileInfo = new FileInfo(data);
                //Create filestream
                FileStream fileStream = fileInfo.Open(FileMode.Open);
                // Create instance of SHA class
                switch (checksumType) {
                    case "sha256": 
                        SHA256 sha256 = SHA256.Create();
                        hashValue = sha256.ComputeHash(fileStream);
                        fileStream.Close();
                        break;
                    case "sha1":
                        SHA1 sha11 = SHA1.Create();
                        hashValue = sha11.ComputeHash(fileStream);
                        fileStream.Close();
                        break;
                    case "md5":
                        MD5 md5 = MD5.Create();
                        hashValue = md5.ComputeHash(fileStream);
                        fileStream.Close();
                        break;
                }
                return ConvertBytesToString(hashValue);
            }
            catch (IOException e) { return e.ToString(); }
            catch (UnauthorizedAccessException e) { return e.ToString(); }
        }
        private string ConvertBytesToString(Byte[] bytes) {
            string hashValue = null;
            // Convert bytes to string in hex format
            foreach (Byte byteIndex in bytes) {
                hashValue += byteIndex.ToString("x2");
            }
            return hashValue;
        }
    }
}
