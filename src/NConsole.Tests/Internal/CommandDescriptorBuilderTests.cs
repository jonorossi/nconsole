using System;
using NConsole.Internal;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class CommandDescriptorBuilderTests
    {
        private readonly CommandDescriptorBuilder builder = new CommandDescriptorBuilder();

        [Test]
        public void ThrowsIfCommandDoesNotImplementICommand()
        {
            var ex = Assert.Throws<Exception>(() => builder.BuildDescriptor(typeof(int)));

            Assert.That(ex.Message, Is.EqualTo("Command type 'System.Int32' does not implement ICommand."));
        }

        [Test]
        public void CommandTypeIsSet()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestClass));

            Assert.That(descriptor.CommandType, Is.EqualTo(typeof(TestClass)));
        }

        [Test]
        public void DefaultCommandNameIsTheClassName()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestClass));

            Assert.That(descriptor.Name, Is.EqualTo("testclass"));
        }

        [Test]
        public void DefaultCommandNameIsTheClassNameWithoutCommand()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestCommand));

            Assert.That(descriptor.Name, Is.EqualTo("test"));
        }

        [Test]
        public void CommandsMustHaveAName()
        {
            var ex = Assert.Throws<Exception>(() => builder.BuildDescriptor(typeof(Command)));

            Assert.That(ex.Message, Is.EqualTo("Command type 'NConsole.Tests.Internal.CommandDescriptorBuilderTests+Command' does not provide a command name."));
        }

        [Test]
        public void ArgumentDescriptorsAreCreatedOnlyWhenMarkedWithAttribute()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestArgSimpleCommand));

            Assert.That(descriptor.Arguments.Count, Is.EqualTo(1));
        }

        [Test]
        public void ArgumentDescriptorsHaveCorrectType()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestArgCommand));

            Assert.That(descriptor.Arguments[0].ArgumentType, Is.EqualTo(typeof(string)));
            Assert.That(descriptor.Arguments[1].ArgumentType, Is.EqualTo(typeof(int)));
            Assert.That(descriptor.Arguments[2].ArgumentType, Is.EqualTo(typeof(bool)));
            Assert.That(descriptor.Arguments[2].ArgumentType, Is.EqualTo(typeof(bool)));
        }

        [Test]
        public void ArgumentDescriptorsHaveCorrectPropertyInfo()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestArgCommand));

            Assert.That(descriptor.Arguments[0].PropertyInfo.Name, Is.EqualTo("StringArg"));
            Assert.That(descriptor.Arguments[1].PropertyInfo.Name, Is.EqualTo("IntegerArg"));
            Assert.That(descriptor.Arguments[2].PropertyInfo.Name, Is.EqualTo("BooleanArg"));
            Assert.That(descriptor.Arguments[3].PropertyInfo.Name, Is.EqualTo("AnotherArg"));
        }

        [Test]
        public void ArgumentAttributeWithoutNameSetShouldGetAutoLongName()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestArgCommand));

            Assert.That(descriptor.Arguments[0].ShortNames.Count, Is.EqualTo(0));
            Assert.That(descriptor.Arguments[0].LongNames.Count, Is.EqualTo(1));
            Assert.That(descriptor.Arguments[0].LongNames[0], Is.EqualTo("stringarg"));
        }

        [Test]
        public void ArgumentAttributeWithoutNameSetButHasPositionSetShouldNotGetAutoLongName()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestArgCommand));

            Assert.That(descriptor.Arguments[1].ShortNames.Count, Is.EqualTo(0));
            Assert.That(descriptor.Arguments[1].LongNames.Count, Is.EqualTo(0));
        }

        [Test]
        public void ArgumentAttributeWithShortNameSetShouldNotHaveAnyLongNames()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestArgCommand));

            Assert.That(descriptor.Arguments[2].ShortNames.Count, Is.EqualTo(1));
            Assert.That(descriptor.Arguments[2].ShortNames[0], Is.EqualTo("f"));
            Assert.That(descriptor.Arguments[2].LongNames.Count, Is.EqualTo(0));
        }

        [Test]
        public void ArgumentAttributeWithLongNameSetShouldNotHaveAnyShortNames()
        {
            var descriptor = builder.BuildDescriptor(typeof(TestArgCommand));

            Assert.That(descriptor.Arguments[3].ShortNames.Count, Is.EqualTo(0));
            Assert.That(descriptor.Arguments[3].LongNames.Count, Is.EqualTo(1));
            Assert.That(descriptor.Arguments[3].LongNames[0], Is.EqualTo("flagarg"));
        }

        #region Test Commands

        private class TestClass : ICommand
        {
            public void Execute()
            {
            }
        }

        private class TestCommand : ICommand
        {
            public void Execute()
            {
            }
        }

        private class Command : ICommand
        {
            public void Execute()
            {
            }
        }

        private class TestArgSimpleCommand : ICommand
        {
            public bool PropertyWithoutAttribute { get; set; }

            [Argument]
            public string PropertyWithAttribute { get; set; }

            public void Execute()
            {
            }
        }

        private class TestArgCommand : ICommand
        {
            [Argument]
            public string StringArg { get; set; }

            [Argument(Position = 0)]
            public int IntegerArg { get; set; }

            [Argument(ShortName = "f")]
            public bool BooleanArg { get; set; }

            [Argument(LongName = "flagarg")]
            public bool AnotherArg { get; set; }

            public void Execute()
            {
            }
        }

        #endregion
    }
}