using System;
using AnimeCrawler.Models;
using CsQuery;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AnimeCrawler.Helpers;

namespace AnimeCrawler.Providers
{
    public class NineAnime : AnimeProviderInterface.IAnimeProvider
    {
        private const string BaseUri = "https://9anime.is";
        private const string SearchString = "/search?keyword={0}";
        private const string SearchPageString = "/search?keyword={0}&page={1}";

        private readonly WebClient _client = new WebClient();
        private readonly Kitsu _kitsu = new Kitsu();
        private readonly RapidVideo _rapidVideo = new RapidVideo();

        public string Name { get; set; }

        public NineAnime()
        {
            Name = "NineAnime";
        }

        public List<AnimeResult> SearchAnime(string query)
        {
            var html = _client.DownloadString(BaseUri + string.Format(SearchString, query));
            var dom = CQ.CreateDocument(html);

            var results = new List<AnimeResult>();
            if (!string.IsNullOrEmpty(dom.Select(".total").Text()))
            {
                var totalPages = Convert.ToInt32(dom.Select(".total").Text());

                for (var page = 1; page <= totalPages; page++)
                {
                    var pageHtml = _client.DownloadString(BaseUri + string.Format(SearchPageString, query, page));
                    var pageDom = CQ.CreateDocument(pageHtml);

                    results.AddRange(pageDom.Select("div.item")
                        .Select(element => new AnimeResult
                        {
                            Name = element.Cq().Find(".name").Text(),
                            CoverUrl = element.Cq().Find("img").Attr("src"),
                            PageUrl = element.Cq().Find(".name").Attr("href"),
                            KitsuSearchResult = _kitsu.SearchAnime(element.Cq().Find(".name").Text()).Data?.First()
                        })
                        .ToList());
                }
            }
            else
            {
                results = dom.Select("div.item")
                    .Select(element => new AnimeResult
                    {
                        Name = element.Cq().Find(".name").Text(),
                        CoverUrl = element.Cq().Find("img").Attr("src"),
                        PageUrl = element.Cq().Find(".name").Attr("href"),
                        KitsuSearchResult = _kitsu.SearchAnime(element.Cq().Find(".name").Text()).Data?.First()
                    })
                    .ToList();
            }

            return results;
        }

        public List<EpisodeResult> RetrieveEpisodeResults(AnimeResult animeResult)
        {
            var html = _client.DownloadString(animeResult.PageUrl);
            var dom = CQ.CreateDocument(html);
            var results = dom.Select("label:contains('RapidVideo')").Next().Find("a")
                .Select(element => new EpisodeResult
                    {
                        Title = element.Cq().Attr("data-base"),
                        PageUrl = element.Cq().Attr("href")
                }
                ).ToList();
            return results;
        }

        public List<string> RetrieveVideoSource(EpisodeResult episodeResult)
        {
            var html = _client.DownloadString(BaseUri + episodeResult.PageUrl);
            var dom = CQ.CreateDocument(html);
            var rapidVideo = dom.Select("#player > iframe").Attr("src");
            var result = _rapidVideo.GetVideoSource(rapidVideo);
            var results = new List<string> { result };
            return results;
        }
    }
}