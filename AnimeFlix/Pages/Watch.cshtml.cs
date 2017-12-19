using AnimeCrawler.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using AnimeCrawler;

namespace AnimeFlix.Pages
{
    public class WatchModel : PageModel
    {
        private readonly Crawler _crawler = new Crawler();

        public List<AnimeResult> AnimeResults;
        public List<EpisodeResult> EpisodeResults;
        public List<string> EpisodeSources;

        public string EpisodeTitle { get; set; }
        public int EpisodePrev { get; set; }
        public int EpisodeNext { get; set; }
        public int EpisodeCount { get; set; }
        public int AnimeId { get; set; }
        public string Anime { get; set; }
        public string AnimeTitle { get; set; }
        public string Provider { get; set; }
        public bool ShowPrevious { get; set; }
        public bool ShowNext { get; set; }

        public void OnGet(string anime, int animeid, int episodeid, string provider)
        {
            Anime = anime;
            Provider = provider;
            AnimeId = animeid;

            AnimeResults = !string.IsNullOrEmpty(anime) ? _crawler.SearchAnime(anime, provider) : new List<AnimeResult>();
            EpisodeResults = !string.IsNullOrEmpty(anime) ? _crawler.RetrieveEpisodeResults(AnimeResults[animeid], provider) : new List<EpisodeResult>();

            AnimeTitle = AnimeResults[animeid].KitsuSearchResult.Attributes.CanonicalTitle;
            EpisodeTitle = EpisodeResults[episodeid].Title;
            EpisodeCount = EpisodeResults.Count;

            ShowPrevious = episodeid > 0;
            EpisodeNext = episodeid + 1;
            EpisodePrev = episodeid - 1;
            ShowNext = episodeid < EpisodeCount - 1;
            EpisodeSources = !string.IsNullOrEmpty(anime) ? _crawler.RetrieveVideoSource(EpisodeResults[episodeid], provider) : new List<string>();
        }
    }
}