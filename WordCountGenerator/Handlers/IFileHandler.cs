using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WordCountGenerator.Handlers
{
    public interface IFileHandler
    {
        bool IsHandleable(string file);

        Task<Dictionary<string, long>> GetWordCount(FileInfo file);
    }
}
