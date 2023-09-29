using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Application.Common.Interfaces
{
    public interface IRequestContext
    {
        Guid RequestId { get; set; }
    }
}
