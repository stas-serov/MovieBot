﻿using MovieBot.TMDB.Objects.Search;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MovieBot.TMDB.Objects.Responses
{
    public class SearchTVShowResponse : BaseResponse
    {
        [JsonProperty("results")]
        public List<SearchTVShow> results { get; set; }
    }
}
