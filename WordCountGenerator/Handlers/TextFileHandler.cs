using System;
using System.IO;
using System.Threading.Tasks;

namespace WordCountGenerator.Handlers
{
    public static class TextFileHandler
    {
        public static String TextFileExtension = @".txt";
        private static char[] StringSeparators = { ' ' };

        public static bool IsTextFile(string file)
        {
            return file.EndsWith(TextFileHandler.TextFileExtension);
        }

        public static async Task<long> GetWordCount(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException("File {0} does not exist", file.Name);
            }

            if (!TextFileHandler.IsTextFile(file.Name))
            {
                throw new ArgumentException(
                    String.Format(
                        "File {0} is not a known Text File type ({1})",
                        file.Name,
                        TextFileHandler.TextFileExtension));
            }

            using (FileStream fs = new FileStream(
                file.FullName, 
                FileMode.Open, 
                FileAccess.Read, 
                FileShare.Read, 
                bufferSize: 4096, 
                useAsync: true))
            {
                return await TextFileHandler.GetWordCount(fs);
            }
        }

        public static async Task<long> GetWordCount(Stream textFile)
        {
            if (textFile == null)
            {
                return 0;
            }

            long wordCount = 0;  // Use a long to support large text files

            using (StreamReader file = new StreamReader(textFile))
            {
                while (!file.EndOfStream)
                {
                    string fileBuffer = await file.ReadLineAsync();

                    string[] words = fileBuffer.Trim().Split(TextFileHandler.StringSeparators, StringSplitOptions.RemoveEmptyEntries);

                    wordCount += words.Length;
                }
            }

            return wordCount;
        }
    }
}
