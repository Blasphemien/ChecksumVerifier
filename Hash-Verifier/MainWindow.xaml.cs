using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;

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

                List<CheckBox> selectedAlgorithms = GetSelectedHashAlgorithms().ToList();
                Dictionary<string, string> algorithmsDictionary = GetAlgorithmsDictionary(selectedAlgorithms);

                if (data == null) 
                    MessageBox.Show("Data must be in windows drop format");

                GetHash(data[0], algorithmsDictionary);
            }
        }

        private TextBlock CreateTextBlock(string name) {
            TextBlock textBlock = new TextBlock {
                Width = double.NaN,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.NoWrap,
                Name = name
            };
            TextBlockResults.Children.Add(textBlock);
            return textBlock;
        }

        private IEnumerable<CheckBox> GetSelectedHashAlgorithms() {
            IEnumerable<CheckBox> hashOptions = CheckBoxStack.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
            return hashOptions;
        }

        private IEnumerable<TextBlock> GetResultTextBlocks() {
            IEnumerable<TextBlock> textBlocks = TextBlockResults.Children.OfType<TextBlock>();
            return textBlocks;
        }

        private Dictionary<string, string> GetAlgorithmsDictionary(List<CheckBox> hashOptions) {
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
            return algorithmsDictionary;
        }

        private void GetHash(string data, Dictionary<string, string> algorithmsDictionary) {
            foreach (KeyValuePair<string, string> item in algorithmsDictionary) {
               CreateTextBlock(item.Key).Text = _hash.CalculateHash(data, item.Value);
            }
            if (CheckBoxVerify.IsChecked == true && string.IsNullOrEmpty(TextBoxVerify.Text))
                MessageBox.Show("Enter a hash to verify against");
            else {
                MessageBox.Show(VerifyHash());
            }
        }

        private string VerifyHash() {
            string result = null;
            List<TextBlock> results = GetResultTextBlocks().ToList();
            Dictionary<string, string> algorithmsDic = GetAlgorithmsDictionary(GetSelectedHashAlgorithms().ToList());

            //  Loop throuh each of the hash result text blocks
            foreach (TextBlock textBlock in results) {
                string trimmedHash = textBlock.Text.Substring(textBlock.Text.IndexOf(' ') + 1);
                // Loop through the algorithms dictionary and check if its value equals the hash result
                foreach (KeyValuePair<string, string> item in algorithmsDic) {
                    if (textBlock.Name == item.Key && trimmedHash == TextBoxVerify.Text.Trim()) {
                        result = item.Value + " matches";
                    } else {
                        result = "WARNING! " + item.Value + " DOES NOT MATCH!";
                    }
                }
            }

            return result;
        }

        private void ClearTextBlock() {
            TextBlockResults.Children.Clear();
        }
    }
}