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

            // TODO - Since all Handlers follow the same interface, you should have a Collection of IFileHandler 
            // objects.  That way, below, you only have to loop through them and call each one's IsHandleable 
            // method.  If it is handled, call that one's GetWordCount and then do a 'continue' on the loop. 
            // This means that to add new handlers, you just add them to the constroctor and everything else is 
            // taken care of.  May be a further improvement to have the handlers all passed in by the owner
            // of the FileHandlerController (or a factory that just has a 'GetFileHandlerControler' which
            // returns one of these and THAT factory passes in the various IFileHandlers to be used.
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
