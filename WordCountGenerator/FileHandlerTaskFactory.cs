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
            if (fileToProcess == null)
            {
                throw new ArgumentNullException("fileToProcess");
            }
            
            if (wordCounts == null)
            {
                throw new ArgumentNullException("wordCounts");
            }

            if (!fileToProcess.Exists)
            {
                Console.WriteLine("File {0} does not exist", fileToProcess.FullName);
                return;
            }

            if (TextFileHandler.IsTextFile(fileToProcess.Extension))
            {
                long wordCount = await TextFileHandler.GetWordCount(fileToProcess);

                if (wordCounts.ContainsKey(wordCount))
                {
                    wordCounts[wordCount]++;
                }
                else
                {
                    wordCounts.TryAdd(wordCount, 1);
                }
            }
            else if (ArchiveFileHandler.IsArchiveFile(fileToProcess.Extension))
            {
                List<long> archiveWordCounts = (List<long>) await ArchiveFileHandler.GetWordCount(fileToProcess);

                foreach (int wordCount in archiveWordCounts)
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
    }
}
