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

            ConcurrentDictionary<long, int> wordCounts = new ConcurrentDictionary<long, int>();
            FileInfo fi = new FileInfo(inputFile);
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(1, wordCounts[39737]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\George Berkeley - Three Dialogues.txt")]
        public async Task ProcessFileTest_SingleTextFile_Increment()
        {
            string inputFile = @".\George Berkeley - Three Dialogues.txt";

            ConcurrentDictionary<long, int> wordCounts = new ConcurrentDictionary<long, int>();
            wordCounts.TryAdd(39050, 2);

            FileInfo fi = new FileInfo(inputFile);
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(3, wordCounts[39050]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task ProcessFileTest_SingleArchiveFile()
        {
            string inputFile = @".\NestedFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            ConcurrentDictionary<long, int> wordCounts = new ConcurrentDictionary<long, int>();
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(6, wordCounts.Count);
            Assert.AreEqual(1, wordCounts[39737]);
            Assert.AreEqual(1, wordCounts[39050]);

            Assert.AreEqual(1, wordCounts[210484]);
            Assert.AreEqual(1, wordCounts[192261]);

            Assert.AreEqual(1, wordCounts[153053]);
            Assert.AreEqual(1, wordCounts[127908]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task ProcessFileTest_SingleArchiveFile_Increment()
        {
            string inputFile = @".\NestedFiles.zip";
            
            ConcurrentDictionary<long, int> wordCounts = new ConcurrentDictionary<long, int>();
            wordCounts.TryAdd(39737, 5);
            wordCounts.TryAdd(210484, 3);

            FileInfo fi = new FileInfo(inputFile);
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(6, wordCounts.Count);
            Assert.AreEqual(6, wordCounts[39737]);
            Assert.AreEqual(1, wordCounts[39050]);

            Assert.AreEqual(4, wordCounts[210484]);
            Assert.AreEqual(1, wordCounts[192261]);

            Assert.AreEqual(1, wordCounts[153053]);
            Assert.AreEqual(1, wordCounts[127908]);
        }
    }
}