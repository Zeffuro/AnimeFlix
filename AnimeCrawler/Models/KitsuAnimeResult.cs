using System;
using System.Collections.Generic;

namespace AnimeCrawler.Models
{
    public class Links
    {
        public string Self { get; set; }
        public string Related { get; set; }
    }

    public class Titles
    {
        public string En { get; set; }
        public string EnJp { get; set; }
        public string JaJp { get; set; }
    }

    public class RatingFrequencies
    {
        public string R2 { get; set; }
        public string R3 { get; set; }
        public string R4 { get; set; }
        public string R5 { get; set; }
        public string R6 { get; set; }
        public string R7 { get; set; }
        public string R8 { get; set; }
        public string R9 { get; set; }
        public string R10 { get; set; }
        public string R11 { get; set; }
        public string R12 { get; set; }
        public string R13 { get; set; }
        public string R14 { get; set; }
        public string R15 { get; set; }
        public string R16 { get; set; }
        public string R17 { get; set; }
        public string R18 { get; set; }
        public string R19 { get; set; }
        public string R20 { get; set; }
    }

    public class Tiny
    {
        public object Width { get; set; }
        public object Height { get; set; }
    }

    public class Small
    {
        public object Width { get; set; }
        public object Height { get; set; }
    }

    public class Medium
    {
        public object Width { get; set; }
        public object Height { get; set; }
    }

    public class Large
    {
        public object Width { get; set; }
        public object Height { get; set; }
    }

    public class Dimensions
    {
        public Tiny Tiny { get; set; }
        public Small Small { get; set; }
        public Medium Medium { get; set; }
        public Large Large { get; set; }
    }

    public class Meta
    {
        public Dimensions Dimensions { get; set; }
    }

    public class PosterImage
    {
        public string Tiny { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Large { get; set; }
        public string Original { get; set; }
        public Meta Meta { get; set; }
    }

    public class CoverImage
    {
        public string Tiny { get; set; }
        public string Small { get; set; }
        public string Large { get; set; }
        public string Original { get; set; }
        public Meta Meta { get; set; }
    }

    public class Attributes
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Slug { get; set; }
        public string Synopsis { get; set; }
        public int? CoverImageTopOffset { get; set; }
        public Titles Titles { get; set; }
        public string CanonicalTitle { get; set; }
        public IList<string> AbbreviatedTitles { get; set; }
        public string AverageRating { get; set; }
        public RatingFrequencies RatingFrequencies { get; set; }
        public int? UserCount { get; set; }
        public int? FavoritesCount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? PopularityRank { get; set; }
        public int? RatingRank { get; set; }
        public string AgeRating { get; set; }
        public string AgeRatingGuide { get; set; }
        public string Subtype { get; set; }
        public string Status { get; set; }
        public string Tba { get; set; }
        public PosterImage PosterImage { get; set; }
        public CoverImage CoverImage { get; set; }
        public int? EpisodeCount { get; set; }
        public int? EpisodeLength { get; set; }
        public string YoutubeVideoId { get; set; }
        public string ShowType { get; set; }
        public bool Nsfw { get; set; }
    }

    public class Genres
    {
        public Links Links { get; set; }
    }

    public class Categories
    {
        public Links Links { get; set; }
    }

    public class Castings
    {
        public Links Links { get; set; }
    }

    public class Installments
    {
        public Links Links { get; set; }
    }

    public class Mappings
    {
        public Links Links { get; set; }
    }

    public class Reviews
    {
        public Links Links { get; set; }
    }

    public class MediaRelationships
    {
        public Links Links { get; set; }
    }

    public class Episodes
    {
        public Links Links { get; set; }
    }

    public class StreamingLinks
    {
        public Links Links { get; set; }
    }

    public class AnimeProductions
    {
        public Links Links { get; set; }
    }

    public class AnimeCharacters
    {
        public Links Links { get; set; }
    }

    public class AnimeStaff
    {
        public Links Links { get; set; }
    }

    public class Relationships
    {
        public Genres Genres { get; set; }
        public Categories Categories { get; set; }
        public Castings Castings { get; set; }
        public Installments Installments { get; set; }
        public Mappings Mappings { get; set; }
        public Reviews Reviews { get; set; }
        public MediaRelationships MediaRelationships { get; set; }
        public Episodes Episodes { get; set; }
        public StreamingLinks StreamingLinks { get; set; }
        public AnimeProductions AnimeProductions { get; set; }
        public AnimeCharacters AnimeCharacters { get; set; }
        public AnimeStaff AnimeStaff { get; set; }
    }

    public class Datum
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public Links Links { get; set; }
        public Attributes Attributes { get; set; }
        public Relationships Relationships { get; set; }
    }

    public class KitsuSearchResults
    {
        public IList<Datum> Data { get; set; }
        public Meta Meta { get; set; }
        public Links Links { get; set; }
    }
}