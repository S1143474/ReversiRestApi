using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Application.Common.RequestParameters
{
    public class FinishedSpelParameters : QueryStringParameters
    {
        public FinishedSpelParameters() : base()
        {
            OrderBy = "FinishedAt desc";
        }
    }
}
