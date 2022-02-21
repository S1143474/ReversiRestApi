using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Domain.Entities
{
    public class SpelList
    {
        public IList<Spel> Spellen { get; private set; } = new List<Spel>();
    }
}
