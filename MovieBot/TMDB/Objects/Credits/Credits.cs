using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieBot.TMDB.Objects.Credits
{
    public class Credits
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("cast")]
        public List<Cast> cast { get; set; }

        [JsonProperty("crew")]
        public List<Crew> crew { get; set; }
    }
}
