using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Application.Spellen.Queries.GetSpel;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpellen
{
    public class GetAllSpellenFinishedQuery : IRequest<PagedList<Spel>>
    {
        public QueryStringParameters Paramameters { get; set; }
    }

    public class GetAllSpellenFinishedQueryHandle : IRequestHandler<GetAllSpellenFinishedQuery, PagedList<Spel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetAllSpellenFinishedQueryHandle> _logger;

        public GetAllSpellenFinishedQueryHandle(
            IRequestContext requestContext,
            IRepositoryWrapper repositoryWrapper,
            ILogger<GetAllSpellenFinishedQueryHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }

        public Task<PagedList<Spel>> Handle(GetAllSpellenFinishedQuery request, CancellationToken cancellationToken)
        {
            var spellenFinished = _repository.Spel.GetAllSpellenFinished(request.Paramameters);

            if (spellenFinished == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, the IEnumerable<spel> is null");
                throw new NotFoundException(nameof(PagedList<Spel>), "all");
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded {spellenFinished.Count} finished spel entities from the database.");

            return Task.FromResult(spellenFinished);
        }
    }
}
