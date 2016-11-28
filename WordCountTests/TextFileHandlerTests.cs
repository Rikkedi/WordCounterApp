using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountGenerator.Handlers;

namespace WordCountTests.FileStreamHandlerTests
{
    [TestClass]
    public class TextFileHandlerTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task FileDoesNotExistTestAsync()
        {
            string inputFile = @".\SampleInputFiles\George.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);

            Dictionary<string, long> count = await tfh.GetWordCount(fi);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\FiveWords.txt")]
        public async Task TestMethod_FiveWords()
        {
            string inputFile = @".\FiveWords.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            Assert.AreEqual(FileWordCounts.FiveWordFile, count.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\FiveHundredWords_LongLines.txt")]
        public async Task TestMethod_FiveHundredWords_FiveLines()
        {
            string inputFile = @".\FiveHundredWords_LongLines.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            Assert.AreEqual(FileWordCounts.FiveHundredWordFile, count.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\FiveHundredWords_LineBreaks.txt")]
        public async Task TestMethod_FiveHundredWords_LineBreaks()
        {
            string inputFile = @".\FiveHundredWords_LineBreaks.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            Assert.AreEqual(FileWordCounts.FiveHundredWordFile, count.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\George Berkeley - Principles of Human Knowledge.txt")]
        public async Task TestMethod1()
        {
            string inputFile = @".\George Berkeley - Principles of Human Knowledge.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            //Assert.AreEqual(39951, count);  // Word Count based on MS Word
            //Assert.AreEqual(36600, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(FileWordCounts.PrinciplesofHumanKnowledge, count.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\George Berkeley - Three Dialogues.txt")]
        public async Task TestMethod2()
        {
            string inputFile = @".\George Berkeley - Three Dialogues.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            //Assert.AreEqual(39105, count);  // Word Count based on MS Word
            //Assert.AreEqual(36532, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(FileWordCounts.ThreeDialogues, count.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\Immanuel Kant - Critique of Pure Reason.txt")]
        public async Task TestMethod3()
        {
            string inputFile = @".\Immanuel Kant - Critique of Pure Reason.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            //Assert.AreEqual(211731, count);  // Word Count based on MS Word
            //Assert.AreEqual(193392, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(FileWordCounts.CritiqueofPureReason, count.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\Leibniz, Gottfried - Theodicy.txt")]
        public async Task TestMethod4()
        {
            string inputFile = @".\Leibniz, Gottfried - Theodicy.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            //Assert.AreEqual(192328, count);  // Word Count based on MS Word
            //Assert.AreEqual(183062, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(FileWordCounts.Theodicy, count.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\Locke, John - Essay Concerning Humane Understanding, Vol 1.txt")]
        public async Task TestMethod5()
        {
            string inputFile = @".\Locke, John - Essay Concerning Humane Understanding, Vol 1.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            //Assert.AreEqual(153324, count);  // Word Count based on MS Word
            //Assert.AreEqual(140548, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(FileWordCounts.EssayConcerningHumaneUnderstandingVol1, count.Values.Sum());
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\Locke, John - Essay Concerning Humane Understanding, Vol 2.txt")]
        public async Task TestMethod6()
        {
            string inputFile = @".\Locke, John - Essay Concerning Humane Understanding, Vol 2.txt";

            TextFileHandler tfh = new TextFileHandler();
            FileInfo fi = new FileInfo(inputFile);
            Dictionary<string, long> count = await tfh.GetWordCount(fi);

            //Assert.AreEqual(127959, count);  // Word Count based on MS Word
            //Assert.AreEqual(117320, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(FileWordCounts.EssayConcerningHumaneUnderstandingVol2, count.Values.Sum());
        }
    }
}
