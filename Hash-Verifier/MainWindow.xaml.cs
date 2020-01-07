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
        private readonly Hash _hash = new Hash();
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
                List<CheckBox> hashOptions = GetHashOptions().ToList();

                if (!hashOptions.Any()) {
                    MessageBox.Show("No check boxes selected");
                    throw new NullReferenceException("No check boxes selected");
                }
                if (data == null) {
                    MessageBox.Show("Data must be in windows drop format");
                    throw new NullReferenceException("Data must be in windows drop format");
                }
                
                GetHash(data[0], hashOptions);
            }
        }

        // Creates a new textbox dynamically
        private TextBlock CreateTextBlock(string name) {
            var textBlock = new TextBlock {
                Width = double.NaN,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.NoWrap,
                Name = name
            };
            textBoxStack.Children.Add(textBlock);
            return textBlock;
        }

        private IEnumerable<CheckBox> GetHashOptions() {
            IEnumerable<CheckBox> hashOptions = checkBoxStack.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
            return hashOptions;
        }

        private void GetHash(string data, List<CheckBox> hashOptions) {
            Dictionary<string, string> algorithmsDictionary = new Dictionary<string, string>();
            
            // Gets checksums for for selected types
            foreach (CheckBox item in hashOptions) {
                if (item.Name == "checkBox_md5") 
                    algorithmsDictionary.Add("textBox_md5", "MD5");
                if (item.Name == "checkBox_sha256") 
                    algorithmsDictionary.Add("textBox_sha256", "SHA256");
                if (item.Name == "checkBox_sha1") 
                    algorithmsDictionary.Add("textBox_sha1", "SHA1");
                if (item.Name == "checkBox_sha512")
                    algorithmsDictionary.Add("textBox_sha512", "SHA512");
            }

            foreach (KeyValuePair<string, string> item in algorithmsDictionary) {
                CreateTextBlock(item.Key).Text = _hash.CalculateHash(data, item.Value);
            }
            
        }

        private void ClearTextBlock() {
            textBoxStack.Children.Clear();
        }
    }
}