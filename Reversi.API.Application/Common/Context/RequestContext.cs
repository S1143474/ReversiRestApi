using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Reversi.API.Application.Common.Interfaces;

namespace Reversi.API.Application.Common.Behaviours
{
    public class RequestContext : IRequestContext
    {
        public Guid RequestId { get; set; }
    }
}
