using System;
using NConsole.Internal;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class ValueConverterTests
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
            // Single value
            Assert.That(ParseValue<string>(""), Is.EqualTo(""));
            Assert.That(ParseValue<string>("text"), Is.EqualTo("text"));
            Assert.That(ParseValue<string>("UpperCamelCaseText"), Is.EqualTo("UpperCamelCaseText"));

            // Multiple values
            Assert.That(ParseValue<string[]>("text1"), Is.EqualTo(new[] { "text1" }));
            Assert.That(ParseValue<string[]>("text1,text2"), Is.EqualTo(new[] { "text1", "text2" }));
        }

        [Test]
        public void QuotedString()
        {
            Assert.That(ParseValue<string>("First Second Third"), Is.EqualTo("First Second Third"));

            Assert.That(ParseValue<string[]>("first second,text2"), Is.EqualTo(new[] { "first second", "text2" }));
        }

        [Test]
        public void Numeric()
        {
            Assert.That(ParseValue<int>("12345"), Is.EqualTo(12345));
            Assert.That(ParseValue<int?>("12345"), Is.EqualTo(12345));
            Assert.That(ParseValue<double>("123.45"), Is.EqualTo(123.45));
            Assert.That(ParseValue<double?>("123.45"), Is.EqualTo(123.45));
        }

        [Test]
        public void Enumeration()
        {
            Assert.That(ParseValue<Mode>("mode1"), Is.EqualTo(Mode.Mode1));
            Assert.That(ParseValue<Mode>("mode2"), Is.EqualTo(Mode.Mode2));

            Assert.That(ParseValue<Mode[]>("mode1,mode2"), Is.EqualTo(new[] { Mode.Mode1, Mode.Mode2 }));
        }

        [Flags]
        private enum Mode
        {
            Mode1, Mode2, Mode3
        }

        [Test]
        public void ParseWithAPreviousValue()
        {
            Assert.That(ParseValue<string[]>("text1"), Is.EqualTo(new[] { "text1" }));
            Assert.That(ParseValue<string[]>("text2", "text1"), Is.EqualTo(new[] { "text1", "text2" }));
            Assert.That(ParseValue<string[]>("text3", new[] { "text1", "text2" }), Is.EqualTo(new[] { "text1", "text2", "text3" }));
        }

        [Test]
        public void ParseValueThrowsWithUnsupportedType()
        {
            var ex = Assert.Throws<Exception>(() => ParseValue<EventArgs>("SomehowAnEventArgs"));

            Assert.That(ex.Message, Is.EqualTo("Unsupported argument type 'System.EventArgs' or value 'SomehowAnEventArgs'."));
        }

        private static T ParseValue<T>(string stringValue)
        {
            return (T)ValueConverter.ParseValue(typeof(T), stringValue, null);
        }

        private static T ParseValue<T>(string stringValue, object currentValue)
        {
            return (T)ValueConverter.ParseValue(typeof(T), stringValue, currentValue);
        }
    }
}