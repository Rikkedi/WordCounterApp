using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCountGenerator;

namespace FileStreamHandlerTests
{
    [TestClass]
    public class FileStreamHandlerTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task FileDoesNotExistTestAsync()
        {
            string inputFile = @".\SampleInputFiles\George.txt";

            FileInfo fi = new FileInfo(inputFile);

            long count = await TextFileHandler.GetWordCount(fi);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\FiveWords.txt")]
        public async Task TestMethod_FiveWords()
        {
            string inputFile = @".\FiveWords.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            Assert.AreEqual(5, count);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\FiveHundredWords_LongLines.txt")]
        public async Task TestMethod_FiveHundredWords_FiveLines()
        {
            string inputFile = @".\FiveHundredWords_LongLines.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            Assert.AreEqual(500, count);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\FiveHundredWords_LineBreaks.txt")]
        public async Task TestMethod_FiveHundredWords_LineBreaks()
        {
            string inputFile = @".\FiveHundredWords_LineBreaks.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            Assert.AreEqual(500, count);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\George Berkeley - Principles of Human Knowledge.txt")]
        public async Task TestMethod1()
        {
            string inputFile = @".\George Berkeley - Principles of Human Knowledge.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            //Assert.AreEqual(39951, count);  // Word Count based on MS Word
            //Assert.AreEqual(36600, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(39737, count);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\George Berkeley - Three Dialogues.txt")]
        public async Task TestMethod2()
        {
            string inputFile = @".\George Berkeley - Three Dialogues.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            //Assert.AreEqual(39105, count);  // Word Count based on MS Word
            //Assert.AreEqual(36532, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(39050, count);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\Immanuel Kant - Critique of Pure Reason.txt")]
        public async Task TestMethod3()
        {
            string inputFile = @".\Immanuel Kant - Critique of Pure Reason.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            //Assert.AreEqual(211731, count);  // Word Count based on MS Word
            //Assert.AreEqual(193392, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(210484, count);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\Leibniz, Gottfried - Theodicy.txt")]
        public async Task TestMethod4()
        {
            string inputFile = @".\Leibniz, Gottfried - Theodicy.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            //Assert.AreEqual(192328, count);  // Word Count based on MS Word
            //Assert.AreEqual(183062, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(192261, count);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\Locke, John - Essay Concerning Humane Understanding, Vol 1.txt")]
        public async Task TestMethod5()
        {
            string inputFile = @".\Locke, John - Essay Concerning Humane Understanding, Vol 1.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            //Assert.AreEqual(153324, count);  // Word Count based on MS Word
            //Assert.AreEqual(140548, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(153053, count);
        }

        [TestMethod]
        [DeploymentItem(@".\SampleInputFiles\Locke, John - Essay Concerning Humane Understanding, Vol 2.txt")]
        public async Task TestMethod6()
        {
            string inputFile = @".\Locke, John - Essay Concerning Humane Understanding, Vol 2.txt";

            FileInfo fi = new FileInfo(inputFile);
            long count = await TextFileHandler.GetWordCount(fi);

            //Assert.AreEqual(127959, count);  // Word Count based on MS Word
            //Assert.AreEqual(117320, count);  // Word Count based on string.Split() over the entire file
            Assert.AreEqual(127908, count);
        }
    }
}
