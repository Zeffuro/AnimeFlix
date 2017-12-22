using System;
using System.Collections.Generic;
using AnimeCrawler;
using AnimeCrawler.Models;
using LazyCache;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AnimeFlix.Pages
{
    public class EpisodesModel : PageModel
    {
        private readonly Crawler _crawler = new Crawler();
        private readonly IAppCache _cache;

        private readonly CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };

        public List<AnimeResult> AnimeResults;
        public List<EpisodeResult> EpisodeResults;
        public string Anime { get; set; }
        public string AnimeTitle { get; set; }
        public string Provider { get; set; }
        public int AnimeId { get; set; }
        public Datum Kitsu { get; set; }

        public EpisodesModel(IAppCache cache)
        {
            _cache = cache;
        }

        public void OnGet(string anime, int id, string provider)
        {
            Anime = anime;
            AnimeId = id;
            Provider = provider;

            List<AnimeResult> SearchAnimeCached() => _crawler.SearchAnime(anime, provider);
            AnimeResults = !string.IsNullOrEmpty(anime)
                ? _cache.GetOrAdd($"AnimeCrawler-SearchAnime-{anime}-{provider}", SearchAnimeCached, _cacheItemPolicy)
                : new List<AnimeResult>();

            AnimeTitle = AnimeResults[id].KitsuSearchResult.Attributes.CanonicalTitle;
            Kitsu = AnimeResults[id].KitsuSearchResult;

            List<EpisodeResult> RetrieveEpisodeCached() => _crawler.RetrieveEpisodeResults(AnimeResults[id], provider);
            EpisodeResults = !string.IsNullOrEmpty(anime)
                ? _cache.GetOrAdd($"AnimeCrawler-RetrieveEpisodes-{anime}-{AnimeResults[id]}-{provider}", RetrieveEpisodeCached, _cacheItemPolicy)
                : new List<EpisodeResult>();
        }
    }
}