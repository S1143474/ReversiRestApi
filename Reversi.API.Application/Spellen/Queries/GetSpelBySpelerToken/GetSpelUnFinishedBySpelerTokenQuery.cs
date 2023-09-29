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
    public class GetSpelUnFinishedBySpelerTokenQuery : IRequest<Spel>
    {
        [Required(ErrorMessage = "SpelerToken is required")]
        public Guid SpelerToken { get; set; }
    }

    public class GetSpelUnFinishedBySpelerTokenQueryHandle : IRequestHandler<GetSpelUnFinishedBySpelerTokenQuery, Spel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IRequestContext _requestContext;
        private readonly ILogger<GetSpelUnFinishedBySpelerTokenQueryHandle> _logger;

        public GetSpelUnFinishedBySpelerTokenQueryHandle(IRequestContext requestContext, IRepositoryWrapper repositoryWrapper, ILogger<GetSpelUnFinishedBySpelerTokenQueryHandle> logger)
        {
            _repository = repositoryWrapper;
            _logger = logger;
            _requestContext = requestContext;
        }

        public async Task<Spel> Handle(GetSpelUnFinishedBySpelerTokenQuery request, CancellationToken cancellationToken)
        {
            var spelUnFinished = await _repository.Spel.GetSpelUnFinishedBySpelerTokenAsync(request.SpelerToken);

            if (spelUnFinished == null)
            {
                _logger.LogError($"Error for request id: {_requestContext.RequestId}, unfinished spel is null, request parameter: Spelertoken: {request.SpelerToken}");
                throw new NotFoundException(nameof(Spel), request.SpelerToken);
            }

            _logger.LogInformation($"Request with id: {_requestContext.RequestId}, Loaded 1 unfinished spel entity from the database with token: {spelUnFinished.Token}.");

            return spelUnFinished;
        }
    }
}
