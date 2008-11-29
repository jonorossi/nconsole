using System;
using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineParserValueParsingTests
    {
        [Test]
        public void Boolean()
        {
            // True
            Assert.IsTrue(ParseValue<bool>(null));
            Assert.IsTrue(ParseValue<bool>(""));
            Assert.IsTrue(ParseValue<bool>("+"));

            // False
            Assert.IsFalse(ParseValue<bool>("-"));
        }

        [Test]
        public void String()
        {
            Assert.AreEqual("", ParseValue<string>(""));
            Assert.AreEqual("text", ParseValue<string>("text"));
            Assert.AreEqual("UpperCamelCaseText", ParseValue<string>("UpperCamelCaseText"));

            Assert.AreEqual(new[] { "text1" }, ParseValue<string[]>("text1"));
            Assert.AreEqual(new[] { "text1", "text2" }, ParseValue<string[]>("text1,text2"));
        }

        [Test]
        public void QuotedString()
        {
            Assert.AreEqual("First Second Third", ParseValue<string>("First Second Third"));

            Assert.AreEqual(new[] { "first second", "text2" }, ParseValue<string[]>("first second,text2"));
        }

        [Test]
        public void Numeric()
        {
            Assert.AreEqual(12345, ParseValue<int>("12345"));
            Assert.AreEqual(123.45, ParseValue<double>("123.45"));
        }

        [Test]
        public void Enumeration()
        {
            Assert.AreEqual(Mode.Mode1, ParseValue<Mode>("mode1"));
            Assert.AreEqual(Mode.Mode2, ParseValue<Mode>("mode2"));

            Assert.AreEqual(new[] { Mode.Mode1, Mode.Mode2 }, ParseValue<Mode[]>("mode1,mode2"));
        }

        [Flags]
        private enum Mode
        {
            Mode1,
            Mode2
        }

        [Test]
        public void ParseValueThrowsWithUnsupportedType()
        {
            AssertEx.Throws<CommandLineArgumentException>("Unsupported argument type 'System.EventArgs' or value 'SomehowAnEventArgs'.",
                delegate { ParseValue<EventArgs>("SomehowAnEventArgs"); }
            );
        }

        private static T ParseValue<T>(string stringValue)
        {
            var parser = new CommandLineParser<object>();
            return (T)parser.ParseValue(typeof(T), stringValue);
        }
    }
}