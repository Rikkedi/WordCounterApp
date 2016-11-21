using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace WordCountGUI_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FolderBrowserDialog folderBrowser;
        private WordCountAdapter wordCounts;

        public MainWindow()
        {
            InitializeComponent();

            this.folderBrowser = new FolderBrowserDialog();
            this.folderBrowser.Description = "Select the directory that you want to scan.";
            this.folderBrowser.ShowNewFolderButton = false;
        }

        private void OpenFolder_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult result = this.folderBrowser.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = this.folderBrowser.SelectedPath;

                this.wordCounts = new WordCountAdapter(folderPath);

                // TODO - Make this non-blocking
                this.wordCounts.DiscoverFiles().Wait();
                this.wordCounts.ProcessFiles().Wait();
                Console.WriteLine("Done!");
            }
        }
    }
}
