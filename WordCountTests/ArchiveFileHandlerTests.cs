using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountGenerator;

namespace ArchiveFileHandlerTests
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
            Assert.IsTrue(counts.Contains(39737));
            Assert.IsTrue(counts.Contains(39050));
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
            Assert.IsTrue(counts.Contains(127908));
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\NestedFiles.zip")]
        public async Task GetWordCountTest_ArchiveWithSubdirs()
        {
            string inputFile = @".\NestedFiles.zip";

            FileInfo fi = new FileInfo(inputFile);
            List<long> counts = (List<long>)await ArchiveFileHandler.GetWordCount(fi);

            Assert.AreEqual(6, counts.Count);
            Assert.IsTrue(counts.Contains(39737));
            Assert.IsTrue(counts.Contains(39050));

            Assert.IsTrue(counts.Contains(210484));
            Assert.IsTrue(counts.Contains(192261));

            Assert.IsTrue(counts.Contains(153053));
            Assert.IsTrue(counts.Contains(127908));
        }
    }
}