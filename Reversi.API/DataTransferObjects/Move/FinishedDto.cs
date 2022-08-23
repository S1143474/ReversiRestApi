namespace Reversi.API.DataTransferObjects.Move
{
    public class FinishedDto : BaseDto
    {
        public bool IsSpelFinished { get; set; }

        public bool IsDraw { get; set; }

        public string WinnerToken { get; set; }

        public string LoserToken { get; set; }  
    }
}
