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
        Library Library;
        
        public MainWindow()
        {
            InitializeComponent();

            Library = LibraryDefaults.getHardCodedDefaultLib();

            ErrorTextBox.Text = "Default Library generated" +Library.TimeStamp;



        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDia = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result = openFileDia.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) // Test result.
            {
                //Rhino.RhinoApp.WriteLine(openFileDia.FileName);
                ErrorTextBox.Text = ErrorTextBox.Text +"\n"+ ("Library exported to " + openFileDia.SelectedPath);
                CSVImportExport.ExportLibrary(Library, openFileDia.SelectedPath);

            }

        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDia = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result = openFileDia.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) // Test result.
            {
                //Rhino.RhinoApp.WriteLine(openFileDia.FileName);
                ErrorTextBox.Text = ErrorTextBox.Text + "\n" + ("Library imported from " + openFileDia.SelectedPath);
                Library.Import(CSVImportExport.ImportLibrary(openFileDia.SelectedPath));
            }
        }

    }
}
