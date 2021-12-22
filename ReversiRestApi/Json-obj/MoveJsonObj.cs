using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Json_obj
{
    public class MoveJsonObj
    {
        public bool HasPassed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Token { get; set; }
        public string SpelerToken { get; set; }
    }
}
