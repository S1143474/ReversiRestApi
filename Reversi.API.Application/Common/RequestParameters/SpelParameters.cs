using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Application.Common.RequestParameters
{
    public class SpelParameters : QueryStringParameters
    {
        public SpelParameters() : base()
        {
            OrderBy = "omschrijving";
        }
    }
}
