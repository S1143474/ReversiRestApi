using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Reversi.API.Application.Common.Exceptions;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Domain.Common.Exceptions;

namespace Reversi.API.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
/*        private readonly IRequestContext _requestContext;
*/        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
/*            _requestContext = requestContext;
*/            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DefaultGuidException dgex)
            {
                _logger.LogError($"A new default guid exception has been thrown by request id: (id), exception: {dgex}");
                await HandleExceptionAsync(httpContext, dgex);
            }
            catch (NotFoundException nfEx)
            {
                _logger.LogError($"A new not found exception has been thrown by request id: (id), exception: {nfEx}");
                await HandleExceptionAsync(httpContext, nfEx);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var message = exception switch
            {
                DefaultGuidException => "A token cannot be default.",
                SelfParticipationException => "A player cannot participate in a game created by itself.",
                NotFoundException => "Spel was not found.",
                _ => "Internal server error."
            };
            ;

            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = message 
            }.ToString());
        }
    }
}
