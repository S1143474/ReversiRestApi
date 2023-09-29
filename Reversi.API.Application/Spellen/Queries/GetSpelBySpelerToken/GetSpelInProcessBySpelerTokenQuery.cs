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
using Reversi.API.Application.Spellen.Queries.GetSpel;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application.Spellen.Queries.GetSpelBySpelerToken
{
    public class GetSpelInProcessBySpelerTokenQuery : IRequest<Spel>
    {
        [Required(ErrorMessage = "SpelerToken is required")]
        public Guid SpelerToken { get; set; }
    }

    public class GetSpelInProcessBySpelerTokenQueryHandle : IRequestHandler<GetSpelInProcessBySpelerTokenQuery, Spel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetSpelInProcessBySpelerTokenQueryHandle> _logger;

        public GetSpelInProcessBySpelerTokenQueryHandle(IRequestContext requestContext, IRepositoryWrapper repositoryWrapper, ILogger<GetSpelInProcessBySpelerTokenQueryHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }

        public async Task<Spel> Handle(GetSpelInProcessBySpelerTokenQuery request, CancellationToken cancellationToken)
        {
            var spel = await _repository.Spel.GetSpelInProcessBySpelerTokenAsync(request.SpelerToken);

            if (spel == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, in process spel is null, request parameter: Spelertoken: {request.SpelerToken}");
                throw new NotFoundException(nameof(Spel), request.SpelerToken);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 spel entity from the database with token: {spel.Token}.");

            return spel;
        }
    }
}
