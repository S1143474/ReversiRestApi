using Reversi.API.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Reversi.Domain.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Speltoken_Required_Success()
        {
            // Arrange
            var spel = new Spel { Token = Guid.NewGuid(), Bord = "", Omschrijving = "", Speler1Token = Guid.NewGuid() };

            // Act
            var context = new ValidationContext(spel);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(spel, context, results, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Speltoken_Required_Failure()
        {
            // Arrange
            var spel = new Spel();

            // Act
            var context = new ValidationContext(spel);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(spel, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, result => result.MemberNames.Contains("Token"));
        }
    }
}