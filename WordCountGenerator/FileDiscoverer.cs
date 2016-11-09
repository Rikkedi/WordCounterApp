using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace WordCountGenerator
{
    public class FileDiscoverer
    {
        private ConcurrentQueue<FileInfo> concurrentFileQueue;
        private DirectoryInfo rootDirectory;

        public ConcurrentDictionary<long, int> FileCountsByWordCount { get; private set; }

        public FileDiscoverer(String path)
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
            catch (PathTooLongException ptle)
            {
                Console.WriteLine("Input path too long.");
                throw ptle;
            }

            this.FileCountsByWordCount = new ConcurrentDictionary<long, int>();
        }

        public async Task DiscoverFilesToProcess()
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

                    await Task.Run(() =>
                    {
                        Parallel.ForEach<FileInfo>(currentDirectory.EnumerateFiles(), (currentFile) =>
                        {
                            this.concurrentFileQueue.Enqueue(currentFile);
                        });

                        foreach (DirectoryInfo subDir in currentDirectory.EnumerateDirectories())
                        {
                            subDirectoriesToExplore.Enqueue(subDir);
                        }
                    });
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

        public async Task ProcessFiles()
        {
            if (this.concurrentFileQueue == null)
            {
                return;
            }

            while (!this.concurrentFileQueue.IsEmpty)
            {
                FileInfo currentFile;
                if (this.concurrentFileQueue.TryDequeue(out currentFile))
                {
                    await FileHandlerTaskFactory.ProcessFile(currentFile, this.FileCountsByWordCount);
                }
                else
                {
                    await Task.Run(() => Thread.Sleep(100)); 
                }
            }
        }
    }
}
