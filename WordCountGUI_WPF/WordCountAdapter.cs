using System.Collections.Concurrent;
using System.Threading.Tasks;
using WordCountGenerator;

namespace WordCountGUI_WPF
{
    internal class WordCountAdapter
    {
        private FolderHandler wordCountGenerator;

        public ConcurrentDictionary<long, int> WordCounts
        {
            get
            {
                if (this.wordCountGenerator == null)
                {
                    return null;
                }

                return this.wordCountGenerator.FileCountsByWordCount;
            }
        }

        public WordCountAdapter(string folderPath)
        {
            this.wordCountGenerator = new FolderHandler(folderPath);
        }

        public async Task DiscoverFiles()
        {
            if (this.wordCountGenerator == null)
            {
                return;
            }

            await this.wordCountGenerator.DiscoverFilesToProcess();
        }

        public async Task ProcessFiles()
        {
            if (this.wordCountGenerator == null)
            {
                return;
            }

            await this.wordCountGenerator.ProcessFiles();
        }
    }
}
