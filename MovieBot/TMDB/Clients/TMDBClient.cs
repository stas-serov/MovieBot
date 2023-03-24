using MovieBot.TMDB.Objects.Credits;
using MovieBot.TMDB.Objects.Movies;
using MovieBot.TMDB.Objects.Responses;
using MovieBot.TMDB.Objects.Search;
using MovieBot.TMDB.Objects.TVShows;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieBot.TMDB.Clients
{
    public class TMDBClient
    {
        private const string baseAdress = "https://api.themoviedb.org";

        private const string baseAdressImages = "https://image.tmdb.org/t/p/";

        private const string apiVersion = "3";

        private const string movieContent = "movie";

        private const string tvShowContent = "tv";

        private const string search = "search";

        private const string nowPlaying = "now_playing";

        private const string popular = "popular";

        private const string topRated = "top_rated";

        private const string upcoming = "upcoming";

        private const string airingToday = "airing_today";

        private const string onAir = "on_the_air";
        public string apiKey { get; set; }

        public TMDBClient(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<List<SearchMovie>> SearchMoviesAsync(int numberOfMovies, string movieName, string language)
        {
            List<SearchMovie> searchMovies = new List<SearchMovie>();
            int page = 1;
            while (searchMovies.Count < numberOfMovies)
            {
                string queryString = BuildQuery(search, movieContent, null, null, language, movieName, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != null)
                {
                    SearchMovieResponse searchMovieResponse = JsonConvert.DeserializeObject<SearchMovieResponse>(queryResponse);
                    searchMovies.AddRange(searchMovieResponse.results);
                    page++;
                    if (page > searchMovieResponse.totalPages) break;
                }
                else break;
            }
            return searchMovies;
        }

        public async Task<List<SearchMovie>> GetNowPlayingMoviesAsync(int numberOfMovies, string language)
        {
            List<SearchMovie> searchMovies = new List<SearchMovie>();
            int page = 1;
            while (searchMovies.Count < numberOfMovies)
            {
                string queryString = BuildQuery(null, movieContent, null, nowPlaying, language, null, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchMovieResponse searchMovieResponse = JsonConvert.DeserializeObject<SearchMovieResponse>(queryResponse);
                    searchMovies.AddRange(searchMovieResponse.results);
                    page++;
                    if (page > searchMovieResponse.totalPages) break;
                }
                else break;
            }
            return searchMovies.Take(numberOfMovies).ToList();
        }

        public async Task<List<SearchMovie>> GetPopularMoviesAsync(int numberOfMovies, string language)
        {
            List<SearchMovie> searchMovies = new List<SearchMovie>();
            int page = 1;
            while (searchMovies.Count < numberOfMovies)
            {
                string queryString = BuildQuery(null, movieContent, null, popular, language, null, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchMovieResponse searchMovieResponse = JsonConvert.DeserializeObject<SearchMovieResponse>(queryResponse);
                    searchMovies.AddRange(searchMovieResponse.results);
                    page++;
                    if (page > searchMovieResponse.totalPages) break;
                }
                else break;
            }
            return searchMovies.Take(numberOfMovies).ToList();
        }

        public async Task<List<SearchMovie>> GetTopRatedMoviesAsync(int numberOfMovies, string language)
        {
            List<SearchMovie> searchMovies = new List<SearchMovie>();
            int page = 1;
            while (searchMovies.Count < numberOfMovies)
            {
                string queryString = BuildQuery(null, movieContent, null, topRated, language, null, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchMovieResponse searchMovieResponse = JsonConvert.DeserializeObject<SearchMovieResponse>(queryResponse);
                    searchMovies.AddRange(searchMovieResponse.results);
                    page++;
                    if (page > searchMovieResponse.totalPages) break;
                }
                else break;
            }
            return searchMovies.Take(numberOfMovies).ToList();
        }

        public async Task<List<SearchMovie>> GetUpcomingMoviesAsync(int numberOfMovies, string language)
        {
            List<SearchMovie> searchMovies = new List<SearchMovie>();
            int page = 1;
            while (searchMovies.Count < numberOfMovies)
            {
                string queryString = BuildQuery(null, movieContent, null, upcoming, language, null, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchMovieResponse searchMovieResponse = JsonConvert.DeserializeObject<SearchMovieResponse>(queryResponse);
                    searchMovies.AddRange(searchMovieResponse.results);
                    page++;
                    if (page > searchMovieResponse.totalPages) break;
                }
                else break;
            }
            return searchMovies.Take(numberOfMovies).ToList();
        }

        public async Task<List<SearchTVShow>> SearchTVShowsAsync(int numberOfTVShows, string tvshowName, string language)
        {
            List<SearchTVShow> searchTVShows = new List<SearchTVShow>();
            int page = 1;
            while (searchTVShows.Count < numberOfTVShows)
            {
                string queryString = BuildQuery(search, tvShowContent, null, null, language, tvshowName, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchTVShowResponse searchTVShowResponse = JsonConvert.DeserializeObject<SearchTVShowResponse>(queryResponse);
                    searchTVShows.AddRange(searchTVShowResponse.results);
                    page++;
                    if (page > searchTVShowResponse.totalPages) break;
                }
                else break;
            }

            return searchTVShows.Take(numberOfTVShows).ToList();
        }

        public async Task<List<SearchTVShow>> GetAiringTodayTVShowsAsync(int numberOfTVShows, string language)
        {
            List<SearchTVShow> searchTVShows = new List<SearchTVShow>();
            int page = 1;
            while (searchTVShows.Count < numberOfTVShows)
            {
                string queryString = BuildQuery(null, tvShowContent, null, airingToday, language, null, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchTVShowResponse searchTVShowResponse = JsonConvert.DeserializeObject<SearchTVShowResponse>(queryResponse);
                    searchTVShows.AddRange(searchTVShowResponse.results);
                    page++;
                    if (page > searchTVShowResponse.totalPages) break;
                }
                else break;
            }
            return searchTVShows.Take(numberOfTVShows).ToList();
        }

        public async Task<List<SearchTVShow>> GetOnAirTVShowsAsync(int numberOfTVShows, string language)
        {
            List<SearchTVShow> searchTVShows = new List<SearchTVShow>();
            int page = 1;
            while (searchTVShows.Count < numberOfTVShows)
            {
                string queryString = BuildQuery(null, tvShowContent, null, onAir, language, null, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchTVShowResponse searchTVShowResponse = JsonConvert.DeserializeObject<SearchTVShowResponse>(queryResponse);
                    searchTVShows.AddRange(searchTVShowResponse.results);
                    page++;
                    if (page > searchTVShowResponse.totalPages) break;
                }
                else break;
            }
            return searchTVShows.Take(numberOfTVShows).ToList();
        }

        public async Task<List<SearchTVShow>> GetPopularTVShowsAsync(int numberOfTVShows, string language)
        {
            List<SearchTVShow> searchTVShows = new List<SearchTVShow>();
            int page = 1;
            while (searchTVShows.Count < numberOfTVShows)
            {
                string queryString = BuildQuery(null, tvShowContent, null, popular, language, null, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchTVShowResponse searchTVShowResponse = JsonConvert.DeserializeObject<SearchTVShowResponse>(queryResponse);
                    searchTVShows.AddRange(searchTVShowResponse.results);
                    page++;
                    if (page > searchTVShowResponse.totalPages) break;
                }
                else break;
            }
            return searchTVShows.Take(numberOfTVShows).ToList();
        }

        public async Task<List<SearchTVShow>> GetTopRatedTVShowsAsync(int numberOfTVShows, string language)
        {
            List<SearchTVShow> searchTVShows = new List<SearchTVShow>();
            int page = 1;
            while (searchTVShows.Count < numberOfTVShows)
            {
                string queryString = BuildQuery(null, tvShowContent, null, topRated, language, null, page);
                string queryResponse = await SendQueryAsync(queryString);
                if (queryResponse != string.Empty)
                {
                    SearchTVShowResponse searchTVShowResponse = JsonConvert.DeserializeObject<SearchTVShowResponse>(queryResponse);
                    searchTVShows.AddRange(searchTVShowResponse.results);
                    page++;
                    if (page > searchTVShowResponse.totalPages) break;
                }
                else break;
            }
            return searchTVShows.Take(numberOfTVShows).ToList();
        }
        private string BuildQuery(string? method, string contentType, int? id, string? category, string language, string? contentName,
            int? page)
        {
            StringBuilder queryString = new StringBuilder();
            queryString.Append(baseAdress);
            queryString.Append("/" + apiVersion);
            if (method != null)
            {
                queryString.Append("/" + method);
            }
            queryString.Append("/" + contentType);
            if (id.HasValue)
            {
                queryString.Append("/" + id);
            }
            if (category != null)
            {
                queryString.Append("/" + category);
            }
            queryString.Append("?api_key=" + apiKey);
            queryString.Append("&language=" + language.ToString());
            if (contentName != null)
            {
                queryString.Append("&query=" + contentName.Replace(" ", "%"));
            }
            if (page != null)
            {
                queryString.Append("&page=" + page);
            }
            return queryString.ToString();
        }

        public async Task<MovieDetails> GetMovieDetailsAsync(int movieId, string language)
        {
            string queryString = BuildQuery(null, movieContent, movieId, null, language, null, null);
            string queryResponse = await SendQueryAsync(queryString);
            MovieDetails movieDetails = null;
            if (queryResponse != null)
            {
                movieDetails = JsonConvert.DeserializeObject<MovieDetails>(queryResponse);
            }
            return movieDetails;
        }

        public async Task<TVShowDetails> GetTVShowDetailsAsync(int tvShowId, string lauguage)
        {
            string queryString = BuildQuery(null, tvShowContent, tvShowId, null, lauguage, null, null);
            string queryResponse = await SendQueryAsync(queryString);
            TVShowDetails tvShowDetails = null;
            if (queryResponse != string.Empty)
            {
                tvShowDetails = JsonConvert.DeserializeObject<TVShowDetails>(queryResponse);
            }
            return tvShowDetails;
        }

        public async Task<Credits> GetCreditsAsync(string contentType, int contentId, string language)
        {
            string queryString = BuildQuery(null, contentType, contentId, "credits", language, null, null);
            string queryResponse = await SendQueryAsync(queryString);
            Credits credits = new Credits();
            if (queryResponse != string.Empty)
            {
                credits = JsonConvert.DeserializeObject<Credits>(queryResponse);
            }
            return credits;
        }

        public string GetPosterUrl(int width, string posterPath)
        {
            StringBuilder posterUrl = new StringBuilder();
            posterUrl.Append(baseAdressImages);
            posterUrl.Append("w" + width);
            posterUrl.Append(posterPath);
            return posterUrl.ToString();
        }

        private async Task<string> SendQueryAsync(string query)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync(query);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return httpResponse.Content.ReadAsStringAsync().Result;
            }
            else return string.Empty;
        }
    }
}
