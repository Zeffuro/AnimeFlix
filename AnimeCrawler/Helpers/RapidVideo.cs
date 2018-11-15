using CsQuery;
using System.Net;

namespace AnimeCrawler.Helpers
{
    public class RapidVideo
    {
        private readonly WebClient _client = new WebClient();

        public string GetVideoSource(string url)
        {
            var html = _client.DownloadString(url);
            var dom = CQ.CreateDocument(html);
            //$("div:contains('Quality')")
            //if (string.IsNullOrEmpty(dom.Select("div:contains('Quality')").Text()))
                return dom.Select("source").Attr("src");
            var pageUrl = dom.Select("a").Last().Attr("href");
            var hqHtml = _client.DownloadString(pageUrl);
            var hqDom = CQ.CreateDocument(hqHtml);
            return hqDom.Select("source").Attr("src");
        }
    }
}
