using AnimeCrawler.Models;
using CsQuery;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using AnimeCrawler.Helpers;
using CloudFlareUtilities;

namespace AnimeCrawler.Providers
{
    public class EyeOnAnime : AnimeProviderInterface.IAnimeProvider
    {
        private const string BaseUri = "http://eyeonanime.tv";
        private const string SearchString = "/anime-search/";
        private readonly Kitsu _kitsu = new Kitsu();

        private readonly HttpClient _httpClient = new HttpClient();

        public string Name { get; set; }

        public EyeOnAnime()
        {
            Name = "EyeOnAnime";
        }

        public List<AnimeResult> SearchAnime(string query)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("anime_name", query)
            });

            //var html = _httpClient.GetStringAsync(BaseUri + string.Format(SearchString, query)).Result;
            var html = _httpClient.PostAsync(BaseUri + SearchString, content).Result.ToString();
            var dom = CQ.CreateDocument(html);
            var results = dom.Select(".ws-anime li")
                .Select(element => new AnimeResult
                {
                    Name = element.Cq().Find("a").Eq(0).Text(),
                    CoverUrl = BaseUri + element.Cq().Find("img").Attr("src"),
                    PageUrl = element.Cq().Find("a").Eq(0).Attr("href"),
                    KitsuSearchResult = _kitsu.SearchAnime(element.Cq().Find("a").Eq(0).Text()).Data?.First()
                })
                .ToList();
            return results;
        }

        public List<EpisodeResult> RetrieveEpisodeResults(AnimeResult animeResult)
        {
            var html = _httpClient.GetStringAsync(BaseUri + animeResult.PageUrl).Result;
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
            var html = _httpClient.GetStringAsync(BaseUri + episodeResult.PageUrl).Result;
            var file = new Regex("file: \"(?<File>.*?)\"").Matches(html)[0].Groups["File"].Value;
            var resolveUrl = _httpClient.GetAsync(BaseUri + file, HttpCompletionOption.ResponseHeadersRead).Result;
            resolveUrl.EnsureSuccessStatusCode();
            var results = new List<string> { resolveUrl.RequestMessage.RequestUri.ToString() };
            return results;
        }
    }
}