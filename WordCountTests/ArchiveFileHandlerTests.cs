using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountGenerator.Handlers;

namespace WordCountTests.ArchiveFileHandlerTests
{
    [TestClass]
    public class ArchiveFileHandlerTests
    {
        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\BerkeleyFiles.zip")]
        public async Task GetWordCountTest_Basic1()
        {
            string inputFile = @".\BerkeleyFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            List<long> counts = (List<long>) await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(2, counts.Count);
            Assert.IsTrue(counts.Contains(FileWordCounts.PrinciplesofHumanKnowledge));
            Assert.IsTrue(counts.Contains(FileWordCounts.ThreeDialogues));
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\LockeFiles.zip")]
        public async Task GetWordCountTest_Basic2()
        {
            string inputFile = @".\LockeFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            List<long> counts = (List<long>)await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(2, counts.Count);
            Assert.IsTrue(counts.Contains(153053));
            Assert.IsTrue(counts.Contains(FileWordCounts.EssayConcerningHumaneUnderstandingVol2));
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task GetWordCountTest_ArchiveWithSubdirs()
        {
            string inputFile = @".\NestedFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            List<long> counts = (List<long>)await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(6, counts.Count);
            Assert.IsTrue(counts.Contains(FileWordCounts.PrinciplesofHumanKnowledge));
            Assert.IsTrue(counts.Contains(FileWordCounts.ThreeDialogues));

            Assert.IsTrue(counts.Contains(FileWordCounts.CritiqueofPureReason));
            Assert.IsTrue(counts.Contains(FileWordCounts.Theodicy));

            Assert.IsTrue(counts.Contains(FileWordCounts.EssayConcerningHumaneUnderstandingVol1));
            Assert.IsTrue(counts.Contains(FileWordCounts.EssayConcerningHumaneUnderstandingVol2));
        }
    }
}