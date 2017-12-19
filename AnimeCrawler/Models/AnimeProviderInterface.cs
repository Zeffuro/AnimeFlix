using System.Collections.Generic;

namespace AnimeCrawler.Models
{
    internal class AnimeProviderInterface
    {
        public interface IAnimeProvider
        {
            string Name { get; set; }

            List<AnimeResult> SearchAnime(string query);
            List<EpisodeResult> RetrieveEpisodeResults(AnimeResult animeResult);
            List<string> RetrieveVideoSource(EpisodeResult episodeResult);
        }
    }
}
