using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Reversi.API.Application.Spellen.Commands.InProcessSpelMove.MoveModels
{
    public class MoveExecutedModel : BaseMoveModel
    {
        public int PlayerTurn { get; set; }
        public bool IsMovementExecuted { get; set; }
        public string NotExecutedMessage { get; set; }
        
        public List<CoordsModel> CoordsToTurnAround { get; set; }
    }
}
