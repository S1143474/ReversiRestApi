using NUnit.Framework;
using ReversiRestApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task SpelRepository_AddSpell_SpellAddedToSpellenList()
        {
            // Arrange
            _spelRepository.AddSpel(CancellationToken.None, new Spel());

            // Act
            List<Spel> result = await _spelRepository.GetSpellenAsync(CancellationToken.None);


            // Assert
            Assert.AreEqual(result.Count, 1);
        }

        #region GetSpellen()

        [Test]
        public async Task SpelRepository_GetSpellen_ReturnsListOfSpellen()
        {
            // Arrange
            _spelRepository.AddSpel(CancellationToken.None, new Spel());
            _spelRepository.AddSpel(CancellationToken.None, new Spel());
            _spelRepository.AddSpel(CancellationToken.None, new Spel());

            // Act
            List<Spel> result = await _spelRepository.GetSpellenAsync(CancellationToken.None);

            // Assert
            Assert.AreNotEqual(result.Count, 0);
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public async Task SpelRepository_GetSpellenEmptyList_ReturnEmptyList()
        {
            // Act
            List<Spel> result = await _spelRepository.GetSpellenAsync(CancellationToken.None);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
        #endregion

        #region GetSpel()
        [Test]
        public async Task SpelRepository_GetSpelFromToken_ReturnSpel()
        {
            // Arrange
            Spel spel = new Spel() { Token = "abcdefg" };
            _spelRepository.AddSpel(CancellationToken.None, spel);

            // Act
            Spel result = await _spelRepository.GetSpel(CancellationToken.None, "abcdefg");

            // Assert
            Assert.AreEqual(spel, result);
            Assert.AreNotEqual(null, result);
        }

        [Test]
        public async Task SpelRepository_GetSpelFromWrongToken_ReturnNull()
        {
            // Arrange
            Spel spel = new Spel() { Token = "abcdefg" };
            _spelRepository.AddSpel(CancellationToken.None, spel);

            // Act
            Spel result = await _spelRepository.GetSpel(CancellationToken.None, "hijklm");

            // Assert
            Assert.AreNotEqual(spel, result);
            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task SpelRepository_GetSpelWithEmptyToken_ReturnNull()
        {
            // Arrange
            Spel spel = new Spel() { Token = "abcdefg" };
            _spelRepository.AddSpel(CancellationToken.None, spel);

            // Act
            Spel result = await _spelRepository.GetSpel(CancellationToken.None, "");

            // Assert
            Assert.AreEqual(null, result);
            Assert.AreNotEqual(spel, result);
        }

        [Test]
        public async Task SpelRepository_GetSpelWithNull_ReturnNull()
        {
            // Arrange
            Spel spel = new Spel() { Token = "abcdefg" };
            _spelRepository.AddSpel(CancellationToken.None, spel);

            // Act
            Spel result = await _spelRepository.GetSpel(CancellationToken.None, null);

            // Assert
            Assert.AreEqual(null, result);
            Assert.AreNotEqual(spel, result);
        }
        #endregion
    }
}
