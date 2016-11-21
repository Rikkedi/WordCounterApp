using System.Threading.Tasks;
using System.IO;
using System.Linq;
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

            ConcurrentDictionary<string, long> wordCounts = new ConcurrentDictionary<string, long>();
            FileInfo fi = new FileInfo(inputFile);
            await FileHandler.ProcessFile(fi, wordCounts);

            Assert.AreEqual(22, wordCounts["wherein"]);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task ProcessFileTest_SingleArchiveFile()
        {
            long totalWordCount =
                (1 * FileWordCounts.PrinciplesofHumanKnowledge) +
                (1 * FileWordCounts.ThreeDialogues) +
                (1 * FileWordCounts.CritiqueofPureReason) +
                (1 * FileWordCounts.Theodicy) +
                (1 * FileWordCounts.EssayConcerningHumaneUnderstandingVol1) +
                (1 * FileWordCounts.EssayConcerningHumaneUnderstandingVol2);

            string inputFile = @".\NestedFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            ConcurrentDictionary<string, long> wordCounts = new ConcurrentDictionary<string, long>();
            await FileHandler.ProcessFile(fi, wordCounts);

            Assert.AreEqual(totalWordCount, wordCounts.Values.Sum());
            Assert.AreEqual(2, wordCounts["DEFINITION"]);
            Assert.AreEqual(4, wordCounts["Definition"]);
            Assert.AreEqual(79, wordCounts["definition"]);
        }
    }
}