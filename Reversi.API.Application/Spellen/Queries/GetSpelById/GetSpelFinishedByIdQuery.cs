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
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpelById
{
    public class GetSpelFinishedByIdQuery : IRequest<Spel>
    {
        public Guid Token { get; set; }
    }

    public class GetSpelFinishedByIdQueryHandle : IRequestHandler<GetSpelFinishedByIdQuery, Spel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetSpelByIdQueryHandle> _logger;

        public GetSpelFinishedByIdQueryHandle(
            IRequestContext requestContext,
            IRepositoryWrapper repositoryWrapper,
            ILogger<GetSpelByIdQueryHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }

        public async Task<Spel> Handle(GetSpelFinishedByIdQuery request, CancellationToken cancellationToken)
        {
            var finishedSpel = await _repository.Spel.GetSpelFinishedByIdAsync(request.Token);

            if (finishedSpel == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, finished spel is null, request parameter: Speltoken: {request.Token}");
                throw new NotFoundException(nameof(Spel), request.Token);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 finished spel entity from the database with token: {finishedSpel.Token}.");

            return finishedSpel;
        }
    }
}
