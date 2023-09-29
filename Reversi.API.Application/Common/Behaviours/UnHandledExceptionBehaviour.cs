using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Interfaces;

namespace Reversi.API.Application.Common.Behaviours
{
    public class UnHandledExceptionBehaviour<TRequest, TResponse> : BaseBehaviour, IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnHandledExceptionBehaviour(IRequestContext behaviourContext, ILogger<TRequest> logger) : base(behaviourContext)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, $"Unhandled Exception for Request id: {BehaviourContext.RequestId}, request name: {requestName}, request: {request}");

                throw;
            }
        }
    }
}
