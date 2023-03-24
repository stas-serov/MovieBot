using System;
namespace MovieBot
{
    public class User
    {
        public long id { get; set; }
        public string firstName { get; set; }
        public string? lastName { get; set; }
        public string? username { get; set; }
        public UserState userState { get; set; }
        public UserLanguage userLanguage { get; set; }
        public DateTime firstTimeActivity { get; set; }
        public DateTime lastTimeActivity { get; set; }

        public User(long id, string firstName, string? lastName, string? username, UserState userState, UserLanguage userLanguage,
            DateTime firstTimeActivity, DateTime lastTimeActivity) 
        { 
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.userState = userState;
            this.userLanguage = userLanguage;
            this.firstTimeActivity = firstTimeActivity;
            this.lastTimeActivity = lastTimeActivity;
        }
    }
    public enum UserState
    {
        Unknown,
        MainMenu,
        Search,
        Movies,
        TVShows,
        LanguageChange
    }
    public enum UserLanguage
    {
        Unknown,
        uk,
        en,
    }
}
