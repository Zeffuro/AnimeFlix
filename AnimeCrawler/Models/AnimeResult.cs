namespace AnimeCrawler.Models
{
    public class AnimeResult
    {
        public string Name { get; set; }
        public string PageUrl { get; set; }
        public string CoverUrl { get; set; }
        public Datum KitsuSearchResult { get; set; }
    }
}