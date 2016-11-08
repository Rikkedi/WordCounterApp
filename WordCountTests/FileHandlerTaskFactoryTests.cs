using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountGenerator;

namespace FileHandlerTaskFactoryTests
{
    [TestClass]
    public class FileHandlerTaskFactoryTests
    {
        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\George Berkeley - Principles of Human Knowledge.txt")]
        public async Task ProcessFileTest_SingleTextFile()
        {
            string inputFile = @".\George Berkeley - Principles of Human Knowledge.txt";

            ConcurrentDictionary<int, int> wordCounts = new ConcurrentDictionary<int, int>();
            FileInfo fi = new FileInfo(inputFile);
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(1, wordCounts[36600]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\George Berkeley - Three Dialogues.txt")]
        public async Task ProcessFileTest_SingleTextFile_Increment()
        {
            string inputFile = @".\George Berkeley - Three Dialogues.txt";

            ConcurrentDictionary<int, int> wordCounts = new ConcurrentDictionary<int, int>();
            wordCounts.TryAdd(36532, 2);

            FileInfo fi = new FileInfo(inputFile);
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(3, wordCounts[36532]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task ProcessFileTest_SingleArchiveFile()
        {
            string inputFile = @".\NestedFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            ConcurrentDictionary<int, int> wordCounts = new ConcurrentDictionary<int, int>();
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(6, wordCounts.Count);
            Assert.AreEqual(1, wordCounts[36600]);
            Assert.AreEqual(1, wordCounts[36532]);

            Assert.AreEqual(1, wordCounts[193392]);
            Assert.AreEqual(1, wordCounts[183062]);

            Assert.AreEqual(1, wordCounts[140548]);
            Assert.AreEqual(1, wordCounts[117320]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task ProcessFileTest_SingleArchiveFile_Increment()
        {
            string inputFile = @".\NestedFiles.zip";
            
            ConcurrentDictionary<int, int> wordCounts = new ConcurrentDictionary<int, int>();
            wordCounts.TryAdd(36600, 5);
            wordCounts.TryAdd(193392, 3);

            FileInfo fi = new FileInfo(inputFile);
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(6, wordCounts.Count);
            Assert.AreEqual(6, wordCounts[36600]);
            Assert.AreEqual(1, wordCounts[36532]);

            Assert.AreEqual(4, wordCounts[193392]);
            Assert.AreEqual(1, wordCounts[183062]);

            Assert.AreEqual(1, wordCounts[140548]);
            Assert.AreEqual(1, wordCounts[117320]);
        }
    }
}