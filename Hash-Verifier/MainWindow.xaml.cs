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
        void FileDropped(object sender, DragEventArgs e) {
            ClearTextBlock();

            // Check if the data matches the windows drop format
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

                // Get data that is in the windows drop format
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Gets the check boxes that are checked
                IEnumerable<CheckBox> checkBoxCheckedList = this.checkBoxStack.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);

                // Check if a checkbox has been checked
                if (!checkBoxCheckedList.Any()) {
                    MessageBox.Show("Please select what checksums you would like to view");
                    return;
                }
                GetHash(data[0], checkBoxCheckedList);
            }
        }

        // Creates a new textbox dynamically
        TextBlock CreateTextBlock(string name) {
            TextBlock textBlock = new TextBlock();
            textBlock.Width = Double.NaN;
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.Name = name;
            textBoxStack.Children.Add(textBlock);
            return textBlock;
        }

        private void GetHash(string data, IEnumerable<CheckBox> checkBoxCheckedList) {
            // Copy IEnum so its not changes when enumerating
            List<CheckBox> checkBoxList = new List<CheckBox>();
            checkBoxList = checkBoxCheckedList.ToList();
            Dictionary<string, string> algorithms = new Dictionary<string, string>(); 
            
            // Gets checksums for for selected types
            foreach (CheckBox item in checkBoxList) {
                if (item.Name == "checkBox_sha256")
                    algorithms.Add("textBox_SHA256", "SHA256");             
                if (item.Name == "checkBox_sha1")
                    algorithms.Add("textBox_SHA1", "SHA1");
                if (item.Name == "checkBox_md5")
                    algorithms.Add("textBox_MD5", "MD5");
            }
            CalculateHash(data, algorithms);
        }

        private void CalculateHash(string data, Dictionary<string, string> dictionary) {
            FileStream fileStream = null;
            FileInfo fileInfo = null;

            try
            {
                // Get the data from the file
                fileInfo = new FileInfo(data);
                fileStream = fileInfo.Open(FileMode.Open);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (fileInfo == null | fileStream == null)
            {
                throw new NullReferenceException();
            }

            foreach (var item in dictionary) {
                HashAlgorithm algorithm = HashAlgorithm.Create(item.Value);
                byte[] hashValue = algorithm.ComputeHash(fileStream);
                CreateTextBlock(item.Key).Text = item.Value + ": " + ConvertBytesToString(hashValue);
            }
            fileStream.Dispose();
        }

        private string ConvertBytesToString(Byte[] bytes) {
            string hashValue = null;

            // Convert bytes to string in hex format
            foreach (Byte byteIndex in bytes) 
                hashValue += byteIndex.ToString("x2");
            
            return hashValue;
        }

        private void ClearTextBlock() {
            textBoxStack.Children.Clear();
        }
    }
}
