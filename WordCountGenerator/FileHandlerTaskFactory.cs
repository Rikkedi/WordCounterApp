using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WordCountGenerator.Handlers;

namespace WordCountGenerator
{
    public static class FileHandlerTaskFactory
    {
        // Must ensure file exists before invoking
        public static async Task ProcessFile(FileInfo fileToProcess, ConcurrentDictionary<long, int> wordCounts)
        {
            // TODO : When caller is updated to handle checking task results for exception, have this throw
            //        ArgumentNullExceptions on these
            if (fileToProcess == null || wordCounts == null)
            {
                return;
            }

            if (!fileToProcess.Exists)
            {
                Console.WriteLine("File {0} does not exist", fileToProcess.FullName);
                return;
            }

            if (TextFileHandler.IsTextFile(fileToProcess.Extension))
            {
                long wordCount = await TextFileHandler.GetWordCount(fileToProcess);
                FileHandlerTaskFactory.UpdateWordCount(wordCounts, wordCount);
            }
            else if (ArchiveFileHandler.IsArchiveFile(fileToProcess.Extension))
            {
                List<long> archiveWordCounts = (List<long>) await ArchiveFileHandler.GetWordCount(fileToProcess);

                foreach (int wordCount in archiveWordCounts)
                {
                    FileHandlerTaskFactory.UpdateWordCount(wordCounts, wordCount);
                }
            }
            else
            {
                Console.WriteLine(
                    String.Format(
                        "Ignoring file type {0} encountered on {1}", 
                        fileToProcess.Extension, 
                        fileToProcess.FullName)
                        );
            }
        }

        private static void UpdateWordCount(ConcurrentDictionary<Int64, Int32> wordCounts, Int64 wordCount)
        {
            if (wordCounts.ContainsKey(wordCount))
            {
                wordCounts[wordCount]++;
            }
            else
            {
                wordCounts.TryAdd(wordCount, 1);
            }
        }
    }
}
