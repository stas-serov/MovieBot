using System.Collections.Generic;

namespace MovieBot.Internationalization
{
    public static class Dictionaries
    {
        public static Dictionary<string, string> dictionaryUkr = new Dictionary<string, string>()
        {
            {Constants.popularStr, "Популярні"},
            {Constants.topRatedStr, "Рейтингові"},
            {Constants.nowPlayingStr, "Зараз в ефірі"},
            {Constants.upcomingStr, "Очікувані"},
            {Constants.airingTodayStr, "Сьогодні в ефірі"},
            {Constants.onAirStr, "Зараз по телевізору"},
            {Constants.backStr, "Назад"},
            {Constants.searchStr, "Пошук"},
            {Constants.requestResults, "Результати за запитом"},
            {Constants.moviesStr, "Фільми"},
            {Constants.tvShowsStr, "Серіали"},
            {Constants.languageStr, "Мова"},
            {Constants.informationStr, "Інформація"},
            {Constants.chooseOptionStr, "Оберіть опцію"},
            {Constants.inputNameStr, "Введіть назву"},
            {Constants.chooseCategoryStr, "Оберіть категорію"},
            {Constants.chooseLanguageStr, "Оберіть мову"},
            {Constants.unknownCommandStr, "Невідома команда!"},
            {Constants.unknownCategoryStr, "Невідома категорія!"},
            {Constants.unknownLanguageStr, "Невідома мова!"},
            {Constants.previousStr, "Попередня"},
            {Constants.nextStr, "Наступна"},
            {Constants.pageStr, "Сторінка"},
            {Constants.ukrainianStr, "Українська"}
        };

        public static Dictionary<string, string> dictionaryEng = new Dictionary<string, string>()
        {
            {Constants.popularStr, "Popular"},
            {Constants.topRatedStr, "Top rated"},
            {Constants.nowPlayingStr, "Now playing"},
            {Constants.upcomingStr, "Upcoming"},
            {Constants.airingTodayStr, "Airing today"},
            {Constants.onAirStr, "On air"},
            {Constants.backStr, "Back"},
            {Constants.searchStr, "Search"},
            {Constants.requestResults, "Request results"},
            {Constants.moviesStr, "Movies"},
            {Constants.tvShowsStr, "TV Shows"},
            {Constants.languageStr, "Language"},
            {Constants.informationStr, "Information"},
            {Constants.chooseOptionStr, "Choose the option"},
            {Constants.inputNameStr, "Input the name"},
            {Constants.chooseCategoryStr, "Choose the category"},
            {Constants.chooseLanguageStr, "Choose the language"},
            {Constants.unknownCommandStr, "Unknown command!"},
            {Constants.unknownCategoryStr, "Unknown category!"},
            {Constants.unknownLanguageStr, "Unknown language!"},
            {Constants.previousStr, "Previous"},
            {Constants.nextStr, "Next"},
            {Constants.pageStr, "Page"},
            {Constants.englishStr, "English"}
        };
    }
}
