using System.Collections.Generic;
using NConsole.Internal;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class PositionalArgumentParserTests
    {
        private readonly CommandDescriptorBuilder descriptorBuilder = new CommandDescriptorBuilder();
        private readonly PositionalArgumentParser parser = new PositionalArgumentParser();

        [Test]
        public void SetsSinglePositionalArguments()
        {
            List<string> args = new List<string>(new[] { "http://example.org" });
            SimpleCommand command = new SimpleCommand();
            CommandDescriptor descriptor = descriptorBuilder.BuildDescriptor(typeof(SimpleCommand));

            parser.Apply(args, command, descriptor);

            Assert.That(args, Is.Empty);
            Assert.That(command.Remote, Is.EqualTo("http://example.org"));
            Assert.That(command.Local, Is.Null);
        }

        [Test]
        public void SetsMultiplePositionalArguments()
        {
            List<string> args = new List<string>(new[] { "http://example.org", @"C:\temp" });
            SimpleCommand command = new SimpleCommand();
            CommandDescriptor descriptor = descriptorBuilder.BuildDescriptor(typeof(SimpleCommand));

            parser.Apply(args, command, descriptor);

            Assert.That(args, Is.Empty);
            Assert.That(command.Remote, Is.EqualTo("http://example.org"));
            Assert.That(command.Local, Is.EqualTo(@"C:\temp"));
        }

        private class SimpleCommand : ICommand
        {
            [Argument(Position = 0)]
            public string Remote { get; set; }

            [Argument(Position = 1)]
            public string Local { get; set; }

            public void Execute()
            {
            }
        }
    }
}