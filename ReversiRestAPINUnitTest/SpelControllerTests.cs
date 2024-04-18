using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Controllers;
using Reversi.API.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.API.UnitTests
{
    [TestFixture]
    public class SpelControllerTests
    {
        private Mock<ILogger<SpelController>> _mockLogger;
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<SpelController>>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Test]
        public async Task CreateSpelAsync_ValidInput_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var controller = new SpelController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);

            var spelDto = new SpelCreationDto { /* provide valid values for SpelCreationDto */ };

            // Act
            var result = await controller.CreateSpelAsync(spelDto) as CreatedAtRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("SpelById", result.RouteName);
            Assert.AreNotEqual(Guid.Empty, result.RouteValues["id"]);
            Assert.IsInstanceOf<SpelDto>(result.Value);
        }

        [Test]
        public async Task CreateSpelAsync_InvalidInput_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new SpelController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);

            // Ensure that spelDto is invalid for testing bad request scenario
            var spelDto = new SpelCreationDto { /* provide invalid values for SpelCreationDto */ };

            // Act
            var result = await controller.CreateSpelAsync(spelDto);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}
