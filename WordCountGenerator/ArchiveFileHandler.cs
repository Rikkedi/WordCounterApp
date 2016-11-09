using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace WordCountGenerator
{
    public static class ArchiveFileHandler
    {
        public static String ArchiveExtension = @".zip";

        public static bool IsArchiveFile(String file)
        {
            return file.EndsWith(ArchiveFileHandler.ArchiveExtension);
        }

        public static async Task<IEnumerable<long>> GetWordCount(FileInfo fi)
        {
            if (fi == null)
            {
                throw new ArgumentNullException();
            }

            if (!fi.Exists)
            {
                throw new FileNotFoundException(fi.Name);
            }

            if (!ArchiveFileHandler.IsArchiveFile(fi.Name))
            {
                throw new ArgumentException(
                    String.Format(
                        "File {0} is not a known archive type ({1})",
                        fi.Name,
                        ArchiveFileHandler.ArchiveExtension));
            }

            List<long> wordCounts = new List<long>();
            await ProcessArchive(fi, wordCounts);

            return wordCounts;
        }

        private static async Task ProcessArchive(FileInfo fi, List<Int64> wordCounts)
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

                    if (TextFileHandler.IsTextFile(entry.Name))
                    {
                        wordCounts.Add(await TextFileHandler.GetWordCount(entry.Open()));
                    }
                    else if (ArchiveFileHandler.IsArchiveFile(entry.Name))
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
    }
}
