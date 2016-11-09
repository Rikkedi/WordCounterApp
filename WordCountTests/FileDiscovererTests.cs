using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCountTests.FileDiscovererTests
{
    [TestClass()]
    public class FileDiscovererTests
    {
        private const string FlatFolderTextFilesOnly = @"H:\Users\Candlejack\Documents\visual studio 15\Projects\WordCountGenerator\FolderTestInputs\Flat_TextFilesOnly";
        private const string FlatFolderTextAndArchives = @"H:\Users\Candlejack\Documents\visual studio 15\Projects\WordCountGenerator\FolderTestInputs\Flat_ThreeArchives";
        private const string FlatFolderNestedArchives = @"H:\Users\Candlejack\Documents\visual studio 15\Projects\WordCountGenerator\FolderTestInputs\Flat_ArchivesOnly";

        [TestMethod()]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void FileDiscoverer_EmptyInputTest()
        {
            FileDiscoverer fd = new FileDiscoverer(String.Empty);
        }

        [TestMethod()]
        public void FileDiscovererTest()
        {
            FileDiscoverer fd = new FileDiscoverer(".");
            Assert.IsNotNull(fd.FileCountsByWordCount);
        }

        [TestMethod()]
        public async Task DiscoverFilesToProcess_FlatFolderTextOnlyTest()
        {
            FileDiscoverer fd = new FileDiscoverer(FileDiscovererTests.FlatFolderTextFilesOnly);
            await fd.DiscoverFilesToProcess();
            await fd.ProcessFiles();

            Assert.AreEqual(8, fd.FileCountsByWordCount.Count);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.FiveWordFile));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.FiveWordFile]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.FiveHundredWordFile));
            Assert.AreEqual(2, fd.FileCountsByWordCount[FileWordCounts.FiveHundredWordFile]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.PrinciplesofHumanKnowledge));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.PrinciplesofHumanKnowledge]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.ThreeDialogues));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.ThreeDialogues]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.CritiqueofPureReason));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.CritiqueofPureReason]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.Theodicy));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.Theodicy]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.EssayConcerningHumaneUnderstandingVol1));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.EssayConcerningHumaneUnderstandingVol1]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.EssayConcerningHumaneUnderstandingVol2));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.EssayConcerningHumaneUnderstandingVol2]);
        }

        [TestMethod()]
        public async Task DiscoverFilesToProcess_FlatFolderArchivesAndTextTest()
        {
            FileDiscoverer fd = new FileDiscoverer(FileDiscovererTests.FlatFolderTextAndArchives);
            await fd.DiscoverFilesToProcess();
            await fd.ProcessFiles();

            Assert.AreEqual(8, fd.FileCountsByWordCount.Count);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.FiveWordFile));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.FiveWordFile]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.FiveHundredWordFile));
            Assert.AreEqual(2, fd.FileCountsByWordCount[FileWordCounts.FiveHundredWordFile]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.PrinciplesofHumanKnowledge));
            Assert.AreEqual(3, fd.FileCountsByWordCount[FileWordCounts.PrinciplesofHumanKnowledge]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.ThreeDialogues));
            Assert.AreEqual(3, fd.FileCountsByWordCount[FileWordCounts.ThreeDialogues]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.CritiqueofPureReason));
            Assert.AreEqual(2, fd.FileCountsByWordCount[FileWordCounts.CritiqueofPureReason]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.Theodicy));
            Assert.AreEqual(2, fd.FileCountsByWordCount[FileWordCounts.Theodicy]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.EssayConcerningHumaneUnderstandingVol1));
            Assert.AreEqual(3, fd.FileCountsByWordCount[FileWordCounts.EssayConcerningHumaneUnderstandingVol1]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.EssayConcerningHumaneUnderstandingVol2));
            Assert.AreEqual(3, fd.FileCountsByWordCount[FileWordCounts.EssayConcerningHumaneUnderstandingVol2]);
        }

        [TestMethod()]
        public async Task DiscoverFilesToProcess_FlatFolderNestedArchivesTest()
        {
            FileDiscoverer fd = new FileDiscoverer(FileDiscovererTests.FlatFolderNestedArchives);
            await fd.DiscoverFilesToProcess();
            await fd.ProcessFiles();

            Assert.AreEqual(6, fd.FileCountsByWordCount.Count);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.PrinciplesofHumanKnowledge));
            Assert.AreEqual(4, fd.FileCountsByWordCount[FileWordCounts.PrinciplesofHumanKnowledge]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.ThreeDialogues));
            Assert.AreEqual(4, fd.FileCountsByWordCount[FileWordCounts.ThreeDialogues]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.CritiqueofPureReason));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.CritiqueofPureReason]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.Theodicy));
            Assert.AreEqual(1, fd.FileCountsByWordCount[FileWordCounts.Theodicy]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.EssayConcerningHumaneUnderstandingVol1));
            Assert.AreEqual(4, fd.FileCountsByWordCount[FileWordCounts.EssayConcerningHumaneUnderstandingVol1]);

            Assert.IsTrue(fd.FileCountsByWordCount.ContainsKey(FileWordCounts.EssayConcerningHumaneUnderstandingVol2));
            Assert.AreEqual(4, fd.FileCountsByWordCount[FileWordCounts.EssayConcerningHumaneUnderstandingVol2]);
        }
    }
}