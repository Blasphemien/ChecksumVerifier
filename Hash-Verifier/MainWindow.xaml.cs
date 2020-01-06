using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Hash_Verifier {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }


        // Main function
        private void FileDropped(object sender, DragEventArgs e) {
            ClearTextBlock();

            // Check if the data matches the windows drop format
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

                // Get data that is in the windows drop format
                string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);

                // Gets the check boxes that are checked
                var hashOptions = GetHashOptions().ToList();

                if (!hashOptions.Any())
                    throw new NullReferenceException();
                if (data == null)
                    throw new NullReferenceException();

                GetHash(data[0], hashOptions);
            }
        }

        // Creates a new textbox dynamically
        private TextBlock CreateTextBlock(string name) {
            var textBlock = new TextBlock();
            textBlock.Width = double.NaN;
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.Name = name;
            textBoxStack.Children.Add(textBlock);
            return textBlock;
        }

        private IEnumerable<CheckBox> GetHashOptions() {
            IEnumerable<CheckBox> hashOptions = checkBoxStack.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);

            return hashOptions;
        }

        private void GetHash(string data, List<CheckBox> hashOptions) {
            var algorithms = new Dictionary<string, string>();

            // Gets checksums for for selected types
            foreach (var item in hashOptions) {
                if (item.Name == "checkBox_md5")
                    algorithms.Add("textBox_MD5", "MD5");
                if (item.Name == "checkBox_sha256")
                    algorithms.Add("textBox_SHA256", "SHA256");
                if (item.Name == "checkBox_sha1")
                    algorithms.Add("textBox_SHA1", "SHA1");
            }

            if (!algorithms.Any())
                throw new NullReferenceException();

            foreach (var item in algorithms)
                CalculateHash(data, item.Key, item.Value);
        }

        private void CalculateHash(string data, string key, string value) {
            FileStream fileStream = null;
            FileInfo fileInfo = null;
            byte[] hashValue = null;
            var algorithm = HashAlgorithm.Create(value);

            try {
                // Get the data from the file
                fileInfo = new FileInfo(data);
                fileStream = fileInfo.Open(FileMode.Open);
            }
            catch (IOException ex) {
                MessageBox.Show(ex.ToString());
            }


            catch (UnauthorizedAccessException ex) {
                MessageBox.Show(ex.ToString());
            }

            if ((fileInfo == null) | (fileStream == null))
                throw new NullReferenceException();

            if (algorithm == null)
                throw new NullReferenceException();

            hashValue = algorithm.ComputeHash(fileStream);

            CreateTextBlock(key).Text = value + ": " + ConvertBytesToString(hashValue);


            fileStream.Dispose();
        }

        private string ConvertBytesToString(byte[] bytes) {
            var hashValue = "";

            // Convert bytes to string in hex format
            foreach (var byteIndex in bytes)
                hashValue += byteIndex.ToString("x2").ToUpper();

            return hashValue;
        }

        private void ClearTextBlock() {
            textBoxStack.Children.Clear();
        }
    }
}