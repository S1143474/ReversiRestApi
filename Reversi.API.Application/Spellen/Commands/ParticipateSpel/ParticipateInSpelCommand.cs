using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Spellen.Queries.GetSpel;
using Reversi.API.Domain.Common.Exceptions;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Commands.ParticipateSpel
{
    public class ParticipateInSpelCommand : IRequest
    {
        public Guid Token { get; set; }
        public Guid Speler2Token { get; set; }
    }

    public class ParticipateInSpelCommandHandle : IRequestHandler<ParticipateInSpelCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<ParticipateInSpelCommandHandle> _logger;

        public ParticipateInSpelCommandHandle(IRequestContext requestContext, IRepositoryWrapper repositoryWrapper, ILogger<ParticipateInSpelCommandHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }
        public async Task<Unit> Handle(ParticipateInSpelCommand request, CancellationToken cancellationToken)
        {
            var spel = await _repository.Spel.GetSpelInQueueByIdAsync(request.Token);

            if (spel == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, in queue spel is null, request parameter: Speltoken: {request.Token}");
                throw new NotFoundException(nameof(Spel), request.Token);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 spel entity from the database with token: {spel.Token}");

            if (spel.Speler1Token.Equals(request.Speler2Token))
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, Speler2 tried to join his own game. Speler1: {spel.Speler1Token} - Speler2: {request.Speler2Token} with token: {spel.Token}");
                throw new SelfParticipationException(spel.Token, spel.Speler1Token, request.Speler2Token);
            }

            spel.Speler2Token = spel.UpdatedBy = request.Speler2Token;
            spel.StartedAt = spel.UpdatedAt = DateTime.Now.ToUniversalTime();

            _repository.Spel.Update(spel);
            await _repository.SaveAsync();

            _logger.LogInformation(
                $"Request with id: {_requestContext.RequestId}, User: {spel.Speler2Token} Started a spel with token: {spel.Token}");

            return Unit.Value;
        }
    }
}
