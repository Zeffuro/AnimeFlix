using System;
using AnimeCrawler.Models;
using CsQuery;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AnimeCrawler.Helpers;

namespace AnimeCrawler.Providers
{
    public class GoGoAnime : AnimeProviderInterface.IAnimeProvider
    {
        private const string BaseUri = "https://www.gogoanimes.co";
        private const string SearchString = "/search.html?keyword={0}";
        private const string SearchPageString = "/search.html?keyword={0}&page={1}";
        private const string EpisodeLookupApi = "https://api.watchanime.cc/ajax/load-list-episode?ep_start=0&ep_end=999999&id={0}";

        private readonly WebClient _client = new WebClient();
        private readonly Kitsu _kitsu = new Kitsu();
        private readonly RapidVideo _rapidVideo = new RapidVideo();

        public string Name { get; set; }

        public GoGoAnime()
        {
            Name = "GoGoAnime";
        }

        public List<AnimeResult> SearchAnime(string query)
        {
            var html = _client.DownloadString(BaseUri + string.Format(SearchString, query));
            var dom = CQ.CreateDocument(html);

            var results = new List<AnimeResult>();
            if (!string.IsNullOrEmpty(dom.Select(".pagination-list li").Text()))
            {
                var totalPages = Convert.ToInt32(dom.Select(".pagination-list li").Last().Text());

                for (var page = 1; page <= totalPages; page++)
                {
                    var pageHtml = _client.DownloadString(BaseUri + string.Format(SearchPageString, query, page));
                    var pageDom = CQ.CreateDocument(pageHtml);

                    results.AddRange(pageDom.Select(".items li")
                        .Select(element => new AnimeResult
                        {
                            Name = element.Cq().Find(".name").Text(),
                            CoverUrl = element.Cq().Find("img").Attr("src"),
                            PageUrl = BaseUri + element.Cq().Find("a").Attr("href"),
                            KitsuSearchResult = _kitsu.SearchAnime(element.Cq().Find(".name").Text()).Data?.First()
                        })
                        .ToList());
                }
            }
            else
            {
                results = dom.Select(".items li")
                    .Select(element => new AnimeResult
                    {
                        Name = element.Cq().Find(".name").Text(),
                        CoverUrl = element.Cq().Find("img").Attr("src"),
                        PageUrl = BaseUri + element.Cq().Find("a").Attr("href"),
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
            var animeId = dom.Select("input#movie_id").Val();
            var episodeListHtml = _client.DownloadString(string.Format(EpisodeLookupApi, animeId));
            dom = CQ.CreateDocument(episodeListHtml);
            var results = dom.Select("li")
                .Select(element => new EpisodeResult
                {
                    Title = element.Cq().Find(".name").Text().Trim(),
                    PageUrl = BaseUri + element.Cq().Find("a").Attr("href").Trim()
                })
                .ToList();
            return results;
        }

        public List<string> RetrieveVideoSource(EpisodeResult episodeResult)
        {
            var html = _client.DownloadString(episodeResult.PageUrl);
            var dom = CQ.CreateDocument(html);
            var rapidVideo = dom.Select(".rapidvideo a").Attr("data-video");
            var result = _rapidVideo.GetVideoSource(rapidVideo);
            var results = new List<string> { result };
            return results;
        }
    }
}