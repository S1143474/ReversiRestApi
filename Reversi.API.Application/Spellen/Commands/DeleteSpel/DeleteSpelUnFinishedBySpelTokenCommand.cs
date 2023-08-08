using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Spellen.Commands.SurrenderSpel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reversi.API.Application.Spellen.Commands.DeleteSpel
{
    public class DeleteSpelUnFinishedBySpelTokenCommand : IRequest<bool>
    {
        public Guid SpelToken { get; set; }
    }

    public class DeleteSpelUnFinishedBySpelTokenCommandHandle : IRequestHandler<DeleteSpelUnFinishedBySpelTokenCommand, bool>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<DeleteSpelUnFinishedBySpelTokenCommandHandle> _logger;

        public DeleteSpelUnFinishedBySpelTokenCommandHandle(IRepositoryWrapper repository, ILogger<DeleteSpelUnFinishedBySpelTokenCommandHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task<bool> Handle(DeleteSpelUnFinishedBySpelTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Started Deletion of Spel: {request.SpelToken}");

            var result = _repository.Spel.DeleteSpel(request.SpelToken);

            if (result == false )
            {
                _logger.LogInformation($"Deletion of Spel: {request.SpelToken} failed");
                return Task.FromResult(false);
            } else
            {
                _logger.LogInformation($"Deletion of Spel: {request.SpelToken} Succeeded");
                return Task.FromResult(true);

            }
        }
    }
}