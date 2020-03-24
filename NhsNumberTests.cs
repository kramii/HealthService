using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NhsNumberTests
{
    [TestClass]
    public class NhsNumberTests
    {
        [TestMethod]
        public void ShouldGetUnprocessedValue()
        {
            var nhsNumber = new NhsNumber.NhsNumber("1234567890");
            Assert.AreEqual("1234567890", nhsNumber.Raw);
        }

        [TestMethod]
        public void When334ShouldStripSeparators()
        {
            var nhsNumber = new NhsNumber.NhsNumber("123 456 7890");
            Assert.AreEqual("1234567890", nhsNumber.WithoutSeparators);
        }

        [TestMethod]
        public void When334ShouldStripCustomSeparators()
        {
            var nhsNumber = new NhsNumber.NhsNumber("123-456-7890", '-');
            Assert.AreEqual("1234567890", nhsNumber.WithoutSeparators);
        }

        [TestMethod]
        public void When334ShouldAutoDetectAndStripSeparators()
        {
            var nhsNumber = new NhsNumber.NhsNumber("123+456+7890", null);
            Assert.AreEqual("1234567890", nhsNumber.WithoutSeparators);
        }

        [TestMethod]
        public void WhenNot334ShouldIgnoreSeparators()
        {
            var nhsNumber = new NhsNumber.NhsNumber("123 456 78900", null);
            Assert.AreEqual("123 456 78900", nhsNumber.WithoutSeparators);
        }

        [TestMethod]
        public void WhenNotNumericShouldIgnoreSeparators()
        {
            var nhsNumber = new NhsNumber.NhsNumber("AAA BBB CCCC", null);
            Assert.AreEqual("AAA BBB CCCC", nhsNumber.WithoutSeparators);
        }

        [TestMethod]
        public void WhenIncludesNonDigitsShouldNotStripSeparators()
        {
            var nhsNumber = new NhsNumber.NhsNumber("A23-456-7890");
            Assert.AreEqual("A23-456-7890", nhsNumber.WithoutSeparators);
        }

        [TestMethod]
        public void WhenTenDigitsShouldReturnFormattedValue()
        {
            var nhsNumber = new NhsNumber.NhsNumber("1234567890");
            Assert.AreEqual("123 456 7890", nhsNumber.Formatted);
        }

        [TestMethod]
        public void WhenNumberIsValidShouldReturnValid()
        {
            // 401 023 2137 is a valid NHS Number.
            var nhsNumber = new NhsNumber.NhsNumber("401 023 2137");
            Assert.IsTrue(nhsNumber.IsValid);
        }

        [TestMethod]
        public void WhenNumberIsNotValidShouldReturnNotValid()
        {
            // 401 023 2137 has a bad check digit.
            var nhsNumber = new NhsNumber.NhsNumber("401 023 2138");
            Assert.IsFalse(nhsNumber.IsValid);
        }

        [TestMethod]
        public void WhenNotNumericShouldBeInvalid()
        {
            var nhsNumber = new NhsNumber.NhsNumber("401CAT2137");
            Assert.IsFalse(nhsNumber.IsValid);
        }
    }
}
