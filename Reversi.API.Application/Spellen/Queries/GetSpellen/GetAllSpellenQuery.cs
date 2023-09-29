using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Reversi.API.Application.Common.Behaviours;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Application.Common.RequestParameters;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpellen
{
    public class GetAllSpellenQuery : IRequest<PagedList<Spel>>
    {
        public QueryStringParameters Paramaters { get; set; }
    }

    public class GetAllSpellenQueryHandle : IRequestHandler<GetAllSpellenQuery, PagedList<Spel>> 
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetAllSpellenQueryHandle> _logger;

        public GetAllSpellenQueryHandle(IRequestContext requestContext, ILogger<GetAllSpellenQueryHandle> logger, IRepositoryWrapper repositoryWrapper)
        {
            _logger = logger;
            _requestContext = requestContext;
            _repository = repositoryWrapper;
        }

        public Task<PagedList<Spel>> Handle(GetAllSpellenQuery request, CancellationToken cancellationToken)
        {   
            var spellen = _repository.Spel.GetAllSpellen(request.Paramaters);

            if (spellen == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, the IEnumerable<spel> is null");
                throw new NotFoundException(nameof(PagedList<Spel>), "all");
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded {spellen.Count()} spel entities from the database.");

            return Task.FromResult(spellen);
        }
    }
}
