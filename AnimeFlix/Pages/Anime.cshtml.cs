using System;
using AnimeCrawler;
using AnimeCrawler.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using LazyCache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace AnimeFlix.Pages
{
    public class AnimeModel : PageModel
    {
        private readonly Crawler _crawler = new Crawler();
        private readonly IAppCache _cache;

        public List<AnimeResult> AnimeResults;
        public List<EpisodeResult> EpisodeResults;

        public string Anime { get; set; }
        public string Provider { get; set; }

        public AnimeModel (CachingService cache)
        {
            _cache = new CachingService();
        }

        public void OnGet(string anime, string provider)
        {
            if (!string.IsNullOrEmpty(provider))
            {
                SetCookie("AnimeProvider", provider, 365);
                Provider = provider;
            }
            Anime = anime;
            List<AnimeResult> SearchAnimeCached() => _crawler.SearchAnime(anime, provider);

            //AnimeResults = !string.IsNullOrEmpty(anime) ? _crawler.SearchAnime(anime, provider) : new List<AnimeResult>();
            AnimeResults = !string.IsNullOrEmpty(anime) ? _cache.GetOrAdd($"AnimeCrawler-SearchAnime-{anime}-{provider}", SearchAnimeCached) : new List<AnimeResult>();
        }

        public void SetCookie(string key, string value, int? expireTime)
        {
            var option = new CookieOptions
            {
                Expires = expireTime.HasValue
                    ? DateTime.Now.AddMinutes(expireTime.Value)
                    : DateTime.Now.AddMilliseconds(10)
            };
            Response.Cookies.Append(key, value, option);
        }
    }
}