using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReversiRestApi.DataTransferModels;

namespace ReversiRestApi.Json_obj
{
    public class PutDoMoveExecutedJsonObj : BaseTransferModel
    {
        public int AanDeBeurt { get; set; }
        public bool IsPlaceExecuted { get; set; }
        public string NotExecutedMessage { get; set; }

        public List<CoordsJsonObj> FichesToTurnAround { get; set; }
    }
}
