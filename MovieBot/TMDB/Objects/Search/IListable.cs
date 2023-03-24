using System;

namespace MovieBot.TMDB.Objects.Search
{
    public interface IListable
    {
        int getID();

        string getName();

        DateTime? getReleaseDate();

        float getRating();

        MediaType getMediaType();

        string getEmoji();
    }
}
