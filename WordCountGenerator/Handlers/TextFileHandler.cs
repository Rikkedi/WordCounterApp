using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WordCountGenerator.Handlers
{
    public class TextFileHandler : IFileHandler
    {
        public const string TextFileExtension = @".txt";
        private static char[] StringSeparators = { ' ' };

        public bool IsHandleable(string file)
        {
            return file.EndsWith(TextFileHandler.TextFileExtension);
        }

        public async Task<Dictionary<string, long>> GetWordCount(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException();
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException("File {0} does not exist", file.Name);
            }

            if (!this.IsHandleable(file.Name))
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
                FileShare.Read))
            {
                return await this.GetWordCount(fs);
            }
        }

        internal async Task<Dictionary<string, long>> GetWordCount(Stream textFile)
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
                    // TODO - Asynchronously reading one line at a time not going to be efficient in most common cases, due to the
                    //        added costs of asynchronous operations.  Should move to buffer based reading.
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
