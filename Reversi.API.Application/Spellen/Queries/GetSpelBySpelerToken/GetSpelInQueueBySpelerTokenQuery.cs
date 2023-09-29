using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpelBySpelerToken
{
    public class GetSpelInQueueBySpelerTokenQuery : IRequest<Spel>
    {
        [Required(ErrorMessage = "SpelerToken is required")]
        public Guid SpelerToken { get; set; }
    }

    public class GetSpelInQueueBySpelerTokenQueryHanlde : IRequestHandler<GetSpelInQueueBySpelerTokenQuery, Spel> {

        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetSpelInQueueBySpelerTokenQueryHanlde> _logger;

        public GetSpelInQueueBySpelerTokenQueryHanlde(IRequestContext requestContext, IRepositoryWrapper repositoryWrapper, ILogger<GetSpelInQueueBySpelerTokenQueryHanlde> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }
        public async Task<Spel> Handle(GetSpelInQueueBySpelerTokenQuery request, CancellationToken cancellationToken)
        {
            var spelInQueue = await _repository.Spel.GetSpelInQueueBySpelerTokenAsync(request.SpelerToken);

            if (spelInQueue == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, in queue spel is null, request parameter: Spelertoken: {request.SpelerToken}");
                throw new NotFoundException(nameof(Spel), request.SpelerToken);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 in queue spel entity from the database with token: {spelInQueue.Token}.");

            return spelInQueue;
        }
    }
}
