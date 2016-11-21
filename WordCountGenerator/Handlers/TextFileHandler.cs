using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WordCountGenerator.Handlers
{
    internal static class TextFileHandler
    {
        public static String TextFileExtension = @".txt";
        private static char[] StringSeparators = { ' ' };

        public static bool IsHandleable(string file)
        {
            return file.EndsWith(TextFileHandler.TextFileExtension);
        }

        public static async Task<Dictionary<string, long>> GetWordCount(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException("File {0} does not exist", file.Name);
            }

            if (!TextFileHandler.IsHandleable(file.Name))
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

        internal static async Task<Dictionary<string, long>> GetWordCount(Stream textFile)
        {
            if (textFile == null)
            {
                return null;
            }

            Dictionary<string, long> wordCounts = new Dictionary<string, long>(StringComparer.Ordinal);

            using (StreamReader file = new StreamReader(textFile))
            {
                while (!file.EndOfStream)
                {
                    string fileBuffer = await file.ReadLineAsync();

                    string[] words = fileBuffer.Trim().Split(TextFileHandler.StringSeparators, StringSplitOptions.RemoveEmptyEntries);

                    await Task.Run(() =>
                    {
                        foreach (string word in words)
                        {
                            if (wordCounts.ContainsKey(word))
                            {
                                wordCounts[word]++;
                            }
                            else
                            {
                                wordCounts[word] = 1;
                            }
                        }
                    });
                }
            }

            return wordCounts;
        }
    }
}
