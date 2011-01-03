using System.Linq;
using NUnit.Framework;

namespace Cyclops.Core.Tests
{
    [TestFixture]
    public class CommonExtensionsTests
    {
        [Test]
        public void AnyTest()
        {
            Assert.IsFalse(new int[0].Any());
            Assert.IsTrue(new[] {1}.Any());
        }

        [Test]
        public void CollectionToStringTest()
        {
            var a = new[] {new {Name = "1"}, new {Name = "22"}};
            var b = new[] {1, 2, 3};
            var c = new int[0];
            var d = new[] {1};

            Assert.AreEqual("1; 22", a.CollectionToString(i => i.Name));
            Assert.AreEqual("1__2__3", b.CollectionToString(i => i, "__"));
            Assert.AreEqual("", c.CollectionToString(i => i, "."));
            Assert.AreEqual("1", d.CollectionToString(i => i, "."));
        }

        [Test]
        public void IsNullOrEmptyTest()
        {
            Assert.IsTrue(new int[0].IsNullOrEmpty());
            Assert.IsTrue(new int[] {}.IsNullOrEmpty());
            Assert.IsFalse(new[] {0, 1, 2, 3}.IsNullOrEmpty());
            Assert.IsFalse(new[] {0}.IsNullOrEmpty());
            Assert.IsFalse(new object[] {null}.IsNullOrEmpty());
            Assert.IsFalse(new object[] {1, "1", 1.0}.IsNullOrEmpty());
        }

        [Test]
        public void SplitTest()
        {
            string str = "hhh mmm nnn lll";
            var parts = str.SplitAndIncludeDelimiters(new[] {"hhh", "nnn"}).ToArray();
            Assert.AreEqual(4, parts.Length);
        }
    }
}