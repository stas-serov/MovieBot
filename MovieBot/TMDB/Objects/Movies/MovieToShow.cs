using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MovieBot.TMDB.Objects.Movies
{
    public class MovieToShow
    {
        public string title { get; set; }

        public string originalTitle { get; set; }

        public string posterPath { get; set; }
        
        public DateTime? releaseDate { get; set; }

        public List<string> genres { get; set; }

        public int runtime { get; set; }

        public float rating { get; set; }

        public string description { get; set; }

        public List<string> directors { get; set; }

        public List<string> productionCountries { get; set; }

        public List<string> cast { get; set; }

        public Int128 budget { get; set; }

        public MovieToShow(string title, string originalTitle, string posterPath, DateTime? releaseDate, List<string> genres, int runtime, 
            float rating, string description, List<string> directors, List<string> productionCountries, List<string> cast, Int128 budget)
        {
            this.title = title;
            this.originalTitle = originalTitle;
            this.posterPath = posterPath;
            this.releaseDate = releaseDate;
            this.genres = genres;
            this.runtime = runtime;
            this.rating = rating;
            this.description = description;
            this.directors = directors;
            this.productionCountries = productionCountries;
            this.cast = cast;
            this.budget = budget;
        }

        public string ToStringUkr()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"<b>{title}</b> ({releaseDate?.Year})");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"🔖 <b>Оригінальна назва:</b> {originalTitle}");
            stringBuilder.AppendLine("🕒 <b>Тривалість:</b> " + (runtime == default ? "" : $"{runtime} хв"));
            stringBuilder.AppendLine($"🎭 <b>Жанр:</b> {string.Join(", ", genres)}");
            stringBuilder.AppendLine($"🌟 <b>Рейтинг:</b> {rating}");
            stringBuilder.AppendLine($"💰 <b>Бюджет:</b> " + (budget == default ? "" : $"{budget.ToString("#,#", CultureInfo.CurrentCulture)}$"));
            stringBuilder.AppendLine($"🎬 <b>Режисер:</b> {string.Join(", ", directors)}");
            stringBuilder.AppendLine($"🌍 <b>Країна:</b> {string.Join(", ", productionCountries)}");
            stringBuilder.AppendLine($"👥 <b>В ролях:</b> {string.Join(", ", cast)}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(description == string.Empty ? "" : $"\n<b>📄 Опис</b>\n{description}");
            stringBuilder.AppendLine($"<a href=\"{posterPath}\">&#160;</a>");
            return stringBuilder.ToString();
        }

        public string ToStringEng()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"<b>{title}</b> ({releaseDate?.Year})");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("🕒 <b>Duration:</b> " + (runtime == default ? "" : $"{runtime} min"));
            stringBuilder.AppendLine($"🎭 <b>Genre:</b> {string.Join(", ", genres)}");
            stringBuilder.AppendLine($"🌟 <b>Rating:</b> {rating}");
            stringBuilder.AppendLine($"💰 <b>Budget:</b> " + (budget == default ? "" : $"{budget.ToString("#,#", CultureInfo.CurrentCulture)}$"));
            stringBuilder.AppendLine($"🎬 <b>Director:</b> {string.Join(", ", directors)}");
            stringBuilder.AppendLine($"🌍 <b>Country:</b> {string.Join(", ", productionCountries)}");
            stringBuilder.AppendLine($"👥 <b>Cast:</b> {string.Join(", ", cast)}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(description == string.Empty ? "" : $"\n<b>📄 Overview</b>\n{description}");
            stringBuilder.AppendLine($"<a href=\"{posterPath}\">&#160;</a>");
            return stringBuilder.ToString();
        }
    }
}
