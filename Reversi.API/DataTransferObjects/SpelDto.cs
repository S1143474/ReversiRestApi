using System;
using System.Collections.Generic;

namespace Reversi.API.DataTransferObjects
{
    public class SpelDto
    {
        public Guid Token { get; set; }

        public string Description { get; set; }

        public Guid Speler1Token { get; set; }

        public Guid? Speler2Token { get; set; }

        public List<List<int>> Bord { get; set; }

        public int Turn { get; set; }
    }
}
