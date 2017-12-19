using System;
using System.Linq;
using AnimeCrawler;
using AnimeCrawler.Helpers;

namespace AnimeCrawlerConsole
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Hello World!");

            var crawler = new Crawler();
            var provider = "NineAnime";

            var animeResults = crawler.SearchAnime("bleach", provider);

            var firstEpisode = crawler.RetrieveEpisodeResults(animeResults.First(), provider).First();
            foreach (var videoSource in crawler.RetrieveVideoSource(firstEpisode, provider))
            {
                Console.WriteLine(videoSource);
            }

            //var kitsu = new Kitsu();
            //var searchResults = kitsu.SearchAnime("Naruto").Data;
            //Console.Write(searchResults.First().Attributes.CanonicalTitle);

            Console.ReadKey();
        }
    }
}