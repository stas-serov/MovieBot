using MovieBot.TMDB.Objects.Search;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieBot.TMDB.Objects.Responses
{
    public class SearchMovieResponse : BaseResponse
    {
        [JsonProperty("results")]
        public List<SearchMovie> results { get; set; }
    }
}
