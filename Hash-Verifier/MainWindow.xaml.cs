using System;
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
            if (e.Data.GetDataPresent(DataFormats.FileDrop)){
                // Get data that is in the windows drop format
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                GetHash(data[0]);
            }
        }

        private Byte GetHash(String data)
        {

        }
    }
}
