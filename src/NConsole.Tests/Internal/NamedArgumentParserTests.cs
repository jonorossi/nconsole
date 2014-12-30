using System.Collections.Generic;
using NConsole.Internal;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class NamedArgumentParserTests
    {
        private readonly CommandDescriptorBuilder descriptorBuilder = new CommandDescriptorBuilder();
        private readonly NamedArgumentParser parser = new NamedArgumentParser();

        [Test]
        public void SetsBooleanFlagUsingShortName()
        {
            List<string> args = new List<string>(new[] { "-f" });
            SimpleCommand command = new SimpleCommand();
            CommandDescriptor descriptor = descriptorBuilder.BuildDescriptor(typeof(SimpleCommand));

            parser.Apply(args, command, descriptor);

            Assert.That(args, Is.Empty);
            Assert.That(command.BoolFlag);
        }

        [Test]
        public void SetsBooleanFlagUsingLongName()
        {
            List<string> args = new List<string>(new[] { "--flag" });
            SimpleCommand command = new SimpleCommand();
            CommandDescriptor descriptor = descriptorBuilder.BuildDescriptor(typeof(SimpleCommand));

            parser.Apply(args, command, descriptor);

            Assert.That(args, Is.Empty);
            Assert.That(command.BoolFlag);
        }

        private class SimpleCommand : ICommand
        {
            [Argument(ShortName = "f", LongName = "flag")]
            public bool BoolFlag { get; set; }

            public void Execute()
            {
            }
        }
    }
}