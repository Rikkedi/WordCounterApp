using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using WordCountGenerator;

namespace WordCount
{
    class Program
    {
        private const int NumArgs = 1;

        static void Main(string[] args)
        {
            if (args.Length != Program.NumArgs)
            {
                Program.PrintUsage();
                return;
            }

            WordCountDiscoverer discoverer;

            try
            {
                discoverer = new WordCountDiscoverer(args[0]);
            }
            catch (Exception)
            {
                Console.WriteLine(String.Format("Invalid input {0}", args[0]));
                Program.PrintUsage();
                return;
            }

            ConcurrentQueue<FileInfo> fileQueue = (ConcurrentQueue<FileInfo>)discoverer.DiscoverFiles();
            discoverer.ProcessFilesAsync(fileQueue).Wait();
            Program.PrintResults(discoverer.GetCountOfWordsByWordsWithCount());
        }

        private static void PrintUsage()
        {
            Console.WriteLine("WordCount Usage:");
            Console.WriteLine("Application accepts a single argument which is a folder path for the root directory to process");
            Console.WriteLine("  wordcount.exe \"C:\\users\\me\\Documents\"");
        }

        private static void PrintResults(IEnumerable<KeyValuePair<long, int>> results)
        {
            Console.WriteLine("\tWord Count Bucket\t\tNumber Words in Bucket");
            Console.WriteLine("\t-----------------\t\t----------------------");
            foreach (KeyValuePair<long, int> wordCountOccurrencePair in results)
            {
                Console.WriteLine(
                    String.Format(
                        "\t\t{0}\t\t\t\t{1}", 
                        wordCountOccurrencePair.Key, 
                        wordCountOccurrencePair.Value));
            }
        }
    }
}
