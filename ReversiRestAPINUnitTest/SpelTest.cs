using NUnit.Framework;
using Reversi.API.Application.Common;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Application.Spellen.Commands.InProcessSpelMove.MoveModels;
using Reversi.API.Domain.Entities;
using Reversi.API.Domain.Enums;
using Reversi.API.Infrastructure.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Reversi.API.UnitTests
{
    [TestFixture]
    public class SpelTest
    {
        private readonly ISpelMovement _spelMovement;
        private int[,] _bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
        public SpelTest() 
        {
            _spelMovement = new SpelMovementService();
        }
        // geen kleur = 0
        // Wit = 1
        // Zwart = 2
        [Test]
        public void ZetMogelijk_BuitenBord_ReturnFalse()
        {
            // Arrange
            Spel spel = new()
            {
                // 0 1 2 3 4 5 6 7
                // v
                // 0 0 0 0 0 0 0 0 0
                // 1 0 0 0 0 0 0 0 0
                // 2 0 0 0 0 0 0 0 0
                // 3 0 0 0 1 2 0 0 0
                // 4 0 0 0 2 1 0 0 0
                // 5 0 0 0 0 0 0 0 0
                // 6 0 0 0 0 0 0 0 0
                // 7 0 0 0 0 0 0 0 0
                // 1 <
                // Act
                AandeBeurt = (int)Kleur.Wit,
                Bord = _bord.MapIntArrToBase64String()
            };

            var actual = _spelMovement.ZetMogelijk(ref spel, 8, 8);
            Assert.IsFalse(actual);
        }

        [Test]
        public void ZetMogelijk_StartSituatieZet23Zwart_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            spel.Bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            }.MapIntArrToBase64String();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 2 0 0 0 0 <
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 2, 3);
            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void ZetMogelijk_StartSituatieZet23Wit_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            spel.Bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            }.MapIntArrToBase64String();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 1 0 0 0 0 <
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 2, 3);
            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void ZetMogelijk_ZetAanDeRandBoven_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            bord[1, 3] = (int)Kleur.Wit;
            bord[2, 3] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();
            /*spel.Bord[1, 3] = Kleur.Wit;
            spel.Bord[2, 3] = Kleur.Wit;*/

            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 2 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 0, 3);
            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void ZetMogelijk_ZetAanDeRandBoven_ReturnFalse()
        {
            // Arrange
            /*Spel spel = new Spel();
            spel.Bord[1, 3] = Kleur.Wit;
            spel.Bord[2, 3] = Kleur.Wit;*/
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            }; 
            bord[1, 3] = (int)Kleur.Wit;
            bord[2, 3] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 1 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            /*spel.AandeBeurt = Kleur.Wit;
            var actual = spel.ZetMogelijk(0, 3);*/
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 0, 3);
            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void ZetMogelijk_ZetAanDeRandBovenEnTotBenedenReedsGevuld_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            bord[1, 3] = (int)Kleur.Wit;
            bord[2, 3] = (int)Kleur.Wit;
            bord[3, 3] = (int)Kleur.Wit;
            bord[4, 3] = (int)Kleur.Wit;
            bord[5, 3] = (int)Kleur.Wit;
            bord[6, 3] = (int)Kleur.Wit;
            bord[7, 3] = (int)Kleur.Zwart;
            spel.Bord = bord.MapIntArrToBase64String();

            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 2 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 1 1 0 0 0
            // 5 0 0 0 1 0 0 0 0
            // 6 0 0 0 1 0 0 0 0
            // 7 0 0 0 2 0 0 0 0

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 0, 3);

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void ZetMogelijk_ZetAanDeRandBovenEnTotBenedenReedsGevuld_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            bord[1, 3] = (int)Kleur.Wit;
            bord[2, 3] = (int)Kleur.Wit;
            bord[3, 3] = (int)Kleur.Wit;
            bord[4, 3] = (int)Kleur.Wit;
            bord[5, 3] = (int)Kleur.Wit;
            bord[6, 3] = (int)Kleur.Wit;
            bord[7, 3] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();

            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 2 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 1 1 0 0 0
            // 5 0 0 0 1 0 0 0 0
            // 6 0 0 0 1 0 0 0 0
            // 7 0 0 0 1 0 0 0 0

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 0, 3);

            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void ZetMogelijk_ZetAanDeRandRechts_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            bord[4, 5] = (int)Kleur.Wit;
            bord[4, 6] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();

            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 2 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 1 1 2
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 4, 7);

            // Assert
            Assert.IsTrue(actual);

        }

        [Test]
        public void ZetMogelijk_ZetAanDeRandRechts_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            bord[4, 5] = (int)Kleur.Wit;
            bord[4, 6] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();

            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 1 0 0 0 0
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 1 1 1 <
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 4, 7);

            // Assert
            Assert.IsFalse(actual);

        }

        [Test]
        public void ZetMogelijk_ZetAanDeRandRechtsEnTotLinksReedsGevuld_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            bord[4, 0] = (int)Kleur.Zwart;
            bord[4, 1] = (int)Kleur.Wit;
            bord[4, 2] = (int)Kleur.Wit;
            bord[4, 3] = (int)Kleur.Wit;
            bord[4, 4] = (int)Kleur.Wit;
            bord[4, 5] = (int)Kleur.Wit;
            bord[4, 6] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();

            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 2 1 1 1 1 1 1 2 <
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 4, 7);

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void ZetMogelijk_ZetAanDeRandRechtsEnTotLinksReedsGevuld_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            bord[4, 0] = (int)Kleur.Zwart;
            bord[4, 1] = (int)Kleur.Wit;
            bord[4, 2] = (int)Kleur.Wit;
            bord[4, 3] = (int)Kleur.Wit;
            bord[4, 4] = (int)Kleur.Wit;
            bord[4, 5] = (int)Kleur.Wit;
            bord[4, 6] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();

            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 2 1 1 1 1 1 1 1 <
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 4, 7);

            // Assert
            Assert.IsFalse(actual);

        }
        // 0 1 2 3 4 5 6 7
        //
        // 0 0 0 0 0 0 0 0 0
        // 1 0 0 0 0 0 0 0 0
        // 2 0 0 0 0 0 0 0 0
        // 3 0 0 0 1 2 0 0 0
        // 4 0 0 0 2 1 0 0 0
        // 5 0 0 0 0 0 0 0 0
        // 6 0 0 0 0 0 0 0 0
        // 7 0 0 0 0 0 0 0 0
        [Test]
        public void ZetMogelijk_StartSituatieZet22Wit_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 1 0 0 0 0 0 <
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            bord[2, 2] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 2, 2);

            // Assert
            Assert.IsFalse(actual);

        }
        [Test]
        public void ZetMogelijk_StartSituatieZet22Zwart_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 2 0 0 0 0 0 <
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            bord[2, 2] = (int)Kleur.Zwart;
            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 2, 2);

            // Assert
            Assert.IsFalse(actual);

        }
        [Test]
        public void ZetMogelijk_ZetAanDeRandRechtsBoven_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 1 <
            // 1 0 0 0 0 0 0 2 0
            // 2 0 0 0 0 0 2 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 1 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            bord[5, 2] = (int)Kleur.Wit;
            bord[2, 5] = (int)Kleur.Zwart;
            bord[1, 6] = (int)Kleur.Zwart;
            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 0, 7);

            // Assert
            Assert.IsTrue(actual);

        }
        [Test]
        public void ZetMogelijk_ZetAanDeRandRechtsBoven_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 2 <
            // 1 0 0 0 0 0 0 2 0
            // 2 0 0 0 0 0 2 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 1 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            bord[0, 7] = (int)Kleur.Zwart;
            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 0, 7);

            // Assert
            Assert.IsFalse(actual);

        }
        [Test]
        public void ZetMogelijk_ZetAanDeRandRechtsOnder_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 2 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 1 0 0
            // 6 0 0 0 0 0 0 1 0
            // 7 0 0 0 0 0 0 0 2 <

            bord[2, 2] = (int)Kleur.Zwart;
            bord[5, 5] = (int)Kleur.Wit;
            bord[6, 6] = (int)Kleur.Wit;

            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 7, 7);

            // Assert
            Assert.IsTrue(actual);

        }
        [Test]
        public void ZetMogelijk_ZetAanDeRandRechtsOnder_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 2 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 1 0 0
            // 6 0 0 0 0 0 0 1 0
            // 7 0 0 0 0 0 0 0 1 <

            bord[7, 7] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 7, 7);

            // Assert
            Assert.IsFalse(actual);

        }
        [Test]
        public void ZetMogelijk_ZetAanDeRandLinksBoven_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 2 0 0 0 0 0 0 0
            // 1 0 1 0 0 0 0 0 0
            // 2 0 0 1 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 2 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0 <

            bord[2, 2] = (int)Kleur.Wit;
            bord[1, 1] = (int)Kleur.Wit;
            bord[5, 5] = (int)Kleur.Zwart;

            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 0, 0);

            // Assert
            Assert.IsTrue(actual);

        }
        [Test]
        public void ZetMogelijk_ZetAanDeRandLinksBoven_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 1 0 0 0 0 0 0 0
            // 1 0 1 0 0 0 0 0 0
            // 2 0 0 1 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 2 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0 <

            bord[0, 0] = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 0, 0);

            // Assert
            Assert.IsFalse(actual);

        }
        [Test]
        public void ZetMogelijk_ZetAanDeRandLinksOnder_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 1 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 2 0 0 0 0 0
            // 6 0 2 0 0 0 0 0 0
            // 7 1 0 0 0 0 0 0 0 <

            bord[0, 0] = (int)Kleur.Wit;
            bord[2, 5] = (int)Kleur.Wit;
            bord[5, 2] = (int)Kleur.Zwart;
            bord[6, 1] = (int)Kleur.Zwart;
            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            var actual = _spelMovement.ZetMogelijk(ref spel, 7, 0);

            // Assert
            Assert.IsTrue(actual);

        }
        [Test]
        public void ZetMogelijk_ZetAanDeRandLinksOnder_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // Comments for the board
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 1 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 2 0 0 0 0 0
            // 6 0 2 0 0 0 0 0 0
            // 7 2 0 0 0 0 0 0 0 <

            bord[7, 0] = (int)Kleur.Zwart;
            spel.Bord = bord.MapIntArrToBase64String();

            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            var actual = _spelMovement.ZetMogelijk(ref spel, 7, 0);

            // Assert
            Assert.IsFalse(actual);

        }
        //---------------------------------------------------------------------------
        [Test]
        public void DoeZet_BuitenBord_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // 1 <
            // Act
            spel.AandeBeurt = (int)Kleur.Wit;
            spel.Bord = bord.MapIntArrToBase64String();
            var flipedfisched = new List<CoordsModel>();
            var actual = _spelMovement.DoeZet(ref spel, 8, 8, out flipedfisched);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.AandeBeurt);
        }
        [Test]
        public void DoeZet_StartSituatieZet23Zwart_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 2 0 0 0 0 <
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            spel.AandeBeurt = (int)Kleur.Zwart;
            spel.Bord = bord.MapIntArrToBase64String();
            var flipedfisched = new List<CoordsModel>();
            var actual = _spelMovement.DoeZet(ref spel, 2, 3, out flipedfisched);

            // Assert
            Assert.IsTrue(actual);
            Assert.AreEqual(3, flipedfisched.Count);
            var intBord = spel.Bord.MapStringBordTo2DIntArr();
            Assert.AreEqual((int)Kleur.Zwart, intBord[2, 3]);
            Assert.IsTrue(flipedfisched.Any(model => model.X == 3 && model.Y == 3));
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.AandeBeurt);
        }
        [Test]
        public void DoeZet_StartSituatieZet23Wit_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();

            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 1 0 0 0 0 <
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 2, 3, out flippedfishes);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Geen, spel.Bord.MapStringBordTo2DIntArr()[2, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.AandeBeurt);
        }
        [Test]
        public void DoeZet_ZetAanDeRandBoven_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 2 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 0, 3, out flippedfishes);
            // Assert
            Assert.IsTrue(actual);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[0, 3]);
            
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 1));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 2));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 3));
            //Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[1, 3]);
            //Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[2, 3]);
            //Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.AandeBeurt);
        }
        [Test]
        public void DoeZet_ZetAanDeRandBoven_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            var flippedfishes = new List<CoordsModel>();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 1 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 0, 3, out flippedfishes);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[1, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[2, 3]);
            Assert.AreEqual((int)Kleur.Geen, spel.Bord.MapStringBordTo2DIntArr()[0, 3]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandBovenEnTotBenedenReedsGevuld_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 1, 1, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 2, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 2 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 1 1 0 0 0
            // 5 0 0 0 1 0 0 0 0
            // 6 0 0 0 1 0 0 0 0
            // 7 0 0 0 2 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 0, 3, out flippedfishes);
            // Assert
            Assert.IsTrue(actual);
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 0));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 1));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 3));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 3));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 5));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 6));

            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[0, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[1, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[2, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[5, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[6, 3]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[7, 3]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandBovenEnTotBenedenReedsGevuld_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 1, 1, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 2 0 0 0 0 <
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 1 1 0 0 0
            // 5 0 0 0 1 0 0 0 0
            // 6 0 0 0 1 0 0 0 0
            // 7 0 0 0 1 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 0, 3, out flippedfishes);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[1, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[2, 3]);
            Assert.AreEqual((int)Kleur.Geen, spel.Bord.MapStringBordTo2DIntArr()[0, 3]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandRechts_ReturnTrue()
        {
           // Arrange
           Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
           // 0 1 2 3 4 5 6 7
           // v
           // 0 0 0 0 0 0 0 0 0
           // 1 0 0 0 0 0 0 0 0
           // 2 0 0 0 0 0 0 0 0
           // 3 0 0 0 1 2 0 0 0
           // 4 0 0 0 2 1 1 1 2 <
           // 5 0 0 0 0 0 0 0 0
           // 6 0 0 0 0 0 0 0 0
           // 7 0 0 0 0 0 0 0 0
           // Act
           var actual = _spelMovement.DoeZet(ref spel, 4, 7, out flippedfishes);
           // Assert
           Assert.IsTrue(actual);
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 4 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 5 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 6 && model.Y == 4));

            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 5]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 6]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 7]);
        }
        
        [Test]
        public void DoeZet_ZetAanDeRandRechts_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();

            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 1 0 0 0 0
            // 1 0 0 0 1 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 1 1 1 <
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 4, 7, out flippedfishes);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 5]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 6]);
            Assert.AreEqual((int)Kleur.Geen, spel.Bord.MapStringBordTo2DIntArr()[4, 7]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandRechtsEnTotLinksReedsGevuld_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {2, 1, 1, 1, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 2 1 1 1 1 1 1 2 <
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 4, 7, out flippedfishes);
            // Assert
            Assert.IsTrue(actual);
            Assert.IsTrue(flippedfishes.Any(model => model.X == 0 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 1 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 2 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 4 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 5 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 6 && model.Y == 4));

            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 0]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 1]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 2]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 5]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 6]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 7]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandRechtsEnTotLinksReedsGevuld_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {2, 1, 1, 1, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();

            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 2 1 1 1 1 1 1 1 <
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 4, 7, out flippedfishes);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 0]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 1]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 2]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 5]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 6]);
            Assert.AreEqual((int)Kleur.Geen, spel.Bord.MapStringBordTo2DIntArr()[4, 7]);
        }
        // 0 1 2 3 4 5 6 7
        //
        // 0 0 0 0 0 0 0 0 0
        // 1 0 0 0 0 0 0 0 0
        // 2 0 0 0 0 0 0 0 0
        // 3 0 0 0 1 2 0 0 0
        // 4 0 0 0 2 1 0 0 0
        // 5 0 0 0 0 0 0 0 0
        // 6 0 0 0 0 0 0 0 0
        // 7 0 0 0 0 0 0 0 0
        [Test]
        public void DoeZet_StartSituatieZet22Wit_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 1 0 0 0 0 0 <
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 2, 2, out flippedfishes);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[2, 2]);
        }
        [Test]
        public void DoeZet_StartSituatieZet22Zwart_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 2 0 0 0 0 0 <
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 2, 2, out flippedfishes);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[2, 2]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandRechtsBoven_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 2, 0 },
                {0, 0, 0, 0, 0, 2, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 1, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 1 <
            // 1 0 0 0 0 0 0 2 0
            // 2 0 0 0 0 0 2 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 1 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 0, 7, out flippedfishes);
            // Assert
            Assert.IsTrue(actual);
            Assert.IsTrue(flippedfishes.Any(model => model.X == 2 && model.Y == 5));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 4 && model.Y == 3));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 5 && model.Y == 2));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 6 && model.Y == 1));

            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[5, 2]);
            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[2, 5]);
            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[1, 6]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[0, 7]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandRechtsBoven_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 2, 0 },
                {0, 0, 0, 0, 0, 2, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 1, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 2 <
            // 1 0 0 0 0 0 0 2 0
            // 2 0 0 0 0 0 2 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 1 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 0, 7, out flippedfishes);
            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[1, 6]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[2, 5]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[5, 2]);
            Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[0, 7]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandRechtsOnder_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 2, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 1, 0, 0 },
                {0, 0, 0, 0, 0, 0, 1, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 2 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 1 0 0
            // 6 0 0 0 0 0 0 1 0
            // 7 0 0 0 0 0 0 0 2 <
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 7, 7, out flippedfishes);

            // Assert
            Assert.IsTrue(actual);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[2, 2]);
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 3));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 4 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 5 && model.Y == 5));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 6 && model.Y == 6));

            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[5, 5]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[6, 6]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[7, 7]);
        }

        [Test]
        public void DoeZet_ZetAanDeRandRechtsOnder_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 2, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 1, 0, 0 },
                {0, 0, 0, 0, 0, 0, 1, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 2 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 1 0 0
            // 6 0 0 0 0 0 0 1 0
            // 7 0 0 0 0 0 0 0 1 <
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 7, 7, out flippedfishes);

            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[2, 2]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[5, 5]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[6, 6]);
            Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[7, 7]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandLinksBoven_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 1, 0, 0, 0, 0, 0, 0 },
                {0, 0, 1, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 2, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 2 0 0 0 0 0 0 0 <
            // 1 0 1 0 0 0 0 0 0
            // 2 0 0 1 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 2 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 0, 0, out flippedfishes);

            // Assert
            Assert.IsTrue(actual);
            Assert.IsTrue(flippedfishes.Any(model => model.X == 0 && model.Y == 0));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 1 && model.Y == 1));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 2 && model.Y == 2));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 3));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 4 && model.Y == 4));

            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[0, 0]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[1, 1]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[2, 2]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[5, 5]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandLinksBoven_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 1, 0, 0, 0, 0, 0, 0 },
                {0, 0, 1, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 2, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 1 0 0 0 0 0 0 0 <
            // 1 0 1 0 0 0 0 0 0
            // 2 0 0 1 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 2 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 0, 0, out flippedfishes);

            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[1, 1]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[2, 2]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[5, 5]);
            Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[0, 0]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandLinksOnder_ReturnTrue()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 1, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 2, 0, 0, 0, 0, 0 },
                {0, 2, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 1 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 2 0 0 0 0 0
            // 6 0 2 0 0 0 0 0 0
            // 7 1 0 0 0 0 0 0 0 <
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 7, 0, out flippedfishes);

            // Assert
            Assert.IsTrue(actual);
            Assert.IsTrue(flippedfishes.Any(model => model.X == 0 && model.Y == 7));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 1 && model.Y == 6));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 2 && model.Y == 5));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 3 && model.Y == 4));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 4 && model.Y == 3));

            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[7, 0]);
            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[6, 1]);
            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[5, 2]);
            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            // Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Wit, spel.Bord.MapStringBordTo2DIntArr()[2, 5]);
        }
        [Test]
        public void DoeZet_ZetAanDeRandLinksOnder_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 1, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 2, 0, 0, 0, 0, 0 },
                {0, 2, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();
            // 0 1 2 3 4 5 6 7
            // v
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 1 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 2 0 0 0 0 0
            // 6 0 2 0 0 0 0 0 0
            // 7 2 0 0 0 0 0 0 0 <
            // Act
            var actual = _spelMovement.DoeZet(ref spel, 7, 0, out flippedfishes);

            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[4, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[4, 3]);
            Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[2, 5]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[5, 2]);
            Assert.AreEqual((int)Kleur.Zwart,   spel.Bord.MapStringBordTo2DIntArr()[6, 1]);
            Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[7, 7]);
            Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[7, 0]);
        }

        [Test]
        public void DoeZet_ZetWisseltTweeRijenFichesLinksEnOnder_TweeRijenFichesGedraaid()
         {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 2, 0, 0, 0, 0 },
                {0, 0, 0, 2, 2, 0, 0, 0 },
                {0, 0, 1, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            var flippedfishes = new List<CoordsModel>();

            //   0 1 2 3 4 5 6 7
            //           V
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 2 1 0 0 0 <
            // 3 0 0 0 1 1 0 0 0
            // 4 0 0 1 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            // Act
            var actual = _spelMovement.DoeZet(ref spel, 2, 4, out flippedfishes);


            // Assert
            Assert.IsTrue(actual);
            // Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[3, 3]);
            // Assert.AreEqual((int)Kleur.Wit,     spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            // Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[5, 1]);
            Assert.AreEqual((int)Kleur.Geen,    spel.Bord.MapStringBordTo2DIntArr()[5, 4]);
        }

        [Test]
        public void DoeZet_ZetWisseltTweeRijenFichesRechtsEnOnder_TweeRijenFichesGedraaid()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 1, 1, 0, 0 },
                {0, 0, 0, 2, 2, 2, 2, 0 },
                {0, 0, 0, 0, 2, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
            var flippedfishes = new List<CoordsModel>();

            //   0 1 2 3 4 5 6 7
            //           V
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 2 0 0 0 <
            // 3 0 0 0 1 1 1 0 0
            // 4 0 0 0 2 2 2 2 0
            // 5 0 0 0 0 2 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0

            // Act
            var actual = _spelMovement.DoeZet(ref spel, 2, 4, out flippedfishes);


            // Assert
            Assert.IsTrue(actual);
            Assert.IsTrue(flippedfishes.Any(model => model.X == 4 && model.Y == 3));
            Assert.IsTrue(flippedfishes.Any(model => model.X == 5 && model.Y == 3));

            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 4]);
            // Assert.AreEqual((int)Kleur.Zwart, spel.Bord.MapStringBordTo2DIntArr()[3, 5]);
            Assert.AreEqual((int)Kleur.Geen, spel.Bord.MapStringBordTo2DIntArr()[2, 6]);
        }

        [Test]
        public void Pas_ZwartAanZetGeenZetMogelijk_ReturnTrueEnWisselBeurt()
        {
            // Arrange (zowel wit als zwart kunnen niet meer)
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 1, 1, 0, 0 },
                {0, 0, 0, 2, 2, 2, 2, 0 },
                {0, 0, 0, 0, 2, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Zwart;
           
            // 0 1 2 3 4 5 6 7
            // v
            // 0 1 1 1 1 1 1 1 1
            // 1 1 1 1 1 1 1 1 1
            // 2 1 1 1 1 1 1 1 1
            // 3 1 1 1 1 1 1 1 0
            // 4 1 1 1 1 1 1 0 0
            // 5 1 1 1 1 1 1 0 2
            // 6 1 1 1 1 1 1 1 0
            // 7 1 1 1 1 1 1 1 1
            // Act
            var actual = _spelMovement.Pas(spel);
            // Assert
            Assert.IsTrue(actual);
            Assert.AreEqual((int)Kleur.Wit, spel.AandeBeurt);
        }
       [Test]
        public void Pas_WitAanZetGeenZetMogelijk_ReturnTrueEnWisselBeurt()
        {
            // Arrange (zowel wit als zwart kunnen niet meer)
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 1, 1, 0, 0 },
                {0, 0, 0, 2, 2, 2, 2, 0 },
                {0, 0, 0, 0, 2, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            // v
            // 0 1 1 1 1 1 1 1 1
            // 1 1 1 1 1 1 1 1 1
            // 2 1 1 1 1 1 1 1 1
            // 3 1 1 1 1 1 1 1 0
            // 4 1 1 1 1 1 1 0 0
            // 5 1 1 1 1 1 1 0 2
            // 6 1 1 1 1 1 1 1 0
            // 7 1 1 1 1 1 1 1 1
            // Act
            var actual = _spelMovement.Pas(spel);
            // Assert
            Assert.IsTrue(actual);
            Assert.AreEqual((int)Kleur.Zwart, spel.AandeBeurt);
        }
        [Test]
        public void Afgelopen_GeenZetMogelijk_ReturnTrue()
        {
            // Arrange (zowel wit als zwart kunnen niet meer)
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 0 },
                {1, 1, 1, 1, 1, 1, 0, 0 },
                {1, 1, 1, 1, 1, 1, 0, 2 },
                {1, 1, 1, 1, 1, 1, 1, 0 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            // v
            // 0 1 1 1 1 1 1 1 1
            // 1 1 1 1 1 1 1 1 1
            // 2 1 1 1 1 1 1 1 1
            // 3 1 1 1 1 1 1 1 0
            // 4 1 1 1 1 1 1 0 0
            // 5 1 1 1 1 1 1 0 2
            // 6 1 1 1 1 1 1 1 0
            // 7 1 1 1 1 1 1 1 1
            // Act
            var actual = _spelMovement.Afgelopen(spel);
            // Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void Afgelopen_GeenZetMogelijkAllesBezet_ReturnTrue()
        {
            // Arrange (zowel wit als zwart kunnen niet meer)
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1, 1, 1, 2 },
                {1, 1, 1, 1, 1, 1, 2, 2 },
                {1, 1, 1, 1, 1, 1, 2, 2 },
                {1, 1, 1, 1, 1, 1, 1, 2 },
                {1, 1, 1, 1, 1, 1, 1, 1 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            // v
            // 0 1 1 1 1 1 1 1 1
            // 1 1 1 1 1 1 1 1 1
            // 2 1 1 1 1 1 1 1 1
            // 3 1 1 1 1 1 1 1 2
            // 4 1 1 1 1 1 1 2 2
            // 5 1 1 1 1 1 1 2 2
            // 6 1 1 1 1 1 1 1 2
            // 7 1 1 1 1 1 1 1 1
            // Act
            var actual = _spelMovement.Afgelopen(spel);
            // Assert
            Assert.IsTrue(actual);
        }
        [Test]
        public void Afgelopen_WelZetMogelijk_ReturnFalse()
        {
            // Arrange
            Spel spel = new Spel();
            var bord  = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            //
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            //
            // Act
            var actual = _spelMovement.Afgelopen(spel);
            // Assert
            Assert.IsFalse(actual);
        }
        [Test]
        public void OverwegendeKleur_Gelijk_ReturnKleurGeen()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            //
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 0 0 0 0 0
            // 3 0 0 0 1 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            //
            // Act
            var actual = _spelMovement.OverwegendeKleur(MappingExtensions.FromIntToKleurArray(spel.Bord.MapStringBordTo2DIntArr()));
            // Assert
            Assert.AreEqual(Kleur.Geen, actual);
        }
        [Test]
        public void OverwegendeKleur_Zwart_ReturnKleurZwart()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 2, 0, 0, 0, 0 },
                {0, 0, 0, 2, 2, 0, 0, 0 },
                {0, 0, 0, 2, 1, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            //
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 2 0 0 0 0
            // 3 0 0 0 2 2 0 0 0
            // 4 0 0 0 2 1 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            //
            // Act
            var actual = _spelMovement.OverwegendeKleur(MappingExtensions.FromIntToKleurArray(spel.Bord.MapStringBordTo2DIntArr()));
            // Assert
            Assert.AreEqual(Kleur.Zwart, actual);
        }
        [Test]
        public void OverwegendeKleur_Wit_ReturnKleurWit()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 1, 0, 0, 0, 0 },
                {0, 0, 0, 1, 1, 0, 0, 0 },
                {0, 0, 0, 1, 2, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            //
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 1 0 0 0
            // 4 0 0 0 1 2 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            //
            // Act
            var actual = _spelMovement.OverwegendeKleur(MappingExtensions.FromIntToKleurArray(spel.Bord.MapStringBordTo2DIntArr()));
            // Assert
            Assert.AreEqual(Kleur.Wit, actual);
        }

        [Test]
        public void test()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 2, 2, 2, 1, 0 },
                {0, 0, 0, 2, 2, 2, 0, 0 },
                {0, 0, 0, 1, 1, 2, 1, 0 },
                {0, 0, 0, 0, 0, 2, 2, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            //
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 1 0 0 0
            // 4 0 0 0 1 2 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            //
            // Act
            var coordsList = new List<CoordsModel>();
            var actual = _spelMovement.DoeZet(ref spel, 2, 2, out coordsList);

            // Assert
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[2, 2], (int)Kleur.Wit);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[3, 2], (int)Kleur.Geen);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[2, 3], (int)Kleur.Wit);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[2, 4], (int)Kleur.Wit);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[2, 5], (int)Kleur.Wit);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[3, 3], (int)Kleur.Wit);
        }

        [Test]
        public void test2()
        {
            // Arrange
            Spel spel = new Spel();
            var bord = new int[8, 8]
            {
                {0, 0, 1, 1, 1, 0, 0, 0 },
                {0, 0, 0, 1, 1, 0, 0, 0 },
                {0, 2, 2, 2, 2, 2, 1, 0 },
                {0, 0, 2, 2, 1, 1, 2, 2 },
                {0, 0, 0, 2, 1, 0, 1, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0 },
            };
            spel.Bord = bord.MapIntArrToBase64String();
            spel.AandeBeurt = (int)Kleur.Wit;
            // 0 1 2 3 4 5 6 7
            //
            // 0 0 0 0 0 0 0 0 0
            // 1 0 0 0 0 0 0 0 0
            // 2 0 0 0 1 0 0 0 0
            // 3 0 0 0 1 1 0 0 0
            // 4 0 0 0 1 2 0 0 0
            // 5 0 0 0 0 0 0 0 0
            // 6 0 0 0 0 0 0 0 0
            // 7 0 0 0 0 0 0 0 0
            //
            // Act
            var coordsList = new List<CoordsModel>();
            var actual = _spelMovement.DoeZet(ref spel, 4, 1, out coordsList);

            // Assert
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[4, 1], (int)Kleur.Wit);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[3, 2], (int)Kleur.Wit);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[2, 3], (int)Kleur.Wit);
            /*Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[2, 4], (int)Kleur.Wit);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[2, 5], (int)Kleur.Wit);
            Assert.AreEqual(spel.Bord.MapStringBordTo2DIntArr()[3, 3], (int)Kleur.Wit);*/
        }
    }
}