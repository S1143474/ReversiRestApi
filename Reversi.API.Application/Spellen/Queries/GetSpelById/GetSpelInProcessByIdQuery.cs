using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Spellen.Queries.GetSpellen;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpel
{
    public class GetSpelInProcessByIdQuery : IRequest<Spel>
    {
        public Guid Token { get; set; }
    }

    public class GetSpelInProcessByIdQueryHanlde : IRequestHandler<GetSpelInProcessByIdQuery, Spel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetAllSpellenQueryHandle> _logger;

        public GetSpelInProcessByIdQueryHanlde(IRequestContext requestContext, ILogger<GetAllSpellenQueryHandle> logger, IRepositoryWrapper repositoryWrapper)
        {
            _logger = logger;
            _requestContext = requestContext;
            _repository = repositoryWrapper;
        }

        public async Task<Spel> Handle(GetSpelInProcessByIdQuery request, CancellationToken cancellationToken)
        {
            var spel = await _repository.Spel.GetSpelInProcessByIdAsync(request.Token);

            if (spel == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, in process spel is null, request parameter: Speltoken: {request.Token}");
                throw new NotFoundException(nameof(Spel), request.Token);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 in process spel entity from the database with token: {spel.Token}.");

            return spel;
        }
    }
}
