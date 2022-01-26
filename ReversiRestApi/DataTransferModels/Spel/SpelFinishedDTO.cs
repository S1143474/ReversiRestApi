namespace ReversiRestApi.DataTransferModels.Spel
{
    public class SpelFinishedDTO : BaseTransferModel
    {
        public bool IsSpelFinished { get; set; }

        public bool IsDraw { get; set; }

        public string WinnerToken { get; set; }

        public string LoserToken {  get; set; }
    }
}
