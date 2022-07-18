using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Interfaces;

namespace Reversi.API.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : BaseBehaviour, IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(IRequestContext behaviourContext, ILogger<TRequest> logger) : base(behaviourContext)
        {
            _timer = new Stopwatch();

            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= 500)
                return response;

            var requestName = typeof(TRequest).Name;

            _logger.LogWarning($"Long Running Request: request id: {BehaviourContext.RequestId}, request name: {requestName}, requested by user: UNK, request: {request}, total request time: {elapsedMilliseconds}ms");

            return response;
        }
    }
}
