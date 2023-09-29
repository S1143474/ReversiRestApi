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
    public class LoggingBehaviour<TRequest, TResponse> : BaseBehaviour, IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(IRequestContext behaviourContext, ILogger<TRequest> logger) 
            : base(behaviourContext)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var uniqueId = BehaviourContext.RequestId = Guid.NewGuid();

            string userName = string.Empty;

            _logger.LogInformation(
                $"Begin Request Id: {uniqueId}, request name: {requestName}, requested by user: UNK, request: {request}");
            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();

            _logger.LogInformation(
                $"End Request Id: {uniqueId}, request name: {requestName}, requested by user: UNK, total request time: {timer.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
