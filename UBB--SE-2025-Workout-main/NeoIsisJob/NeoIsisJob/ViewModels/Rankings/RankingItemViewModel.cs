namespace NeoIsisJob.ViewModels.Rankings
{
    public class RankingItemViewModel
    {
        public int Rank { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Points { get; set; }
        public string ActivityDescription { get; set; } = string.Empty;
    }
} 