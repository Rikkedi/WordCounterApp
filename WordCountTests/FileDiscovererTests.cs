using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountGenerator;

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
            long totalWordCount =
                FileWordCounts.FiveWordFile +
                (2 * FileWordCounts.FiveHundredWordFile) +
                FileWordCounts.PrinciplesofHumanKnowledge +
                FileWordCounts.ThreeDialogues +
                FileWordCounts.CritiqueofPureReason +
                FileWordCounts.Theodicy +
                FileWordCounts.EssayConcerningHumaneUnderstandingVol1 +
                FileWordCounts.EssayConcerningHumaneUnderstandingVol2;

            FileDiscoverer fd = new FileDiscoverer(FileDiscovererTests.FlatFolderTextFilesOnly);
            fd.DiscoverFiles();
            await fd.ProcessFilesAsync();

            Assert.AreEqual(totalWordCount, fd.FileCountsByWordCount.Values.Sum());
            Assert.AreEqual(7, fd.FileCountsByWordCount["Lorem"]);
            Assert.AreEqual(2, fd.FileCountsByWordCount["lorem"]);
        }

        [TestMethod()]
        public async Task DiscoverFilesToProcess_FlatFolderArchivesAndTextTest()
        {
            long totalWordCount =
                FileWordCounts.FiveWordFile +
                (2 * FileWordCounts.FiveHundredWordFile) +
                (3 * FileWordCounts.PrinciplesofHumanKnowledge) +
                (3 * FileWordCounts.ThreeDialogues) +
                (2 * FileWordCounts.CritiqueofPureReason) +
                (2 * FileWordCounts.Theodicy) +
                (3 * FileWordCounts.EssayConcerningHumaneUnderstandingVol1) +
                (3 * FileWordCounts.EssayConcerningHumaneUnderstandingVol2);

            FileDiscoverer fd = new FileDiscoverer(FileDiscovererTests.FlatFolderTextAndArchives);
            fd.DiscoverFiles();
            await fd.ProcessFilesAsync();

            Assert.AreEqual(totalWordCount, fd.FileCountsByWordCount.Values.Sum());
            Assert.AreEqual(7, fd.FileCountsByWordCount["Lorem"]);
            Assert.AreEqual(2, fd.FileCountsByWordCount["lorem"]);
            Assert.AreEqual(3, fd.FileCountsByWordCount["ZAHAB?"]);
        }

        [TestMethod()]
        public async Task DiscoverFilesToProcess_FlatFolderNestedArchivesTest()
        {
            long totalWordCount =
                (4 * FileWordCounts.PrinciplesofHumanKnowledge) +
                (4 * FileWordCounts.ThreeDialogues) +
                (1 * FileWordCounts.CritiqueofPureReason) +
                (1 * FileWordCounts.Theodicy) +
                (4 * FileWordCounts.EssayConcerningHumaneUnderstandingVol1) +
                (4 * FileWordCounts.EssayConcerningHumaneUnderstandingVol2);

            FileDiscoverer fd = new FileDiscoverer(FileDiscovererTests.FlatFolderNestedArchives);
            fd.DiscoverFiles();
            await fd.ProcessFilesAsync();

            Assert.AreEqual(totalWordCount, fd.FileCountsByWordCount.Values.Sum());
            Assert.AreEqual(4, fd.FileCountsByWordCount["PRODUCETH."]);
            Assert.AreEqual(4, fd.FileCountsByWordCount["ZAHAB?"]);
        }
    }
}