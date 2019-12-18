using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hash_Verifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void FileDropped(object sender, DragEventArgs e)
        {
            // Check if the data matches the windows drop format
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get data that is in the windows drop format
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                MessageBox.Show(GetHash(data[0]));
            }
        }

        private string GetHash(String data)
        {
            try
            {
                // Get the data from the file
                FileInfo fileInfo = new FileInfo(data);

                //Create filestream
                FileStream fileStream = fileInfo.Open(FileMode.Open);

                // Create instance of SHA class
                SHA256 sha256 = SHA256.Create();

                // Get hash as bytes
                Byte[] hashValue = sha256.ComputeHash(fileStream);

                fileStream.Close();

                return ConvertBytesToString(hashValue);
            }
            catch(IOException e)
            {
                return e.ToString();
            }
            catch(UnauthorizedAccessException e)
            {
                return e.ToString();
            }
            
        }

        private string ConvertBytesToString(Byte[] bytes)
        {
            string hashValue = "";

            // Convert bytes to string in hex format
            foreach(Byte index in bytes){
                hashValue += index.ToString("x2");
            }

            return "sha256: " + hashValue;
        }
    }
}
