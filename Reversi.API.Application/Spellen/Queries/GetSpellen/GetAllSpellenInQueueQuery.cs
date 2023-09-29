using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpellen
{
    public class GetAllSpellenInQueueQuery : IRequest<PagedList<Spel>>
    {
        public QueryStringParameters Paramaters { get; set; }
    }

    public class GetAllSpellenInQueueQueryHanlde : IRequestHandler<GetAllSpellenInQueueQuery, PagedList<Spel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetAllSpellenInQueueQuery> _logger;

        public GetAllSpellenInQueueQueryHanlde(IRepositoryWrapper repositoryWrapper, IRequestContext requestContext, ILogger<GetAllSpellenInQueueQuery> logger)
        {
            _repository = repositoryWrapper;
            _requestContext = requestContext;
            _logger = logger;
        }

        public Task<PagedList<Spel>> Handle(GetAllSpellenInQueueQuery request, CancellationToken cancellationToken)
        {
            var spellenInQueue = _repository.Spel.GetAllSpellenInQueue(request.Paramaters);

            if (spellenInQueue == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, the IEnumerable<spel> is null");
                throw new NotFoundException(nameof(PagedList<Spel>), "all");
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded {spellenInQueue.Count} in queue spel entities from the database.");

            return Task.FromResult(spellenInQueue);
        }
    }
}
