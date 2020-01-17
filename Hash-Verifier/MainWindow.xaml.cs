using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hash_Verifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Hash _hash = new Hash();
     
        public MainWindow()
        {
            InitializeComponent();
        }


        private string[] GetDataFromFile(DragEventArgs file) 
        {
            if (file.Data.GetDataPresent(DataFormats.FileDrop))
                return (string[]) file.Data.GetData(DataFormats.FileDrop);

            return null;
        }

        // Main function
        private void FileDropped(object sender, DragEventArgs e)
        {
            // Validate controls before proceeding
            if (!ValidateStackPanelVerify()) 
                return;
            if (!ValidateCheckBoxes())
                return;

            ClearTextBlock();

            try
            {
                GetHash(GetDataFromFile(e));
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + "Stack Trace:" + "\n" + ex.StackTrace);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + "Stack Trace:" + "\n" + ex.StackTrace);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + "Stack Trace:" + "\n" + ex.StackTrace);
            }

        }

        private TextBox CreateTextBox(string name)
        {
            TextBox textBox = new TextBox
            {
                Width = double.NaN,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.NoWrap,
                IsReadOnly = true,
                Name = name
            };
            StackPanelResults.Children.Add(textBox);
            return textBox;
        }
        private IEnumerable<CheckBox> GetSelectedHashAlgorithms() 
        {
            IEnumerable<CheckBox> hashOptions = StackPanelHashOptions.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
            return hashOptions;
        }
        private IEnumerable<TextBox> GetResultTextBlocks()
        {
            IEnumerable<TextBox> textBlocks = StackPanelResults.Children.OfType<TextBox>();
            return textBlocks;
        }
        private Dictionary<string, string> GetAlgorithmsDictionary() {

            Dictionary<string, string> algorithmsDictionary = new Dictionary<string, string>();

            // Adds the corresponding textBlocks so we can loop through the results and their algorithm types later
            foreach (CheckBox item in GetSelectedHashAlgorithms().ToList())
            {
                if (item.Name == "checkBox_md5")
                    algorithmsDictionary.Add("textBlock_md5", "MD5");
                if (item.Name == "checkBox_sha256")
                    algorithmsDictionary.Add("textBlock_sha256", "SHA256");
                if (item.Name == "checkBox_sha1")
                    algorithmsDictionary.Add("textBlock_sha1", "SHA1");
                if (item.Name == "checkBox_sha512")
                    algorithmsDictionary.Add("textBlock_sha512", "SHA512");
            }
            return algorithmsDictionary;
        }
        private void GetHash(string[] data) 
        {
            foreach (KeyValuePair<string, string> item in GetAlgorithmsDictionary())
                CreateTextBox(item.Key).Text = _hash.CalculateHash(data[0], item.Value);
            
            if (CheckBoxVerify.IsChecked == true)
                MessageBox.Show(VerifyHash());
        }

        private bool ValidateCheckBoxes() {
            if (!GetSelectedHashAlgorithms().Any()) {
                MessageBox.Show("Please select the type(s) of has you would like to calculate");
                return false;
            }
            return true;
        }

        private bool ValidateStackPanelVerify()
        {
            if (CheckBoxVerify.IsChecked == true && string.IsNullOrEmpty(TextBoxVerify.Text))
            {
                MessageBox.Show("You have indicated you would like to validate the resulting hash. Please enter a hash to verify");
                return false;
            }
            return true;
        }

        private string VerifyHash() {

            //  Loop through each of the hash result text blocks
            foreach (TextBox textBoxResult in GetResultTextBlocks().ToList())
            {
                string trimmedHash = textBoxResult.Text.Substring(textBoxResult.Text.IndexOf(' ') + 1);

                // Gets the corresponding algorithm types of the textBlocks from the dictionary
                // Checks if the hashed result matches the users input
                // Dynamically print the algorithm that matches
                foreach (KeyValuePair<string, string> item in GetAlgorithmsDictionary())
                {
                    if (textBoxResult.Name == item.Key && trimmedHash == TextBoxVerify.Text.Trim())
                        return item.Value + " matches";
                }
            }
            return "WARNING! NO HASHES MATCH!";
        }

        private void ClearTextBlock()
        {
            StackPanelResults.Children.Clear();
        }

        private void CheckBoxVerify_OnClick_(object sender, RoutedEventArgs e) {
            if (CheckBoxVerify.IsChecked == true)
                TextBoxVerify.IsEnabled = true;
                
            if (CheckBoxVerify.IsChecked == false)
                TextBoxVerify.IsEnabled = false;
        }
    }
}