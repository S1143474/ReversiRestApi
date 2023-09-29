using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Application.Spellen.Commands.InProcessSpelMove.MoveModels
{
    public class FinishedModel : BaseMoveModel
    {
        public bool IsSpelFinished { get; set; }
        public bool IsSpelDraw { get; set; }
        public Guid WinnerToken { get; set; }
        public Guid LoserToken { get; set; }
    }
}
