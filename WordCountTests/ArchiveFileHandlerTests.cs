using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Dictionary<string, long> counts = await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(
                FileWordCounts.PrinciplesofHumanKnowledge + FileWordCounts.ThreeDialogues, 
                counts.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\LockeFiles.zip")]
        public async Task GetWordCountTest_Basic2()
        {
            string inputFile = @".\LockeFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> counts = await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(
                FileWordCounts.EssayConcerningHumaneUnderstandingVol1 + FileWordCounts.EssayConcerningHumaneUnderstandingVol2,
                counts.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task GetWordCountTest_ArchiveWithSubdirs()
        {
            string inputFile = @".\NestedFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> counts = await ArchiveFileHandler.GetWordCount(fi);

            long allFilesWordCount = 
                FileWordCounts.PrinciplesofHumanKnowledge + 
                FileWordCounts.ThreeDialogues + 
                FileWordCounts.CritiqueofPureReason + 
                FileWordCounts.Theodicy + 
                FileWordCounts.EssayConcerningHumaneUnderstandingVol1 + 
                FileWordCounts.EssayConcerningHumaneUnderstandingVol2;

            Assert.AreEqual(allFilesWordCount, counts.Values.Sum());
        }
    }
}