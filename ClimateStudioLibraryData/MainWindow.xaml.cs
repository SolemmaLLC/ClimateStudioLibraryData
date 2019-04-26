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
using ArchsimLib.LibraryObjects;
using ArchsimLib.CSV;
using System.IO;
using System.Windows.Forms;

namespace ClimateStudioLibraryData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            var lib = LibraryDefaults.getHardCodedDefaultLib();


            string folderPath = @"C:\DIVA\Temp";

            folderPath += @"\ArchsimLibrary-" + lib.TimeStamp.Year + "-" + lib.TimeStamp.Month + "-" + lib.TimeStamp.Day + "-" + lib.TimeStamp.Hour + "-" + lib.TimeStamp.Minute;

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);


            


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {

                    TextBX.Text = fbd.SelectedPath;

                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");

                }
            }
        }

    }
}
