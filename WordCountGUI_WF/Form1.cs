using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordCountGenerator;

namespace WordCountGUI
{
    public partial class Form1 : Form
    {
        public string folderName { get; private set; }
        public BindingSource dataBinding { get; private set; }

        private FolderHandler folderProcessor;
        private Task fileDiscoveryTask;
        private Task fileProcessingTask;
        private bool fileProcessingStarted;


        public Form1()
        {
            InitializeComponent();
            this.fileProcessingStarted = false;
            this.dataBinding = new BindingSource();
        }

        private void openToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.folderName = folderBrowserDialog1.SelectedPath;
                this.folderProcessor = new FolderHandler(this.folderName);
            }

            this.DiscoverInputFolder();
            this.PollProcessResults();
        }

        private void DiscoverInputFolder()
        {
            if (this.folderProcessor == null)
            {
                return;
            }

            this.fileProcessingStarted = true;
            this.fileDiscoveryTask = folderProcessor.DiscoverFilesToProcess();
            this.fileDiscoveryTask.Start();
        }

        private void PollProcessResults()
        {
            if (!fileProcessingStarted || !(this.fileDiscoveryTask.Status == TaskStatus.Created))
            {
                return;
            }

            this.fileProcessingTask = this.folderProcessor.ProcessFiles();
            this.fileProcessingTask.Start();

            this.dataBinding.DataSource = this.folderProcessor.FileCountsByWordCount;
        }

        private void form1BindingSource_CurrentChanged(Object sender, EventArgs e)
        {
        }
    }
}
