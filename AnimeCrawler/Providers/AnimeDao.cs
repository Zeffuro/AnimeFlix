using AnimeCrawler.Models;
using CsQuery;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using AnimeCrawler.Helpers;

namespace AnimeCrawler.Providers
{
    public class AnimeDao : AnimeProviderInterface.IAnimeProvider
    {
        private const string BaseUri = "https://animedao.com";
        private const string SearchString = "/search/?key={0}";
        private readonly WebClient _client = new WebClient();
        private readonly Kitsu _kitsu = new Kitsu();

        public string Name { get; set; }

        public AnimeDao()
        {
            Name = "AnimeDao";
        }

        public List<AnimeResult> SearchAnime(string query)
        {
            var html = _client.DownloadString(BaseUri + string.Format(SearchString, query));
            var dom = CQ.CreateDocument(html);
            var results = dom.Select(".animelist_poster")
                .Select(element => new AnimeResult
                {
                    Name = element.Cq().Find("center").Text(),
                    CoverUrl = BaseUri + element.Cq().Select(".animelist_poster_caption").Prev().Attr("data-original"),
                    PageUrl = element.Cq().Find("a").Attr("href"),
                    KitsuSearchResult = _kitsu.SearchAnime(element.Cq().Find("center").Text()).Data?.First()
                })
                .ToList();
            return results;
        }

        public List<EpisodeResult> RetrieveEpisodeResults(AnimeResult animeResult)
        {
            var html = _client.DownloadString(BaseUri + animeResult.PageUrl);
            var dom = CQ.CreateDocument(html);
            var results = dom.Select(".animeinfo-content")
                .Select(element => new EpisodeResult
                    {
                        Title = element.Cq().Find("b").Text(),
                        Description = element.Cq().Find(".list-group-item-text").Text().Trim(),
                        PageUrl = element.Cq().Attr("href")
                }
                ).ToList();
            return results;
        }

        public List<string> RetrieveVideoSource(EpisodeResult episodeResult)
        {
            var html = _client.DownloadString(BaseUri + episodeResult.PageUrl);
            var file = new Regex("file: \"(?<File>.*?)\"").Matches(html)[0].Groups["File"].Value;
            var results = new List<string> { BaseUri + file };
            return results;
        }
    }
}