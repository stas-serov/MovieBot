using System;
using System.Collections.Generic;
using System.Text;

namespace MovieBot.TMDB.Objects.TVShows
{
    public class TVShowToShow
    {
        public string title { get; set; }

        public string originalTitle { get; set; }

        public string posterPath { get; set; }

        public DateTime? firstEpisodeReleaseDate { get; set; }

        public List<string> genres { get; set; }

        public int episodeRuntime { get; set; }

        public float rating { get; set; }

        public string description { get; set; }

        public List<string> creators { get; set; }

        public List<string> productionCountries { get; set; }

        public List<string> cast { get; set; }

        public int numberOfSeasons { get; set; }

        public int numberOfEpisodes { get; set; }

        public TVShowToShow(string title, string originalTitle, string posterPath, DateTime? firstEpisodeReleaseDate,
            List<string> genres, int episodeRuntime, float rating, string description, List<string> creators,
            List<string> productionCountries, List<string> cast, int numberOfSeasons, int numberOfEpisodes)
        {
            this.title = title;
            this.originalTitle = originalTitle;
            this.posterPath = posterPath;
            this.firstEpisodeReleaseDate = firstEpisodeReleaseDate;
            this.genres = genres;
            this.episodeRuntime = episodeRuntime;
            this.rating = rating;
            this.description = description;
            this.creators = creators;
            this.productionCountries = productionCountries;
            this.cast = cast;
            this.numberOfSeasons = numberOfSeasons;
            this.numberOfEpisodes = numberOfEpisodes;
        }

        public string ToStringUkr()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"<b>{title}</b> ({firstEpisodeReleaseDate?.Year})");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"🔖 <b>Оригінальна назва:</b> {originalTitle}");
            stringBuilder.AppendLine("🕒 <b>Тривалість однієї серії:</b> " + (episodeRuntime == default ? "" : $"{episodeRuntime} хв"));
            stringBuilder.AppendLine($"🎭 <b>Жанр:</b> {string.Join(", ", genres)}");
            stringBuilder.AppendLine($"🌟 <b>Рейтинг:</b> {rating}");
            stringBuilder.AppendLine($"🎬 <b>Творець:</b> {string.Join(", ", creators)}");
            stringBuilder.AppendLine($"🌍 <b>Країна:</b> {string.Join(", ", productionCountries)}");
            stringBuilder.AppendLine($"💿 <b>Кількість сезонів:</b> {numberOfSeasons}");
            stringBuilder.AppendLine($"🔢 <b>Кількість серій:</b> {numberOfEpisodes}");
            stringBuilder.AppendLine($"👥 <b>В ролях:</b> {string.Join(", ", cast)}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(description == string.Empty ? "" : $"\n<b>📄 Опис</b>\n{description}");
            stringBuilder.AppendLine($"<a href=\"{posterPath}\">&#160;</a>");
            return stringBuilder.ToString();
        }

        public string ToStringEng()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"<b>{title}</b> ({firstEpisodeReleaseDate?.Year})");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("🕒 <b>One episode duration:</b> " + (episodeRuntime == default ? "" : $"{episodeRuntime} min"));
            stringBuilder.AppendLine($"🎭 <b>Genre:</b> {string.Join(", ", genres)}");
            stringBuilder.AppendLine($"🌟 <b>Rating:</b> {rating}");
            stringBuilder.AppendLine($"🎬 <b>Creator:</b> {string.Join(", ", creators)}");
            stringBuilder.AppendLine($"🌍 <b>Country:</b> {string.Join(", ", productionCountries)}");
            stringBuilder.AppendLine($"💿 <b>Number of seasons:</b> {numberOfSeasons}");
            stringBuilder.AppendLine($"🔢 <b>Number of series:</b> {numberOfEpisodes}");
            stringBuilder.AppendLine($"👥 <b> Cast:</b> {string.Join(", ", cast)}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(description == string.Empty ? "" : $"\n<b>📄 Overview</b>\n{description}");
            stringBuilder.AppendLine($"<a href=\"{posterPath}\">&#160;</a>");
            return stringBuilder.ToString();
        }
    }
}
