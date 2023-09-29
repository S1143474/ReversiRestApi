using System.Collections.Generic;

namespace Reversi.API.DataTransferObjects.Move
{
    public class MoveExecutedDto : BaseDto
    {
        public int AanDeBeurt { get; set; }
        public bool IsPlaceExecuted { get; set; }
        public string NotExecutedMessage { get; set; }

        public List<CoordsDto> FichesToTurnAround { get; set; }
    }
}
