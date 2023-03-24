using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.Responses
{
    public abstract class BaseResponse
    {

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("page")]
        public int page { get; set; }

        [JsonProperty("total_results")]
        public int totalResults { get; set; }

        [JsonProperty("total_pages")]
        public int totalPages { get; set; }
    }
}
