using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AnimeCrawler.Helpers;
using AnimeCrawler.Models;
using CsQuery;

namespace AnimeCrawler.Providers
{
    public class AnimeTwist : AnimeProviderInterface.IAnimeProvider
    {
        private const string BaseUri = "https://twist.moe";
        private readonly Dictionary<string, string> _animeList = new Dictionary<string, string>();
        private readonly WebClient _client = new WebClient();
        private readonly Kitsu _kitsu = new Kitsu();

        public AnimeTwist()
        {
            Name = "AnimeTwist";
        }

        public string Name { get; set; }

        public List<AnimeResult> SearchAnime(string query)
        {
            var html = _client.DownloadString(BaseUri);
            var dom = CQ.CreateDocument(html);
            foreach (var result in dom.Select(".series-title"))
            {
                var alt = result.Cq().Attr("data-alt") ?? "";
                _animeList.Add(result.Cq().Attr("data-title"), alt);
            }

            var results = (from item in _animeList
                where item.Key.ToLower().Contains(query.ToLower()) || item.Value.ToLower().Contains(query.ToLower())
                select new AnimeResult
                {
                    Name = item.Key,
                    PageUrl = dom.Select($"[data-title='{item.Key}']").Attr("href"),
                    KitsuSearchResult = _kitsu.SearchAnime(item.Key).Data?.First()
                }).ToList();

            return results;
        }

        public List<EpisodeResult> RetrieveEpisodeResults(AnimeResult animeResult)
        {
            var html = _client.DownloadString(BaseUri + animeResult.PageUrl);
            var dom = CQ.CreateDocument(html);
            var results = dom.Select("[role='button']")
                .Select(element => new EpisodeResult
                    {
                        Title = $"{animeResult.Name} {Convert.ToInt32(element.Cq().Attr("data-episode")) + 1}",
                        PageUrl = element.Cq().Attr("href")
                    }
                ).ToList();
            return results;
        }

        public List<string> RetrieveVideoSource(EpisodeResult episodeResult)
        {
            var html = _client.DownloadString(BaseUri + episodeResult.PageUrl);
            var dom = CQ.CreateDocument(html);
            var results = new List<string> {BaseUri + dom.Select("video").Attr("src").Replace(" ", "%20")};
            return results;
        }
    }
}