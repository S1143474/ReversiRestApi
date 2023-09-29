using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Application.Spellen.Queries.GetSpel;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpelBySpelerToken
{
    public class GetSpellenFinishedBySpelerTokenQuery: IRequest<PagedList<Spel>>
    {
        [Required(ErrorMessage = "SpelerToken is required")]
        public Guid SpelerToken { get; set; }

        public QueryStringParameters Parameters { get; set; }
    }

    public class GetSpellenFinishedBySpelerTokenQueryHandle : IRequestHandler<GetSpellenFinishedBySpelerTokenQuery, PagedList<Spel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetSpellenFinishedBySpelerTokenQueryHandle> _logger;

        public GetSpellenFinishedBySpelerTokenQueryHandle(IRequestContext requestContext, IRepositoryWrapper repositoryWrapper, ILogger<GetSpellenFinishedBySpelerTokenQueryHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }

        public Task<PagedList<Spel>> Handle(GetSpellenFinishedBySpelerTokenQuery request, CancellationToken cancellationToken)
        {
            var spellenFinished = _repository.Spel.GetSpellenFinishedBySpelerTokenAsync(request.SpelerToken, request.Parameters);

            if (spellenFinished == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, the IEnumerable<spel> is null");
                throw new NotFoundException(nameof(PagedList<Spel>), request.SpelerToken);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded {spellenFinished.Count} finished spel entities from the database.");

            return Task.FromResult(spellenFinished);
        }
    }
}
