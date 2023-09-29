using System;

namespace Reversi.API.DataTransferObjects
{
    public class SpelInProcessMoveDto
    {
        public bool HasPassed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Guid? Token { get; set; }
        public Guid? SpelerToken { get; set; }
    }
}
