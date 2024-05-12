using NUnit.Framework;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.API.UnitTests
{
    [TestFixture]
    public class MappingExtensionsTest
    {
        [Test]
        public void ToByteArray_WithValidArray_ReturnsCorrectByteArray()
        {
            // Arrange
            int[,] intArray = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            byte[] expectedByteArray = { 1, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 6, 0, 0, 0 };

            // Act
            byte[] result = MappingExtensions.ToByteArray(intArray);

            // Assert
            Assert.AreEqual(expectedByteArray, result);
        }

        [Test]
        public void ToByteArray_WithEmptyArray_ReturnsEmptyByteArray()
        {
            // Arrange
            int[,] intArray = new int[0, 0];

            // Act
            byte[] result = MappingExtensions.ToByteArray(intArray);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void ToByteArray_WithNegativeValues_ReturnsCorrectByteArray()
        {
            // Arrange
            int[,] intArray = new int[,] { { -1, -2, -3 }, { -4, -5, -6 } };
            byte[] expectedByteArray = { 255, 255, 255, 255, 254, 255, 255, 255, 253, 255, 255, 255, 252, 255, 255, 255, 251, 255, 255, 255, 250, 255, 255, 255 };

            // Act
            byte[] result = MappingExtensions.ToByteArray(intArray);

            // Assert
            Assert.AreEqual(expectedByteArray, result);
        }

        [Test]
        public void FromKleurToIntArray_WithValidKleurArray_ReturnsCorrectIntArray()
        {
            // Arrange
            Kleur[,] kleurArray = new Kleur[,] { { Kleur.Zwart, Kleur.Wit }, { Kleur.Geen, Kleur.Zwart } };
            int[,] expectedIntArray = { { 2, 1 }, { 0, 2 } };

            // Act
            int[,] result = MappingExtensions.FromKleurToIntArray(kleurArray);

            // Assert
            Assert.AreEqual(expectedIntArray, result);
        }

        [Test]
        public void FromKleurToIntArray_WithEmptyArray_ReturnsEmptyIntArray()
        {
            // Arrange
            Kleur[,] kleurArray = new Kleur[0, 0];

            // Act
            int[,] result = MappingExtensions.FromKleurToIntArray(kleurArray);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void FromKleurToIntArray_WithNullArray_ReturnsNull()
        {
            // Arrange
            Kleur[,] kleurArray = null;

            // Act
            int[,] result = MappingExtensions.FromKleurToIntArray(kleurArray);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void FromIntToKleurArray_WithValidIntArray_ReturnsCorrectKleurArray()
        {
            // Arrange
            int[,] intArray = { { 2, 1 }, { 0, 2 } };
            Kleur[,] expectedKleurArray = { { Kleur.Zwart, Kleur.Wit }, { Kleur.Geen, Kleur.Zwart } };

            // Act
            Kleur[,] result = MappingExtensions.FromIntToKleurArray(intArray);

            // Assert
            Assert.AreEqual(expectedKleurArray, result);
        }

        [Test]
        public void FromIntToKleurArray_WithEmptyArray_ReturnsEmptyKleurArray()
        {
            // Arrange
            int[,] intArray = new int[0, 0];

            // Act
            Kleur[,] result = MappingExtensions.FromIntToKleurArray(intArray);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void FromIntToKleurArray_WithNegativeValues_ReturnsCorrectKleurArray()
        {
            // Arrange
            int[,] intArray = { { -1, -1 }, { -1, -1 } };
            Kleur[,] expectedKleurArray = { { Kleur.Geen, Kleur.Geen }, { Kleur.Geen, Kleur.Geen } };

            // Act
            Kleur[,] result = MappingExtensions.FromIntToKleurArray(intArray);

            // Assert
            Assert.AreEqual(expectedKleurArray, result);
        }
    }
}
