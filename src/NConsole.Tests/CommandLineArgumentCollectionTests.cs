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

            Assert.IsNull(args["Help"]);
        }

        [Test]
        public void NameIndexerReturnsNullIfArgumentIsNotInCollection()
        {
            CommandLineArgumentCollection args = new CommandLineArgumentCollection();

            Assert.IsNull(args["nonExistantArg"]);
        }

        [Test]
        public void ContainsReturnsTrueIfArgumentIsInCollection()
        {
            CommandLineArgumentCollection args = new CommandLineArgumentCollection();

            args.Add(new CommandLineArgument(new CommandLineArgumentAttribute("help"), null));

            Assert.IsTrue(args.Contains("help"));
        }

        [Test]
        public void ContainsReturnsFalseIfArgumentIsNotInCollection()
        {
            CommandLineArgumentCollection args = new CommandLineArgumentCollection();

            Assert.IsFalse(args.Contains("nonExistantArg"));
        }
    }
}