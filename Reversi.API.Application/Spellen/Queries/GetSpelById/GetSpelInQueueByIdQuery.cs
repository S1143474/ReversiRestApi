using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpel
{
    public class GetSpelInQueueByIdQuery : IRequest<Spel>
    {
        public Guid Id { get; set; }
    }

    public class GeSpelInQueueByIdQueryHanlde : IRequestHandler<GetSpelInQueueByIdQuery, Spel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetSpelByIdQueryHandle> _logger;

        public GeSpelInQueueByIdQueryHanlde(IRequestContext requestContext, IRepositoryWrapper repositoryWrapper, ILogger<GetSpelByIdQueryHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }

        public async Task<Spel> Handle(GetSpelInQueueByIdQuery request, CancellationToken cancellationToken)
        {
            var spelInQueue = await _repository.Spel.GetSpelInQueueByIdAsync(request.Id);

            if (spelInQueue == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, in queue spel is null, request parameter: Speltoken: {request.Id}");
                throw new NotFoundException(nameof(Spel), request.Id);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 in queue spel entity from the database with id: {spelInQueue.Token}.");

            return spelInQueue;
        }
    }
}
