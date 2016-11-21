using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WordCountGenerator.Handlers;

namespace WordCountGenerator
{
    public static class FileHandler
    {
        // Must ensure file exists before invoking
        public static async Task ProcessFile(FileInfo fileToProcess, ConcurrentDictionary<string, long> aggregateWordCounts)
        {
            // TODO : When caller is updated to handle checking task results for exception, have this throw
            //        ArgumentNullExceptions on these
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

            if (TextFileHandler.IsHandleable(fileToProcess.Extension))
            {
                fileWordCounts = await TextFileHandler.GetWordCount(fileToProcess);
            }
            else if (ArchiveFileHandler.IsHandleable(fileToProcess.Extension))
            {
                fileWordCounts = await ArchiveFileHandler.GetWordCount(fileToProcess);
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

            FileHandler.MergeWordCounts(aggregateWordCounts, fileWordCounts);
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
