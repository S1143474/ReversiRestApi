using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Spellen.Commands.ParticipateSpel;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Commands.SurrenderSpel
{
    public class SurrenderSpelCommand : IRequest
    {
        public Guid Token { get; set; }
        public Guid SpelerToken { get; set; }
    }

    public class SurrenderSpelCommandHandle : IRequestHandler<SurrenderSpelCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<SurrenderSpelCommandHandle> _logger;

        public SurrenderSpelCommandHandle(IRequestContext requestContext, IRepositoryWrapper repositoryWrapper, ILogger<SurrenderSpelCommandHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }

        public async Task<Unit> Handle(SurrenderSpelCommand request, CancellationToken cancellationToken)
        {
            if (request.SpelerToken.Equals(default(Guid)))
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, spelertoken is a default GUID, request parameter: spelerToken: {request.SpelerToken}, token: {request.Token}");
                throw new DefaultGuidException(
                    $"A default guid has been entered for spelertoken with spel token: {request.Token}");
            }

            var spelToSurrender = await _repository.Spel.GetSpelInProcessFromSpelerOrSpelTokenAsync(request.SpelerToken, request.Token);

            if (spelToSurrender == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, in process spel to surrender is null, request parameter: spelerToken: {request.SpelerToken}, token: {request.Token}");
                throw new NotFoundException(nameof(Spel), request.SpelerToken);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 in process spel entity from the database with token: {spelToSurrender.Token}");

            spelToSurrender.FinishedAt = spelToSurrender.UpdatedAt = DateTime.Now;
            spelToSurrender.AmountOfUpdates++;
            spelToSurrender.UpdatedBy = request.SpelerToken;
            
            if (request.SpelerToken == spelToSurrender.Speler1Token)
            {
                _logger.LogInformation($"Request with id: {_requestContext.RequestId}, player with spelertoken: {request.SpelerToken} surrendered the game with id: {spelToSurrender.Token}, making player2 with token: {spelToSurrender.Speler2Token} the winner.");

                spelToSurrender.WonBy = spelToSurrender.Speler2Token;
                spelToSurrender.LostBy = spelToSurrender.Speler1Token;

                _repository.Spel.Update(spelToSurrender);
                await _repository.SaveAsync();
                return Unit.Value;
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, player with spelertoken: {request.SpelerToken} surrendered the game with id: {spelToSurrender.Token}, making player1 with token: {spelToSurrender.Speler1Token} the winner.");
            spelToSurrender.WonBy = spelToSurrender.Speler1Token;
            spelToSurrender.LostBy = spelToSurrender.Speler2Token;

            _repository.Spel.Update(spelToSurrender);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
