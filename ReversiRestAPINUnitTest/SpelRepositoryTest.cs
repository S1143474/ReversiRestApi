using NUnit.Framework;
using ReversiRestApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiRestApiNUnitTest
{
    [TestFixture]
    public class SpelRepositoryTest
    {
        private SpelRepository _spelRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            _spelRepository = new SpelRepository();
            _spelRepository.Spellen.Clear();
        }

        [Test]
        public void SpelRepository_AddSpell_SpellAddedToSpellenList()
        {
            // Act
            _spelRepository.AddSpel(new Spel());

            // Assert
            Assert.AreEqual(_spelRepository.GetSpellen().Count, 1);
        }

        #region GetSpellen()

        [Test]
        public void SpelRepository_GetSpellen_ReturnsListOfSpellen()
        {
            // Arrange
            _spelRepository.AddSpel(new Spel());
            _spelRepository.AddSpel(new Spel());
            _spelRepository.AddSpel(new Spel());

            // Act
            List<Spel> result = _spelRepository.GetSpellen();

            // Assert
            Assert.AreNotEqual(result.Count, 0);
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void SpelRepository_GetSpellenEmptyList_ReturnEmptyList()
        {
            // Act
            List<Spel> result = _spelRepository.GetSpellen();

            // Assert
            Assert.AreEqual(0, result.Count);
        }
        #endregion

        #region GetSpel()
        [Test]
        public void SpelRepository_GetSpelFromToken_ReturnSpel()
        {
            // Arrange
            Spel spel = new Spel() { Token = "abcdefg" };
            _spelRepository.AddSpel(spel);

            // Act
            Spel result = _spelRepository.GetSpel("abcdefg");

            // Assert
            Assert.AreEqual(spel, result);
            Assert.AreNotEqual(null, result);
        }

        [Test]
        public void SpelRepository_GetSpelFromWrongToken_ReturnNull()
        {
            // Arrange
            Spel spel = new Spel() { Token = "abcdefg" };
            _spelRepository.AddSpel(spel);

            // Act
            Spel result = _spelRepository.GetSpel("hijklm");

            // Assert
            Assert.AreNotEqual(spel, result);
            Assert.AreEqual(null, result);
        }

        [Test]
        public void SpelRepository_GetSpelWithEmptyToken_ReturnNull()
        {
            // Arrange
            Spel spel = new Spel() { Token = "abcdefg" };
            _spelRepository.AddSpel(spel);

            // Act
            Spel result = _spelRepository.GetSpel("");

            // Assert
            Assert.AreEqual(null, result);
            Assert.AreNotEqual(spel, result);
        }

        [Test]
        public void SpelRepository_GetSpelWithNull_ReturnNull()
        {
            // Arrange
            Spel spel = new Spel() { Token = "abcdefg" };
            _spelRepository.AddSpel(spel);

            // Act
            Spel result = _spelRepository.GetSpel(null);

            // Assert
            Assert.AreEqual(null, result);
            Assert.AreNotEqual(spel, result);
        }
        #endregion
    }
}
