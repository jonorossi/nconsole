using NConsole.Internal;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class ArgumentDescriptorTests
    {
        [Test]
        public void TestInitialValues()
        {
            ArgumentDescriptor descriptor = new ArgumentDescriptor();

            Assert.That(descriptor.ArgumentType, Is.Null);
            Assert.That(descriptor.ShortNames, Is.Not.Null);
            Assert.That(descriptor.LongNames, Is.Not.Null);
        }
    }
}