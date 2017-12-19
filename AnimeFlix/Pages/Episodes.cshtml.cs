using System.Collections.Generic;
using AnimeCrawler;
using AnimeCrawler.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeFlix.Pages
{
    public class EpisodesModel : PageModel
    {
        private readonly Crawler _crawler = new Crawler();

        public List<AnimeResult> AnimeResults;
        public List<EpisodeResult> EpisodeResults;
        public string Anime { get; set; }
        public string AnimeTitle { get; set; }
        public string Provider { get; set; }
        public int AnimeId { get; set; }
        public Datum Kitsu { get; set; }

        public void OnGet(string anime, int id, string provider)
        {
            Anime = anime;
            AnimeId = id;
            Provider = provider;
            AnimeResults = !string.IsNullOrEmpty(anime)
                ? _crawler.SearchAnime(anime, provider)
                : new List<AnimeResult>();
            AnimeTitle = AnimeResults[id].KitsuSearchResult.Attributes.CanonicalTitle;
            Kitsu = AnimeResults[id].KitsuSearchResult;
            EpisodeResults = !string.IsNullOrEmpty(anime)
                ? _crawler.RetrieveEpisodeResults(AnimeResults[id], provider)
                : new List<EpisodeResult>();
        }
    }
}