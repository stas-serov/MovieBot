using System.Collections.Generic;
using Xunit;
using MovieBot.TMDB.Objects.Search;
using MovieBot.TMDB.Clients;
using System.Threading.Tasks;
using MovieBot.TMDB.Objects.Movies;
using MovieBot.TMDB.Objects.TVShows;

namespace TestProject
{
    public class UnitTest
    {
        public static TMDBClient client = new TMDBClient("d9c080afd04749606ed64b6a082c0678");
        [Fact]
        public async Task TestSearchMoviesAsync()
        {
            List<SearchMovie> searchMovies = await client.SearchMoviesAsync(5, "Аватар", "uk");
            Assert.NotEmpty(searchMovies);
        }

        [Fact]
        public async Task TestGetNowPlayingMoviesAsync()
        {
            List<SearchMovie> searchMovies = await client.GetNowPlayingMoviesAsync(5, "uk");
            Assert.NotEmpty(searchMovies);
        }

        [Fact]
        public async Task TestGetPopularMoviesAsync()
        {
            List<SearchMovie> searchMovies = await client.GetPopularMoviesAsync(5, "uk");
            Assert.NotEmpty(searchMovies);
        }

        [Fact]
        public async Task TestGetTopRatedMoviesAsync()
        {
            List<SearchMovie> searchMovies = await client.GetTopRatedMoviesAsync(5, "uk");
            Assert.NotEmpty(searchMovies);
        }

        [Fact]
        public async Task TestGetUpcomingMoviesAsync()
        {
            List<SearchMovie> searchMovies = await client.GetUpcomingMoviesAsync(5, "uk");
            Assert.NotEmpty(searchMovies);
        }

        [Fact]
        public async Task TestSearchTVShowsAsync()
        {
            List<SearchTVShow> searchTVShows = await client.SearchTVShowsAsync(5, "Гра престолів", "uk");
            Assert.NotEmpty(searchTVShows);
        }

        [Fact]
        public async Task TestGetAiringTodayTVShowsAsync()
        {
            List<SearchTVShow> searchTVShows = await client.GetAiringTodayTVShowsAsync(5, "uk");
            Assert.NotEmpty(searchTVShows);
        }

        [Fact]
        public async Task TestGetOnAirTVShowsAsync()
        {
            List<SearchTVShow> searchTVShows = await client.GetOnAirTVShowsAsync(5, "uk");
            Assert.NotEmpty(searchTVShows);
        }

        [Fact]
        public async Task TestGetPopularTVShowsAsync()
        {
            List<SearchTVShow> searchTVShows = await client.GetPopularTVShowsAsync(5, "uk");
            Assert.NotEmpty(searchTVShows);
        }

        [Fact]
        public async Task TestGetTopRatedTVShowsAsync()
        {
            List<SearchTVShow> searchTVShows = await client.GetTopRatedTVShowsAsync(5, "uk");
            Assert.NotEmpty(searchTVShows);
        }

        [Fact]
        public async Task TestGetMovieDetailsAsync()
        {
            MovieDetails movieDetails = await client.GetMovieDetailsAsync(24428, "uk");
            Assert.NotNull(movieDetails);
        }

        [Fact]
        public async Task TestGetTVShowDetailsAsync()
        {
            TVShowDetails tVShowDetails = await client.GetTVShowDetailsAsync(1399, "uk");
            Assert.NotNull(tVShowDetails);
        }

        [Fact]
        public async Task TestGetPosterUrl()
        {
            string expectedUrl = "https://image.tmdb.org/t/p/w500/cezWGskPY5x7GaglTTRN4Fugfb8.jpg";
            string actualUrl = client.GetPosterUrl(500, "/cezWGskPY5x7GaglTTRN4Fugfb8.jpg");
            Assert.Equal(expectedUrl, actualUrl);
        }
    }
}