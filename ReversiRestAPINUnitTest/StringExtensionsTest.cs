using NUnit.Framework;
using Reversi.API.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.API.UnitTests
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void LimitLength_SourceLengthLessThanMaxLength_ReturnsSourceString()
        {
            // Arrange
            string source = "Hello";
            int maxLength = 10;

            // Act
            string result = source.LimitLength(maxLength);

            // Assert
            Assert.AreEqual(source, result);
        }

        [Test]
        public void LimitLength_SourceLengthEqualsMaxLength_ReturnsSourceString()
        {
            // Arrange
            string source = "Hello";
            int maxLength = 5;

            // Act
            string result = source.LimitLength(maxLength);

            // Assert
            Assert.AreEqual(source, result);
        }

        [Test]
        public void LimitLength_SourceLengthGreaterThanMaxLength_ReturnsTrimmedString()
        {
            // Arrange
            string source = "This is a long string";
            int maxLength = 10;
            string expected = "This is a ";

            // Act
            string result = source.LimitLength(maxLength);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void LimitLength_MaxLengthZero_ReturnsEmptyString()
        {
            // Arrange
            string source = "Hello";
            int maxLength = 0;

            // Act
            string result = source.LimitLength(maxLength);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void LimitLength_MaxLengthNegative_ReturnsOriginalString()
        {
            // Arrange
            string source = "Hello";
            int maxLength = -5;

            // Act
            string result = source.LimitLength(maxLength);

            // Assert
            Assert.AreEqual(source, result);
        }
    }
}
