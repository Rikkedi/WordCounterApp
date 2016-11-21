using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace WordCountGenerator
{
    public class FolderHandler
    {
        private ConcurrentQueue<FileInfo> concurrentFileQueue;
        private DirectoryInfo rootDirectory;

        public ConcurrentDictionary<string, long> FileCountsByWordCount { get; private set; }

        public FolderHandler(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            try
            {
                this.rootDirectory = new DirectoryInfo(path);
            }
            catch (SecurityException)
            {
                Console.WriteLine("User does not have permission to access {0}", path);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Path contains invalid characters");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("Input path too long.");
            }

            this.FileCountsByWordCount = new ConcurrentDictionary<string, long>();
        }

        public void DiscoverFiles()
        {
            if (this.rootDirectory == null || !this.rootDirectory.Exists)
            {
                return;
            }

            try
            {
                // Start with the files in the base directory
                this.concurrentFileQueue = new ConcurrentQueue<FileInfo>(this.rootDirectory.EnumerateFiles());

                // Explore any subdirectories
                IEnumerable<DirectoryInfo> subdirs = this.rootDirectory.EnumerateDirectories();
                Queue<DirectoryInfo> subDirectoriesToExplore = new Queue<DirectoryInfo>(subdirs);

                while (subDirectoriesToExplore.Count > 0)
                {
                    DirectoryInfo currentDirectory = subDirectoriesToExplore.Dequeue();

                    Parallel.ForEach<FileInfo>(currentDirectory.EnumerateFiles(), (currentFile) =>
                    {
                        this.concurrentFileQueue.Enqueue(currentFile);
                    });

                    foreach (DirectoryInfo subDir in currentDirectory.EnumerateDirectories())
                    {
                        subDirectoriesToExplore.Enqueue(subDir);
                    }
                }
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
        }

        public async Task ProcessFilesAsync()
        {
            if (this.concurrentFileQueue == null)
            {
                return;
            }

            // TODO -- Can we do parallel processing of these items?
            while (!this.concurrentFileQueue.IsEmpty)
            {
                FileInfo currentFile;
                if (this.concurrentFileQueue.TryDequeue(out currentFile))
                {
                    await FileHandler.ProcessFile(currentFile, this.FileCountsByWordCount);
                }
                else
                {
                    await Task.Run(() => Thread.Sleep(100)); 
                }
            }
        }
    }
}
