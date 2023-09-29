using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Behaviours;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Spellen.Queries.GetSpellen;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpel
{
    public class GetSpelByIdQuery : IRequest<Spel>
    {
        public Guid Id { get; set; }
    }

    public class GetSpelByIdQueryHandle : IRequestHandler<GetSpelByIdQuery, Spel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetSpelByIdQueryHandle> _logger;

        public GetSpelByIdQueryHandle(IRequestContext requestContext, IRepositoryWrapper repositoryWrapper, ILogger<GetSpelByIdQueryHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }

        public async Task<Spel> Handle(GetSpelByIdQuery request, CancellationToken cancellationToken)
        {
            var spel = await _repository.Spel.GetSpelByIdAsync(request.Id);
            
            if (spel == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, spel is null, request parameter: Speltoken: {request.Id}");
                throw new NotFoundException(nameof(Spel), request.Id);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 spel entity from the database with token: {spel.Token}.");

            return spel;
        }
    }
}
