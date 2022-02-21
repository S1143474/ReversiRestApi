using System;
using System.Collections.Generic;
using System.Text;
using Reversi.API.Application.Common.Interfaces;

namespace Reversi.API.Infrastructure.Maps
{
    public class FicheSeriesMap : IFiche
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
