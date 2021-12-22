using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Json_obj
{
    public class PutDoMoveExecutedJsonObj
    {
        public bool Executed { get; set; }

        public List<CoordsJsonObj> Cells { get; set; }
    }
}
