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

namespace WordCountGUI_WPF
{
    /// <summary>
    /// Interaction logic for WordCountHome.xaml
    /// </summary>
    public partial class WordCountHome : Page
    {
        private FolderBrowserDialog folderBrowser;
        internal string folderPath;

        public WordCountHome()
        {
            InitializeComponent();

            this.folderBrowser = new FolderBrowserDialog();
            this.folderBrowser.Description = "Select the directory that you want to scan.";
            this.folderBrowser.ShowNewFolderButton = false;

            this.folderPath = String.Empty;
        }

        private void OpenFolder_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult result = this.folderBrowser.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                this.folderPath = this.folderBrowser.SelectedPath;

                WordCountReport reportPage = new WordCountReport();
                this.NavigationService.Navigate(reportPage);
            }
        }
    }
}
