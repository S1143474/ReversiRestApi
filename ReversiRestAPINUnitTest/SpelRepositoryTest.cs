using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Controllers;
using Reversi.API.Domain.Entities;
using Reversi.API.Infrastructure.Persistence;
using Reversi.API.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reversi.API.UnitTests
{
    [TestFixture]
    public class SpelRepositoryTest
    {
        private readonly string _board = new int[8, 8]
                   {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 1, 2, 0, 0, 0 },
                        { 0, 0, 0, 2, 1, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                   }.MapIntArrToBase64String();

        private RepositoryContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var databaseContext = new RepositoryContext(options);
            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();
            databaseContext.Spellen.AsNoTracking();
            if (databaseContext.Spellen.Count() <= 0)
            {
                databaseContext.Add(new Spel
                {
                    Token = Guid.NewGuid(),
                    Omschrijving = "Potje Reversi",
                    Bord = _board,
                    StartedAt = DateTime.Now.AddHours(-7),

                });

                databaseContext.SaveChanges();
            }

            return databaseContext;
        }

        private RepositoryContext _dbContext;
        private RepositoryWrapper _wrapper;

        public RepositoryWrapper RepoWrapper
        {
            get
            {
                return _wrapper ??= new RepositoryWrapper(RepoContext, new SortHelper<Spel>());
            }
        }
        public RepositoryContext RepoContext
        {
            get
            {
                return _dbContext ??= GetDbContext();
            }
        }

        [TearDown]
        public async Task AfterTest()
        {

            foreach (var entity in RepoContext.Spellen)
            {
                RepoContext.Spellen.Remove(entity);
            }

            await RepoContext.SaveChangesAsync();
        }

        [Test]
        public async Task SpelRepository_Create_spelhasbeenadded()
        {
            // Arrange
            var token = Guid.NewGuid();
            var spel = new Spel
            {
                Token = token,
                Omschrijving = "Test Potje",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String()

            };

            // Act
            RepoWrapper.Spel.Create(spel);
            await RepoWrapper.SaveAsync();

            var result = await RepoWrapper.Spel.GetSpelByIdAsync(token);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(spel.Token, result.Token);
            Assert.AreEqual(spel.Omschrijving, result.Omschrijving);

           
        }

        [Test]
        public async Task SpelRepository_GetById_SpelExist()
        {
            // Arrange
            var token = Guid.NewGuid();
            var spel = new Spel
            {
                Token = token,
                Omschrijving = "Test Potje Test",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String()

            };

            // Act
            RepoWrapper.Spel.Create(spel);
            await RepoWrapper.SaveAsync();

            var result = await RepoWrapper.Spel.GetSpelByIdAsync(token);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(spel.Token, result.Token);
            Assert.AreEqual(spel.Omschrijving, result.Omschrijving);
        }

        [Test]
        public void SpelRepository_GetAllSpellen_ReturnsPagedList()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repoWrapper = new RepositoryWrapper(dbContext, new SortHelper<Spel>());

            // Create some sample Spel entities in the database
            var spel1 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 1",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.AddHours(-3)
            };
            var spel2 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 2",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.AddHours(-2)
            };
            var spel3 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 3",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.AddHours(-1)
            };

            repoWrapper.Spel.Create(spel1);
            repoWrapper.Spel.Create(spel2);
            repoWrapper.Spel.Create(spel3);
            repoWrapper.Save();

            var queryStringParameters = new SpelParameters
            {
                PageNumber = 1,
                PageSize = 2,
                OrderBy = "StartedAt"
            };

            var queryStringParameters2 = new SpelParameters
            {
                PageNumber = 2,
                PageSize = 2,
                OrderBy = "StartedAt"
            };

            // Act
            var result = repoWrapper.Spel.GetAllSpellen(queryStringParameters);
            var result2 = repoWrapper.Spel.GetAllSpellen(queryStringParameters2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // Check if the correct number of Spel entities is returned based on PageSize
            Assert.AreEqual(4, result.TotalCount); // Check if the total count is correct
            Assert.AreEqual(spel3.Token, result2[1].Token); // Check if Spel 3 is the first in the sorted list
            Assert.AreEqual(spel2.Token, result2[0].Token); // Check if Spel 2 is the second in the sorted list
        }

        [Test]
        public void SpelRepository_GetAllSpellenInQueue_ReturnsPagedList()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repoWrapper = new RepositoryWrapper(dbContext, new SortHelper<Spel>());

            // Create some sample Spel entities in the database
            var spel1 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 1",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = null,
                Speler2Token = null
            };
            var spel2 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 2",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = DateTime.Now,
                Speler2Token = null
            };
            var spel3 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 3",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = null,
                Speler2Token = Guid.NewGuid()
            };

            repoWrapper.Spel.Create(spel1);
            repoWrapper.Spel.Create(spel2);
            repoWrapper.Spel.Create(spel3);
            repoWrapper.Save();

            var queryStringParameters = new SpelParameters
            {
                PageNumber = 1,
                PageSize = 2,
                OrderBy = "Omschrijving" // Order by Omschrijving for testing purposes
            };

            // Act
            var result = repoWrapper.Spel.GetAllSpellenInQueue(queryStringParameters);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count); // Only one Spel meets the criteria
            Assert.AreEqual(1, result.TotalCount); // Total count should reflect the filtered count
            Assert.AreEqual(spel1.Token, result[0].Token); // Check if the correct Spel is returned
        }

        [Test]
        public void SpelRepository_GetAllSpellenInProcess_ReturnsPagedList()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repoWrapper = new RepositoryWrapper(dbContext, new SortHelper<Spel>());

            // Create some sample Spel entities in the database
            var spel1 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 1",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.AddHours(-3),
                FinishedAt = null
            };
            var spel2 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 2",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.AddHours(-2),
                FinishedAt = DateTime.Now.AddHours(-1)
            };
            var spel3 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 3",
                Bord = new int[8, 8]
                    {
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 1, 2, 0, 0, 0 },
                        {0, 0, 0, 2, 1, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                        {0, 0, 0, 0, 0, 0, 0, 0 },
                    }.MapIntArrToBase64String(),
                StartedAt = DateTime.Now.AddHours(-1),
                FinishedAt = null
            };

            repoWrapper.Spel.Create(spel1);
            repoWrapper.Spel.Create(spel2);
            repoWrapper.Spel.Create(spel3);
            repoWrapper.Save();

            var queryStringParameters = new SpelParameters
            {
                PageNumber = 0,
                PageSize = 2,
                OrderBy = "StartedAt" // Order by StartedAt for testing purposes
            };

            var queryStringParameters2 = new SpelParameters
            {
                PageNumber = 2,
                PageSize = 2,
                OrderBy = "StartedAt" // Order by StartedAt for testing purposes
            };

            // Act
            var result = repoWrapper.Spel.GetAllSpellenInProcess(queryStringParameters);
            var result2 = repoWrapper.Spel.GetAllSpellenInProcess(queryStringParameters2);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // Two Spel entities meet the criteria
            Assert.AreEqual(3, result.TotalCount); // Total count should reflect the filtered count
            Assert.AreEqual(spel3.Token, result2[0].Token); // Check if the correct Spel is returned first based on StartedAt
            Assert.AreEqual(spel1.Token, result[1].Token); // Check if the correct Spel is returned second based on StartedAt
        }

        [Test]
        public void SpelRepository_GetAllSpellenFinished_ReturnsPagedList()
        {
            // Arrange
            var dbContext = GetDbContext();
            var repoWrapper = new RepositoryWrapper(dbContext, new SortHelper<Spel>());

            // Create some sample Spel entities in the database
            var spel1 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 1",
                Bord = _board,/* valid Bord data here */
                FinishedAt = DateTime.Now.AddHours(-3)
            };
            var spel2 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 2",
                Bord = _board,/* valid Bord data here */
                FinishedAt = DateTime.Now.AddHours(-2)
            };
            var spel3 = new Spel
            {
                Token = Guid.NewGuid(),
                Omschrijving = "Spel 3",
                Bord = _board,/* valid Bord data here */
                FinishedAt = DateTime.Now.AddHours(-1)
            };

            repoWrapper.Spel.Create(spel1);
            repoWrapper.Spel.Create(spel2);
            repoWrapper.Spel.Create(spel3);
            repoWrapper.Save();

            var queryStringParameters = new SpelParameters
            {
                PageNumber = 1,
                PageSize = 2,
                OrderBy = "FinishedAt" // Order by FinishedAt for testing purposes
            };

            // Act
            var result = repoWrapper.Spel.GetAllSpellenFinished(queryStringParameters);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // Two Spel entities meet the criteria
            Assert.AreEqual(3, result.TotalCount); // Total count should reflect the total number of finished Spel entities
            Assert.AreEqual(spel1.Token, result[0].Token); // Check if the correct Spel is returned first based on FinishedAt
            Assert.AreEqual(spel2.Token, result[1].Token); // Check if the correct Spel is returned second based on FinishedAt
        }





        /*        #region GetSpellen()

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
                }*/
        //#endregion
    }
}
