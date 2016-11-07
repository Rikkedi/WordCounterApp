using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WordCountGenerator.Tests
{
    [TestClass()]
    public class ArchiveFileHandlerTests
    {
        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\BerkeleyFiles.zip")]
        public async Task GetWordCountTest_Basic1()
        {
            string inputFile = @".\BerkeleyFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            List<int> counts = (List<int>) await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(2, counts.Count);
            Assert.IsTrue(counts.Contains(36600));
            Assert.IsTrue(counts.Contains(36532));
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\LockeFiles.zip")]
        public async Task GetWordCountTest_Basic2()
        {
            string inputFile = @".\LockeFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            List<int> counts = (List<int>)await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(2, counts.Count);
            Assert.IsTrue(counts.Contains(140548));
            Assert.IsTrue(counts.Contains(117320));
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task GetWordCountTest_ArchiveWithSubdirs()
        {
            string inputFile = @".\NestedFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            List<int> counts = (List<int>)await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(6, counts.Count);
            Assert.IsTrue(counts.Contains(36600));
            Assert.IsTrue(counts.Contains(36532));

            Assert.IsTrue(counts.Contains(193392));
            Assert.IsTrue(counts.Contains(183062));

            Assert.IsTrue(counts.Contains(140548));
            Assert.IsTrue(counts.Contains(117320));
        }
    }
}