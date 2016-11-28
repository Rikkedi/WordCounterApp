using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WordCountGenerator.Handlers;

namespace WordCountGenerator
{
    public class FileHandlerController
    {
        private ArchiveFileHandler archiveHandler;
        private TextFileHandler textHandler;

        public FileHandlerController()
        {
            this.textHandler = new TextFileHandler();
            this.archiveHandler = new ArchiveFileHandler(this.textHandler);
        }

        // Must ensure file exists before invoking
        public async Task ProcessFile(FileInfo fileToProcess, ConcurrentDictionary<string, long> aggregateWordCounts)
        {
            if (fileToProcess == null || aggregateWordCounts == null)
            {
                return;
            }

            if (!fileToProcess.Exists)
            {
                Console.WriteLine("File {0} does not exist", fileToProcess.FullName);
                return;
            }

            Dictionary<string, long> fileWordCounts;

            if (this.textHandler.IsHandleable(fileToProcess.Extension))
            {
                fileWordCounts = await this.textHandler.GetWordCount(fileToProcess);
            }
            else if (this.archiveHandler.IsHandleable(fileToProcess.Extension))
            {
                fileWordCounts = await this.archiveHandler.GetWordCount(fileToProcess);
            }
            else
            {
                Console.WriteLine(
                    String.Format(
                        "Ignoring file type {0} encountered on {1}", 
                        fileToProcess.Extension, 
                        fileToProcess.FullName)
                        );

                return;
            }

            FileHandlerController.MergeWordCounts(aggregateWordCounts, fileWordCounts);
        }

        private static void MergeWordCounts(ConcurrentDictionary<string, long> aggregateWordCounts, Dictionary<string, long> wordCounts)
        {
            foreach(KeyValuePair<string, long> wordCount in wordCounts)
            {
                aggregateWordCounts.AddOrUpdate(
                    wordCount.Key, 
                    wordCount.Value, 
                    (key, vOld) => vOld += wordCount.Value);
            }
        }
    }
}
