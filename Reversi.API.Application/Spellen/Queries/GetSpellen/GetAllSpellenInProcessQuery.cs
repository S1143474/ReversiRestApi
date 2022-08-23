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
    public class GetAllSpellenInProcessQuery : IRequest<PagedList<Spel>>
    {
        public QueryStringParameters Params { get; set; }
    }

    public class GetAllSpellenInProcessQueryHandle : IRequestHandler<GetAllSpellenInProcessQuery, PagedList<Spel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetAllSpellenQueryHandle> _logger;

        public GetAllSpellenInProcessQueryHandle(IRequestContext requestContext, ILogger<GetAllSpellenQueryHandle> logger, IRepositoryWrapper repositoryWrapper)
        {
            _logger = logger;
            _requestContext = requestContext;
            _repository = repositoryWrapper;
        }

        public Task<PagedList<Spel>> Handle(GetAllSpellenInProcessQuery request, CancellationToken cancellationToken)
        {
            var spellenInProcess = _repository.Spel.GetAllSpellenInProcess(request.Params);

            if (spellenInProcess == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, the IEnumerable<spel> is null");
                throw new NotFoundException(nameof(PagedList<Spel>), "all");
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded {spellenInProcess.Count} spel in process entities from the database.");

            return Task.FromResult(spellenInProcess);
        }
    }
}
