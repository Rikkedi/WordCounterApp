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

        public static async Task<IEnumerable<int>> GetWordCount(FileInfo fi)
        {
            if (fi == null)
            {
                throw new ArgumentNullException();
            }

            if (!fi.Exists)
            {
                throw new FileNotFoundException(fi.Name);
            }
            
            if (fi.Extension != ArchiveFileHandler.ArchiveExtension)
            {
                throw new ArgumentException(
                    String.Format(
                        "File {0} is not a known archive type ({1})", 
                        fi.Name, 
                        ArchiveFileHandler.ArchiveExtension));
            }

            List<int> wordCounts = new List<int>();

            using (ZipArchive archive = ZipFile.OpenRead(fi.FullName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name.EndsWith(TextFileHandler.TextFileExtension))
                    {
                        wordCounts.Add(await TextFileHandler.GetWordCount(entry.Open()));
                    }
                }
            }

            return wordCounts;
        }
    }
}
