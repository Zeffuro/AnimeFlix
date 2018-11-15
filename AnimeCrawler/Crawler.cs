using System.Collections.Generic;
using AnimeCrawler.Models;
using AnimeCrawler.Providers;
using static AnimeCrawler.Models.AnimeProviderInterface;

namespace AnimeCrawler
{
    public class Crawler
    {
        private readonly IReadOnlyDictionary<string, IAnimeProvider> _animeProviders = new Dictionary<string, IAnimeProvider>
        {
            ["AnimeDao"] = new AnimeDao(),
            ["AnimeTwist"] = new AnimeTwist(),
            ["GoGoAnime"] = new GoGoAnime(),
            ["EyeOnAnime"] = new EyeOnAnime(),
            ["NineAnime"] = new NineAnime() //Currently Broken
        };

        public List<AnimeResult> SearchAnime(string query, string provider)
        {
            return _animeProviders[provider].SearchAnime(query);
        }

        public List<EpisodeResult> RetrieveEpisodeResults(AnimeResult animeResult, string provider)
        {
            return _animeProviders[provider].RetrieveEpisodeResults(animeResult);
        }

        public List<string> RetrieveVideoSource(EpisodeResult episodeResult, string provider)
        {
            return _animeProviders[provider].RetrieveVideoSource(episodeResult);
        }
    }
}
