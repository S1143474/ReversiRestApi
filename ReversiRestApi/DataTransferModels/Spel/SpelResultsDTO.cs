namespace ReversiRestApi.DataTransferModels.Spel
{
    public class SpelResultsDTO : SpelFinishedDTO
    {
        public int AmountOfWitFichesTurned { get; set; }
        public int AmountOfZwartFichesTurned { get; set; }
        public int AmountOfGeenFichesTurned { get; set; }
    }
}
