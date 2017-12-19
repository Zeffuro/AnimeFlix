using System.Net;
using AnimeCrawler.Models;
using Newtonsoft.Json;

namespace AnimeCrawler.Helpers
{
    public class Kitsu
    {
        private const string SearchUri = "https://kitsu.io/api/edge/anime?filter[text]={0}";
        private readonly WebClient _client = new WebClient();

        public KitsuSearchResults SearchAnime(string query)
        {
            var json = _client.DownloadString(string.Format(SearchUri, query));
            return JsonConvert.DeserializeObject<KitsuSearchResults>(json);
        }
    }
}