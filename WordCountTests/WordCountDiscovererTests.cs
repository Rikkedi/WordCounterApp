using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
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
            WordCountDiscoverer fd = new WordCountDiscoverer(String.Empty);
        }

        [TestMethod()]
        public void FileDiscovererTest()
        {
            WordCountDiscoverer fd = new WordCountDiscoverer(".");
            Assert.IsNotNull(fd.AggregateWordOccurrenceCount);
        }

        [TestMethod()]
        public async Task DiscoverAndProcess_FlatFolderTextOnlyTest()
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

            WordCountDiscoverer fd = new WordCountDiscoverer(FileDiscovererTests.FlatFolderTextFilesOnly);
            await fd.ProcessFilesAsync((ConcurrentQueue<FileInfo>) fd.DiscoverFiles());

            Assert.AreEqual(totalWordCount, fd.AggregateWordOccurrenceCount.Values.Sum());
            Assert.AreEqual(7, fd.AggregateWordOccurrenceCount["Lorem"]);
            Assert.AreEqual(2, fd.AggregateWordOccurrenceCount["lorem"]);
        }

        [TestMethod()]
        public async Task DiscoverAndProcess_FlatFolderArchivesAndTextTest()
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

            WordCountDiscoverer fd = new WordCountDiscoverer(FileDiscovererTests.FlatFolderTextAndArchives);
            await fd.ProcessFilesAsync((ConcurrentQueue<FileInfo>)fd.DiscoverFiles());

            Assert.AreEqual(totalWordCount, fd.AggregateWordOccurrenceCount.Values.Sum());
            Assert.AreEqual(7, fd.AggregateWordOccurrenceCount["Lorem"]);
            Assert.AreEqual(2, fd.AggregateWordOccurrenceCount["lorem"]);
            Assert.AreEqual(3, fd.AggregateWordOccurrenceCount["ZAHAB?"]);
        }

        [TestMethod()]
        public async Task DiscoverAndProcess_FlatFolderNestedArchivesTest()
        {
            long totalWordCount =
                (4 * FileWordCounts.PrinciplesofHumanKnowledge) +
                (4 * FileWordCounts.ThreeDialogues) +
                (1 * FileWordCounts.CritiqueofPureReason) +
                (1 * FileWordCounts.Theodicy) +
                (4 * FileWordCounts.EssayConcerningHumaneUnderstandingVol1) +
                (4 * FileWordCounts.EssayConcerningHumaneUnderstandingVol2);

            WordCountDiscoverer fd = new WordCountDiscoverer(FileDiscovererTests.FlatFolderNestedArchives);
            await fd.ProcessFilesAsync((ConcurrentQueue<FileInfo>)fd.DiscoverFiles());

            Assert.AreEqual(totalWordCount, fd.AggregateWordOccurrenceCount.Values.Sum());
            Assert.AreEqual(4, fd.AggregateWordOccurrenceCount["PRODUCETH."]);
            Assert.AreEqual(4, fd.AggregateWordOccurrenceCount["ZAHAB?"]);
        }

        [TestMethod()]
        public async Task GetCountOfWordsByWordsWithCount_FlatFolderTextOnlyTest()
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

            WordCountDiscoverer fd = new WordCountDiscoverer(FileDiscovererTests.FlatFolderTextFilesOnly);
            await fd.ProcessFilesAsync((ConcurrentQueue<FileInfo>)fd.DiscoverFiles());
            IEnumerable<KeyValuePair<long, int>> countOfWordsByWordsWithCount = fd.GetCountOfWordsByWordsWithCount();

            double wordCountsFromAggregate = countOfWordsByWordsWithCount.Sum(e => (e.Key * e.Value));

            Assert.AreEqual(totalWordCount, wordCountsFromAggregate);
        }

        [TestMethod()]
        public async Task GetCountOfWordsByWordsWithCount_FlatFolderArchivesAndTextTest()
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

            WordCountDiscoverer fd = new WordCountDiscoverer(FileDiscovererTests.FlatFolderTextAndArchives);
            await fd.ProcessFilesAsync((ConcurrentQueue<FileInfo>)fd.DiscoverFiles());
            IEnumerable<KeyValuePair<long, int>> countOfWordsByWordsWithCount = fd.GetCountOfWordsByWordsWithCount();

            double wordCountsFromAggregate = countOfWordsByWordsWithCount.Sum(e => (e.Key * e.Value));

            Assert.AreEqual(totalWordCount, wordCountsFromAggregate);
        }

        [TestMethod()]
        public async Task GetCountOfWordsByWordsWithCount_FlatFolderNestedArchivesTest()
        {
            long totalWordCount =
                (4 * FileWordCounts.PrinciplesofHumanKnowledge) +
                (4 * FileWordCounts.ThreeDialogues) +
                (1 * FileWordCounts.CritiqueofPureReason) +
                (1 * FileWordCounts.Theodicy) +
                (4 * FileWordCounts.EssayConcerningHumaneUnderstandingVol1) +
                (4 * FileWordCounts.EssayConcerningHumaneUnderstandingVol2);

            WordCountDiscoverer fd = new WordCountDiscoverer(FileDiscovererTests.FlatFolderNestedArchives);
            await fd.ProcessFilesAsync((ConcurrentQueue<FileInfo>)fd.DiscoverFiles());
            IEnumerable<KeyValuePair<long, int>> countOfWordsByWordsWithCount = fd.GetCountOfWordsByWordsWithCount();

            double wordCountsFromAggregate = countOfWordsByWordsWithCount.Sum(e => (e.Key * e.Value));

            Assert.AreEqual(totalWordCount, wordCountsFromAggregate);
        }
    }
}