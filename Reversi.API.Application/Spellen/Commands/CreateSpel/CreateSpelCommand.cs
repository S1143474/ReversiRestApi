using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Commands.CreateSPel
{
    public class CreateSpelCommand : IRequest<Spel>
    {
        public Spel Spel { get; set; }
    }

    public class CreateSpelCommandHandle : IRequestHandler<CreateSpelCommand, Spel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<CreateSpelCommandHandle> _logger;

        public CreateSpelCommandHandle(IRepositoryWrapper repositoryWrapper, IRequestContext requestContext,
            ILogger<CreateSpelCommandHandle> logger)
        {
            _repository = repositoryWrapper;
            _requestContext = requestContext;
            _logger = logger;
        }

        public async Task<Spel> Handle(CreateSpelCommand request, CancellationToken cancellationToken)
        {
            var spel = request.Spel; 

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

            spel.Token = Guid.NewGuid();
            spel.Bord = bord.MapIntArrToBase64String();
            spel.CreatedAt = DateTime.Now.ToUniversalTime();
            spel.AandeBeurt = 2;

            _repository.Spel.Create(spel);
            await _repository.SaveAsync();

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Created a new spel for a player with id: {request.Spel.Speler1Token}");
            return spel;
        }
    }
}
