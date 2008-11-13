using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineArgumentCollectionTests
    {
        [Test]
        public void NameIndexerReturnsCorrectArgument()
        {
            CommandLineArgument argument = new CommandLineArgument(new CommandLineArgumentAttribute("help"), null);
            CommandLineArgumentCollection args = new CommandLineArgumentCollection { argument };

            Assert.AreEqual(argument, args["help"]);
        }

        [Test]
        public void NameIndexerReturnsNullIfCasingOfArgumentIsNotExact()
        {
            CommandLineArgument argument = new CommandLineArgument(new CommandLineArgumentAttribute("help"), null);
            CommandLineArgumentCollection args = new CommandLineArgumentCollection { argument };

            Assert.AreEqual(null, args["Help"]);
        }

        [Test]
        public void NameIndexerReturnsNullIfArgumentIsNotInCollection()
        {
            CommandLineArgumentCollection args = new CommandLineArgumentCollection();

            Assert.AreEqual(null, args["nonExistantArg"]);
        }
    }
}