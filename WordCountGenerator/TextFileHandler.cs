using System;
using System.IO;
using System.Threading.Tasks;

namespace WordCountGenerator
{
    public static class TextFileHandler
    {
        public static String TextFileExtension = @".txt";

        public static async Task<int> GetWordCount(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException("File {0} does not exist", file.Name);
            }

            if (file.Extension != TextFileHandler.TextFileExtension)
            {
                throw new ArgumentException(
                    String.Format(
                        "File {0} is not a known archive type ({1})",
                        file.Name,
                        ArchiveFileHandler.ArchiveExtension));
            }

            using (FileStream fs = file.OpenRead())
            {
                return await TextFileHandler.GetWordCount(fs);
            }
        }

        public static async Task<int> GetWordCount(Stream textFile)
        {
            if (textFile == null)
            {
                return 0;
            }

            using (StreamReader file = new StreamReader(textFile))
            {
                string fileBuffer = await file.ReadToEndAsync();

                string[] words = fileBuffer.Split(' ');

                return words.Length;
            }
        }
    }
}
