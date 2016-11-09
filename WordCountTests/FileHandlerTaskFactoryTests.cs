using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountGenerator;

namespace WordCountTests.FileHandlerTaskFactoryTests
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

            Assert.AreEqual(1, wordCounts[FileWordCounts.PrinciplesofHumanKnowledge]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\George Berkeley - Three Dialogues.txt")]
        public async Task ProcessFileTest_SingleTextFile_Increment()
        {
            string inputFile = @".\George Berkeley - Three Dialogues.txt";

            ConcurrentDictionary<long, int> wordCounts = new ConcurrentDictionary<long, int>();
            wordCounts.TryAdd(FileWordCounts.ThreeDialogues, 2);

            FileInfo fi = new FileInfo(inputFile);
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(3, wordCounts[FileWordCounts.ThreeDialogues]);
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
            Assert.AreEqual(1, wordCounts[FileWordCounts.PrinciplesofHumanKnowledge]);
            Assert.AreEqual(1, wordCounts[FileWordCounts.ThreeDialogues]);

            Assert.AreEqual(1, wordCounts[FileWordCounts.CritiqueofPureReason]);
            Assert.AreEqual(1, wordCounts[FileWordCounts.Theodicy]);

            Assert.AreEqual(1, wordCounts[FileWordCounts.EssayConcerningHumaneUnderstandingVol1]);
            Assert.AreEqual(1, wordCounts[FileWordCounts.EssayConcerningHumaneUnderstandingVol2]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task ProcessFileTest_SingleArchiveFile_Increment()
        {
            string inputFile = @".\NestedFiles.zip";
            
            ConcurrentDictionary<long, int> wordCounts = new ConcurrentDictionary<long, int>();
            wordCounts.TryAdd(FileWordCounts.PrinciplesofHumanKnowledge, 5);
            wordCounts.TryAdd(FileWordCounts.CritiqueofPureReason, 3);

            FileInfo fi = new FileInfo(inputFile);
            await FileHandlerTaskFactory.ProcessFile(fi, wordCounts);

            Assert.AreEqual(6, wordCounts.Count);
            Assert.AreEqual(6, wordCounts[FileWordCounts.PrinciplesofHumanKnowledge]);
            Assert.AreEqual(1, wordCounts[FileWordCounts.ThreeDialogues]);

            Assert.AreEqual(4, wordCounts[FileWordCounts.CritiqueofPureReason]);
            Assert.AreEqual(1, wordCounts[FileWordCounts.Theodicy]);

            Assert.AreEqual(1, wordCounts[FileWordCounts.EssayConcerningHumaneUnderstandingVol1]);
            Assert.AreEqual(1, wordCounts[FileWordCounts.EssayConcerningHumaneUnderstandingVol2]);
        }
    }
}