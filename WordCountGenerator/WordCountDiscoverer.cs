using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace WordCountGenerator
{
    public class WordCountDiscoverer
    {
        private DirectoryInfo rootDirectory;
        private const int MaxRetries = 5;

        public ConcurrentDictionary<string, long> AggregateWordOccurrenceCount { get; private set; }

        public WordCountDiscoverer(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            try
            {
                this.rootDirectory = new DirectoryInfo(path);
            }
            catch (SecurityException se)
            {
                Console.WriteLine("User does not have permission to access {0}", path);
                throw se;
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("Path contains invalid characters");
                throw ae;
            }
            catch (PathTooLongException pe)
            {
                Console.WriteLine("Input path too long.");
                throw pe;
            }

            if (!this.rootDirectory.Exists)
            {
                throw new ArgumentException("Input is not a directory!", "path");
            }

            this.AggregateWordOccurrenceCount = new ConcurrentDictionary<string, long>();
        }

        public IEnumerable<FileInfo> DiscoverFiles()
        {
            if (this.rootDirectory == null || !this.rootDirectory.Exists)
            {
                return null;
            }

            try
            {
                return DiscoverAllFilesUnderBaseDirectory();
            }
            catch (SecurityException se)
            {
                // Just skip the file if we can't access it
                Console.WriteLine(se.Message);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                Console.WriteLine(dnfe.Message);
            }

            return null;
        }

        private IEnumerable<FileInfo> DiscoverAllFilesUnderBaseDirectory()
        {
            // Start with the files in the base directory
            ConcurrentQueue<FileInfo> concurrentFileQueue = new ConcurrentQueue<FileInfo>(this.rootDirectory.EnumerateFiles());

            // Explore any subdirectories
            IEnumerable<DirectoryInfo> subdirs = this.rootDirectory.EnumerateDirectories();
            ConcurrentQueue<DirectoryInfo> subDirectoriesToExplore = new ConcurrentQueue<DirectoryInfo>(subdirs);

            int retries = 0;

            while (subDirectoriesToExplore.Count > 0 && retries < WordCountDiscoverer.MaxRetries)
            {
                DirectoryInfo currentDirectory;
                subDirectoriesToExplore.TryDequeue(out currentDirectory);

                if (currentDirectory == null)
                {
                    retries++;
                    Thread.Sleep(10);
                    continue;
                }

                retries = 0;
                try
                {
                    Parallel.ForEach<FileInfo>(currentDirectory.EnumerateFiles(), (currentFile) =>
                    {
                        concurrentFileQueue.Enqueue(currentFile);
                    });
                
                    Parallel.ForEach<DirectoryInfo>(currentDirectory.EnumerateDirectories(), (directory) =>
                    {
                        subDirectoriesToExplore.Enqueue(directory);
                    });
                }
                catch (SecurityException se)
                {
                    // Just skip the directory if we can't access it
                    Console.WriteLine(String.Format("Security Exception encountered under {0}.  Results may contain a partial set of content. Exception: {1}", currentDirectory, se.Message));
                }
            }

            return concurrentFileQueue;
        }

        public async Task ProcessFilesAsync(ConcurrentQueue<FileInfo> fileQueue)
        {
            if (fileQueue == null || fileQueue.Count == 0)
            {
                return;
            }

            FileHandlerController fileHandler = new FileHandlerController();

            while (!fileQueue.IsEmpty)
            {
                FileInfo currentFile;
                if (fileQueue.TryDequeue(out currentFile))
                {
                    await fileHandler.ProcessFile(currentFile, this.AggregateWordOccurrenceCount);
                }
                else
                {
                    await Task.Run(() => Thread.Sleep(10)); 
                }
            }
        }

        /// <summary>
        ///  Returns a dictionary that has as Key the word occurrence and as Value the number 
        ///  of words that occur that many times
        /// </summary>
        /// <returns>Enumeration of the number of times a word occurred and how many words occurred that many times</returns>
        public IEnumerable<KeyValuePair<long, int>> GetCountOfWordsByWordsWithCount()
        {
            return this.AggregateWordOccurrenceCount
                .GroupBy(wordCount => wordCount.Value, wordCount => wordCount.Key)
                .Select(countPair => new KeyValuePair<long, int>(countPair.Key, countPair.Count()))
                .OrderBy(countPair => countPair.Key);
        }
    }
}
