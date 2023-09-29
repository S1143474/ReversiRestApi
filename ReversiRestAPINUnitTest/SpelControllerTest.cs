using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ReversiRestApi;
using ReversiRestApi.Controllers;
using ReversiRestApi.Json_obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;

namespace ReversiRestApiNUnitTest
{
    [TestFixture]
    public class SpelControllerTest
    {
        private SpelController _spelController { get; set; }
        private SpelRepository _spelRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            var logger = Mock.Of<ILogger<SpelController>>();
            _spelRepository = new SpelRepository();
            _spelController = new SpelController(_spelRepository, logger: logger);
        }

        [Test]
        public void SpelController_GetSpelOmschrijvingVanSpellenMetWachtendeSpeler_ReturnsListOfGameDescriptions()
        {
            // Act
            List<string> result = _spelController.GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken.None).Result.Value.ToList();
            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void SpelController_GetGameReversiWithWaitingPlayers_ReturnSpelList()
        {
            // Act
            List<SpelJsonObj> result = _spelController.GetGameReversiWithWaitingPlayers(CancellationToken.None).Result.Value;

            // Assert
            Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
        }

        #region PlaceNewGameReversi
        [Test]
        public void SpelController_PlaceNewGameReversiWithTokenAndDescription_AddsNewGameToList()
        {
            // Act
            _spelController.PlaceNewGameReversi(CancellationToken.None, new PlaceGameJsonObj() { PlayerToken = "abcdef", Description = "description test" });
            List<string> result = _spelController.GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken.None).Result.Value.ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void SpelController_PlaceNewGameReversiWithoutTokenAndWithDescription_AddsNewGameToList()
        {
            // Act
            _spelController.PlaceNewGameReversi(CancellationToken.None, new PlaceGameJsonObj() { PlayerToken = null, Description = "description test" });
            List<string> result = _spelController.GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken.None).Result.Value.ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void SpelController_PlaceNewGameReversiWithTokenAndWithoutDescription_AddsNewGameToList()
        {
            // Act
            _spelController.PlaceNewGameReversi(CancellationToken.None, new PlaceGameJsonObj() { PlayerToken = "abcdef", Description = null });
            List<string> result = _spelController.GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken.None).Result.Value.ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void SpelController_PlaceNewGameReversiWithEmptyTokenAndWithDescription_AddsNewGameToList()
        {
            // Act
            _spelController.PlaceNewGameReversi(CancellationToken.None, new PlaceGameJsonObj() { PlayerToken = "", Description = "Test Description" });
            List<string> result = _spelController.GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken.None).Result.Value.ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void SpelController_PlaceNewGameReversiWithTokenAndWithEmptyDescription_AddsNewGameToList()
        {
            // Act
            _spelController.PlaceNewGameReversi(CancellationToken.None, new PlaceGameJsonObj() { PlayerToken = "abcdef", Description = "" });
            List<string> result = _spelController.GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken.None).Result.Value.ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void SpelController_PlaceNewGameReversiWithWhiteSpaceTokenAndWithDescription_AddsNewGameToList()
        {
            // Act
            _spelController.PlaceNewGameReversi(CancellationToken.None, new PlaceGameJsonObj() { PlayerToken = " ", Description = "Test Description" });
            List<string> result = _spelController.GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken.None).Result.Value.ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void SpelController_PlaceNewGameReversiWithTokenAndWithWhiteSpaceDescription_AddsNewGameToList()
        {
            // Act
            _spelController.PlaceNewGameReversi(CancellationToken.None, new PlaceGameJsonObj() { PlayerToken = "abcdef", Description = " " });
            List<string> result = _spelController.GetSpelOmschrijvingVanSpellenMetWachtendeSpeler(CancellationToken.None).Result.Value.ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
        }
        #endregion

        #region RetrieveGameReversiWithSpelToken
        [Test]
        public void SpelController_RetrieveGameReversiWithToken_ReturnSpelWithSameToken()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversi(CancellationToken.None, "abcdef").Result.Value;

            // Assert
            Assert.IsNotNull(result.Token);
            Assert.AreEqual("abcdef", result.Token);
        }

        [Test]
        public void SpelController_RetrieveGameReversiWithoutToken_ReturnNull()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversi(CancellationToken.None, null).Result.Value;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SpelController_RetrieveGameReversiWithEmptyToken_ReturnNull()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversi(CancellationToken.None, "").Result.Value;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SpelController_RetrieveGameReversiWithWrongToken_ReturnNull()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversi(CancellationToken.None, "WrongToken").Result.Value;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region RetrieveGameReversiWithSpelerToken
        [Test]
        public void SpelController_RetrieveGameReversiWithSpeler1Token_ReturnSpel()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversiViaSpelerToken(CancellationToken.None, "ghijkl").Result.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ghijkl", result.Speler1Token);
        }

        [Test]
        public void SpelController_RetrieveGameReversiWithSpeler2Token_ReturnSpel()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversiViaSpelerToken(CancellationToken.None, "mnoqpr").Result.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("mnoqpr", result.Speler2Token);
        }

        [Test]
        public void SpelController_CheckIfTheSameGameReturnsWithSpeler1AndSpeler2Token_ReturnSpell()
        {
            // Act
            SpelJsonObj speler1Result = _spelController.RetrieveGameReversiViaSpelerToken(CancellationToken.None, "ghijkl").Result.Value;
            SpelJsonObj speler2Result = _spelController.RetrieveGameReversiViaSpelerToken(CancellationToken.None,"mnoqpr").Result.Value;

            // Assert
            Assert.IsNotNull(speler1Result);
            Assert.IsNotNull(speler2Result);
            Assert.AreEqual("ghijkl", speler2Result.Speler1Token);
            Assert.AreEqual("mnoqpr", speler1Result.Speler2Token);
        }

        [Test]
        public void SpelController_RetrieveGameReversiWithNoneExistingSpelerToken_ReturnNull()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversiViaSpelerToken(CancellationToken.None, "WrongToken").Result.Value;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SpelController_RetrieveGameReversiWithEmptySpelerToken_ReturnNull()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversiViaSpelerToken(CancellationToken.None, "").Result.Value;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SpelController_RetrieveGameReversiWithSpelerTokenNull_ReturnNull()
        {
            // Act
            SpelJsonObj result = _spelController.RetrieveGameReversiViaSpelerToken(CancellationToken.None, null).Result.Value;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region PutDoMove

        [Test]
        public void SpelController_PutDoMovePassMove_ReturnTrue()
        {
            // Arrange
            MoveJsonObj moveJsonObj = new MoveJsonObj()
            {
                HasPassed = true,
                Token = "abcdef",
                SpelerToken = "abcdef",
                Y = 2,
                X = 3
            };

            // Act
            var result = _spelController.PutDoMove(CancellationToken.None, moveJsonObj).Result.Value as PutDoMoveExecutedJsonObj;

            // Assert
            Assert.IsTrue(result.IsPlaceExecuted);
        }

        [Test]
        public void SpelController_PutDoMovePlacefiche_ReturnTrue()
        {
            // Arrange
            MoveJsonObj moveJsonObj = new MoveJsonObj()
            {
                HasPassed = false,
                Token = "abcdef",
                SpelerToken = "abcdef",
                Y = 2,
                X = 3
            };

            // Act
            var result = _spelController.PutDoMove(CancellationToken.None, moveJsonObj).Result.Value as PutDoMoveExecutedJsonObj;

            // Assert
            Assert.IsTrue(result.IsPlaceExecuted);
        }

        [Test]
        public void SpelController_PutDoMovePlaceficheOnFiche_ReturnFalse()
        {
            // Arrange
            MoveJsonObj moveJsonObj = new MoveJsonObj()
            {
                HasPassed = false,
                Token = "abcdef",
                SpelerToken = "abcdef",
                Y = 2,
                X = 4
            };

            // Act
            var result = _spelController.PutDoMove(CancellationToken.None, moveJsonObj).Result.Value as PutDoMoveExecutedJsonObj;

            // Assert
            Assert.IsFalse(result.IsPlaceExecuted);
        }

        [Test]
        public void SpelController_PutDoMovePlaceficheOnWrongSpot_ReturnFalse()
        {
            // Arrange
            MoveJsonObj moveJsonObj = new MoveJsonObj()
            {
                HasPassed = false,
                Token = "abcdef",
                SpelerToken = "abcdef",
                Y = 0,
                X = 0
            };

            // Act
            var result = _spelController.PutDoMove(CancellationToken.None, moveJsonObj).Result.Value as PutDoMoveExecutedJsonObj;

            // Assert
            Assert.IsFalse(result.IsPlaceExecuted);
        }

        [Test]
        public void SpelController_PutDoMovePlaceficheWithSpelerToken_ReturnFalse()
        {
            // Arrange
            MoveJsonObj moveJsonObj = new MoveJsonObj()
            {
                HasPassed = false,
                Token = null,
                SpelerToken = "abcdef",
                Y = 2,
                X = 3
            };

            // Act
            var result = _spelController.PutDoMove(CancellationToken.None, moveJsonObj).Result.Value as PutDoMoveExecutedJsonObj;

            // Assert
            Assert.IsTrue(result.IsPlaceExecuted);
        }

        #endregion

        #region GetNextPlayerTurn
        [Test]
        public void SpelController_GetNextPlayerTurnNotStarted_ReturnZero()
        {
            // Act
            var result = _spelController.GetNextPlayerTurn(CancellationToken.None, "abcdef").Result.Value;

            // Assert
            Assert.AreNotEqual(-1, result.Beurt);
            Assert.AreEqual(2, result.Beurt);
        }

        [Test]
        public void SpelController_GetNextPlayerTurnGameNotExisting_ReturnNegativeOne()
        {
            // Act
            var result = _spelController.GetNextPlayerTurn(CancellationToken.None, "WrongToken").Result.Value;

            // Assert
            Assert.AreEqual(-1, result.Beurt);
        }

        #endregion

        #region GiveUp

        [Test]
        public void SpelController_GiveUpWithSpelerToken_ReturnTrue()
        {
            // Arrange
            GiveUpJsonObj giveUpJsonObj = new GiveUpJsonObj() { Token = "", SpelerToken = "abcdef" };

            // Act
            bool result = _spelController.GiveUp(CancellationToken.None, giveUpJsonObj).Result.Value;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SpelController_GiveUpWithSpelToken_ReturnTrue()
        {
            // Arrange
            GiveUpJsonObj giveUpJsonObj = new GiveUpJsonObj() { Token = "abcdef", SpelerToken = "" };

            // Act
            bool result = _spelController.GiveUp(CancellationToken.None, giveUpJsonObj).Result.Value;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SpelController_GiveUpWithoutTokens_ReturnFalse()
        {
            // Arrange
            GiveUpJsonObj giveUpJsonObj = new GiveUpJsonObj() { Token = "", SpelerToken = "" };

            // Act
            bool result = _spelController.GiveUp(CancellationToken.None, giveUpJsonObj).Result.Value;

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void SpelController_GiveUpWithInvalidToken_ReturnFalse()
        {
            // Arrange
            GiveUpJsonObj giveUpJsonObj = new GiveUpJsonObj() { Token = "1234asdf", SpelerToken = "" };

            // Act
            bool result = _spelController.GiveUp(CancellationToken.None, giveUpJsonObj).Result.Value;

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void SpelController_GiveUpWithInvalidSpelerToken_ReturnFalse() { 
            // Arrange
            GiveUpJsonObj giveUpJsonObj = new GiveUpJsonObj() { Token = "", SpelerToken = "asdf1234" };

            // Act
            bool result = _spelController.GiveUp(CancellationToken.None, giveUpJsonObj).Result.Value;

            // Assert
            Assert.IsFalse(result);
        }
        #endregion
    }
}
