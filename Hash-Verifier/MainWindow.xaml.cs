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
        public void FileDropped(object sender, DragEventArgs e) {
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
        private TextBlock CreateTextBlock(string name) {
            TextBlock textBlock = new TextBlock();
            textBlock.Width = Double.NaN;
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.Name = name;
            textBoxStack.Children.Add(textBlock);
            return textBlock;
        }
        private string GetHash(String data, IEnumerable<CheckBox> checkBoxCheckedList) {
            // Copy IEnum so its not changes when enumerating
            List<CheckBox> checkBoxList = new List<CheckBox>();
            checkBoxList = checkBoxCheckedList.ToList();

            // Store checkSum values   
            Byte[] hashValue = null;

            // Get the data from the file
            FileInfo fileInfo = new FileInfo(data);            
            FileStream fileStream = fileInfo.Open(FileMode.Open);

            // Gets checksums for for selected types
            try {          
                foreach (CheckBox item in checkBoxList) {
                    if(item.Name == "checkBox_sha256") {
                        SHA256 sha256 = SHA256.Create();
                        hashValue = sha256.ComputeHash(fileStream);
                        CreateTextBlock("textBlock_SHA256").Text = "SHA256: " + ConvertBytesToString(hashValue);                      
                    }
                    if(item.Name == "checkBox_sha1") {
                        SHA1 sha11 = SHA1.Create();
                        hashValue = sha11.ComputeHash(fileStream);
                        CreateTextBlock("textBlock_SHA1").Text = "SHA1: " + ConvertBytesToString(hashValue);
                    }
                    if(item.Name == "checkBox_md5") {
                        MD5 md5 = MD5.Create();
                        hashValue = md5.ComputeHash(fileStream);
                        CreateTextBlock("textBlock_MD5").Text = "MD5: " + ConvertBytesToString(hashValue);
                    }                                     
                }
                fileStream.Close();
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
        private void ClearTextBlock() {
            textBoxStack.Children.Clear();
        }
    }
}
