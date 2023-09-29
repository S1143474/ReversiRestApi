using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Application
{
    public class DBSpel
    {
        public int GUID { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public string Bord { get; set; }
        public int Beurt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
    }
}
