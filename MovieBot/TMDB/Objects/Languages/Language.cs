using Newtonsoft.Json;

namespace MovieBot.TMDB.Objects.Languages
{
    public class Language
    {
        [JsonProperty("english_name")]
        public string englishName { get; set; }

        [JsonProperty("iso_639_1")]
        public string iso_639_1 { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }
}
