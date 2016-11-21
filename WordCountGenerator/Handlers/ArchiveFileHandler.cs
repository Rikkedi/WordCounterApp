﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace WordCountGenerator.Handlers
{
    internal static class ArchiveFileHandler
    {
        public static String ArchiveExtension = @".zip";

        public static bool IsHandleable(String file)
        {
            return file.EndsWith(ArchiveFileHandler.ArchiveExtension);
        }

        public static async Task<Dictionary<string, long>> GetWordCount(FileInfo fi)
        {
            if (fi == null)
            {
                throw new ArgumentNullException();
            }

            if (!fi.Exists)
            {
                throw new FileNotFoundException(fi.Name);
            }

            if (!ArchiveFileHandler.IsHandleable(fi.Name))
            {
                throw new ArgumentException(
                    String.Format(
                        "File {0} is not a known archive type ({1})",
                        fi.Name,
                        ArchiveFileHandler.ArchiveExtension));
            }

            Dictionary<string, long> wordCounts = new Dictionary<string, long>();
            await ProcessArchive(fi, wordCounts);

            return wordCounts;
        }

        private static async Task ProcessArchive(FileInfo fi, Dictionary<string, long> wordCounts)
        {
            if (wordCounts == null)
            {
                throw new ArgumentNullException("wordCounts");
            }

            using (ZipArchive archive = ZipFile.OpenRead(fi.FullName))
            {
                Queue<ZipArchiveEntry> filesToProcess = new Queue<ZipArchiveEntry>(archive.Entries);

                while (filesToProcess.Count > 0)
                {
                    ZipArchiveEntry entry = filesToProcess.Dequeue();

                    if (TextFileHandler.IsHandleable(entry.Name))
                    {
                        await ArchiveFileHandler.MergeWordCounts(
                            wordCounts, 
                            await TextFileHandler.GetWordCount(entry.Open()));
                    }
                    else if (ArchiveFileHandler.IsHandleable(entry.Name))
                    {
                        Stream zipEntry = entry.Open();
                        ZipArchive innerArchive = new ZipArchive(zipEntry, ZipArchiveMode.Read, false);

                        foreach (ZipArchiveEntry innerFile in innerArchive.Entries)
                        { 
                            filesToProcess.Enqueue(innerFile);
                        }
                    }
                }
            }
        }

        private static async Task MergeWordCounts(Dictionary<string, long> aggregateWordCounts, Dictionary<string, long> wordCounts)
        {
            await Task.Run(() =>
            {
                foreach (KeyValuePair<string, long> wordCount in wordCounts)
                {
                    if (aggregateWordCounts.ContainsKey(wordCount.Key))
                    {
                        aggregateWordCounts[wordCount.Key] += wordCount.Value;
                    }
                    else
                    {
                        aggregateWordCounts[wordCount.Key] = wordCount.Value;
                    }
                }
            });
        }
    }
}
