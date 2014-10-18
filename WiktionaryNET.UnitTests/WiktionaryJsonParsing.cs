using System;
using System.IO;
using System.Linq;
using NUnit;
using NUnit.Framework;
using Moq;
using Moq.Linq;

namespace WiktionaryNET.UnitTests
{
    /// <summary>
    /// Tests the correct parsing of the Json data retrieved from Wiktionary.
    /// Use local Json files retrieved from Wiktionary for the tests.
    /// Because every language has different format headers, we must test 
    /// for all languages supported by the WiktionaryNet library.
    /// </summary>

    [TestFixture]
    public class WiktionaryJsonParsing
    {
        Mock<IJsonQuery> mock = new Mock<IJsonQuery>();

        public WiktionaryJsonParsing()
        {
            mock.Setup(m => m.Download(It.IsAny<string>(), It.Is<string>(str => str == "en")))
                .Returns((string word, string language) => File.ReadAllText("../../en_house.json"));

            mock.Setup(m => m.Download(It.IsAny<string>(), It.Is<string>(str => str == "de")))
                .Returns((string word, string language) => File.ReadAllText("../../de_Haus.json"));

            mock.Setup(m => m.Download(It.IsAny<string>(), It.Is<string>(str => str == "ro")))
                .Returns((string word, string language) => File.ReadAllText("../../ro_casa.json"));

            mock.Setup(m => m.Download(It.Is<string>(str => str == "non_existent_word"), It.IsAny<string>()))
                .Returns((string word, string language) => File.ReadAllText("../../word_not_found.json"));
        }
        
        [Test]
        public void Correct_Word_Definition()
        {
            var enWord = Wiktionary.Define("house", "en", mock.Object);
            var deWord = Wiktionary.Define("Haus", "de", mock.Object);
            var roWord = Wiktionary.Define("casa", "ro", mock.Object);

            Assert.IsTrue(enWord.Definition.ElementAt(0).Contains("within a structure or container"));
            Assert.IsTrue(deWord.Definition.ElementAt(0).Contains("die Gemeinschaft der Menschen, die unter"));
            Assert.IsTrue(roWord.Definition.ElementAt(0).Contains("a anula"));
        }

        [Test]
        public void Word_Not_Found_Return_Empty_Object()
        {
            var noWord = Wiktionary.Define("non_existent_word", "en", mock.Object);

            Assert.IsNotNull(noWord);
            Assert.IsNotNull(noWord.Definition);
            Assert.IsNotNull(noWord.AlternativeForms);
            Assert.IsNotNull(noWord.Etymology);
            Assert.IsNotNull(noWord.Translation);
            Assert.IsNotNull(noWord.WordType);
        }
    }
}
