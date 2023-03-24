using System;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Generic;
using System.Linq;
using MovieBot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;
using System.Text.RegularExpressions;
using MovieBot.Internationalization;
using MovieBot.Database;
using MovieBot.TMDB.Clients;
using MovieBot.TMDB.Objects.Credits;
using MovieBot.TMDB.Objects.Movies;
using MovieBot.TMDB.Objects.TVShows;
using MovieBot.TMDB.Objects.Search;

public class Program
{
    private static TelegramBotClient client;

    private static TMDBClient tmdbClient;

    private static MySQLDatabase database;

    private enum ListType
    {
        Search,
        Movies,
        TVShows,
    }

    private static void Main(string[] args)
    {
        tmdbClient = new TMDBClient(Constants.tmdbClientApiKey);
        database = new MySQLDatabase();
        CancellationTokenSource cts = new CancellationTokenSource();
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
        };
        client = new TelegramBotClient(Constants.tgToken);
        client.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token);
        Console.ReadLine();
        cts.Cancel();
    }

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        MovieBot.User? user = null;
        switch (update.Type)
        {
            case UpdateType.Message:
                Message? message = update.Message;
                if (message is null) break;
                user = await database.GetUser(message.From.Id);
                if (user is null)
                {
                    user = new MovieBot.User(message.From.Id, message.From.FirstName, message.From.LastName, message.From.Username, UserState.Unknown, UserLanguage.uk, DateTime.Now, DateTime.Now);
                    database.AddUser(user);
                }
                if (message?.Text?.ToLower() == "/start")
                {
                    if (user.userLanguage == UserLanguage.uk)
                        await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsUkr());
                    else
                        await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsEng());
                    user.userState = UserState.MainMenu;
                    break;
                }
                switch (user.userState)
                {
                    case UserState.MainMenu:
                        if (message.Text is null)
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsEng());
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.searchStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.searchStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.inputNameStr], replyMarkup: SearchMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.inputNameStr], replyMarkup: SearchMenuButtonsEng());
                            user.userState = UserState.Search;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.moviesStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.moviesStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseCategoryStr], replyMarkup: MoviesMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseCategoryStr], replyMarkup: MoviesMenuButtonsEng());
                            user.userState = UserState.Movies;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.tvShowsStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.tvShowsStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseCategoryStr], replyMarkup: TVShowsMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseCategoryStr], replyMarkup: TVShowsMenuButtonsEng());
                            user.userState = UserState.TVShows;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.languageStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.languageStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseLanguageStr], replyMarkup: LanguageMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseLanguageStr], replyMarkup: LanguageMenuButtonsEng());
                            user.userState = UserState.LanguageChange;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.informationStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.informationStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, "Даний бот створений для пошуку рекомендацій щодо фільмів та серіалів\r\nКнопка \"Пошук\" для " +
                                    "пошуку фільмів та серіалів за назвою.\r\nКнопка \"Фільми\" для пошук фільмів за категорією.\r\nКнопка \"Серіали\" для пошуку серіалів за " +
                                    "категорією.\r\nКнопка \"Інформація\" для виведення інформації щодо користування ботом.\r\nКнопка \"Мова\" для зміни мови інтерфейсу.", 
                                    parseMode: ParseMode.Html);
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, "The bot was created to search movie and TV show recommendations.\r\nButton \"Search\" to search " +
                                    "movies and TV shows by title.\r\nButton \"Movies\" to search movies by category.\r\nButton \"TV Shows\" to search TV shows by category.\r\nButton " +
                                    "\"Information\" to see information about bot.\r\nButton \"Language\" to change bot language.", parseMode: ParseMode.Html);
                        }
                        else
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.unknownCommandStr]);
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.unknownCommandStr]);
                        }
                        break;
                    case UserState.Search:
                        if(message.Text is null)
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseCategoryStr], replyMarkup: TVShowsMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseCategoryStr], replyMarkup: TVShowsMenuButtonsEng());
                            break;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.backStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.backStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsEng());
                            user.userState = UserState.MainMenu;
                            break;
                        }
                        List<IListable> searchItems = new List<IListable>();
                        searchItems.AddRange(await tmdbClient.SearchMoviesAsync(Constants.maxNumberOfContent, message.Text, user.userLanguage.ToString()));
                        searchItems.AddRange(await tmdbClient.SearchTVShowsAsync(Constants.maxNumberOfContent, message.Text, user.userLanguage.ToString()));
                        searchItems = searchItems.Take(Constants.numberOfContentToShow).ToList();
                        int pageNumber = 1;
                        if (user.userLanguage == UserLanguage.uk)
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, $"{Dictionaries.dictionaryUkr[Constants.requestResults]} \"{message.Text}\"");
                            if(searchItems.Count > 0)
                                await client.SendTextMessageAsync(message.Chat.Id, BuildStringToShowList(searchItems, pageNumber), replyMarkup: BuildInlineKeyboardUkr(false, true, pageNumber, searchItems, ListType.Search, message.Text), parseMode: ParseMode.Html);
                        } 
                        else
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, $"{Dictionaries.dictionaryEng[Constants.requestResults]} \"{message.Text}\"");
                            if(searchItems.Count > 0)
                                await client.SendTextMessageAsync(message.Chat.Id, BuildStringToShowList(searchItems, pageNumber), replyMarkup: BuildInlineKeyboardEng(false, true, pageNumber, searchItems, ListType.Search, message.Text), parseMode: ParseMode.Html);
                        }
                        break;
                    case UserState.Movies:
                        if (message.Text is null)
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseCategoryStr], replyMarkup: MoviesMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseCategoryStr], replyMarkup: MoviesMenuButtonsEng());
                            break;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.backStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.backStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsEng());
                            user.userState = UserState.MainMenu;
                            break;
                        }
                        int pageNumberMovies = 1;
                        string moviesCategory;
                        List<IListable> searchMovies = new List<IListable>();
                        if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.nowPlayingStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.nowPlayingStr]))
                        {
                            searchMovies.AddRange(await tmdbClient.GetNowPlayingMoviesAsync(Constants.numberOfContentToShow, user.userLanguage.ToString()));
                            moviesCategory = Constants.nowPlayingStr;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.popularStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.popularStr]))
                        {
                            searchMovies.AddRange(await tmdbClient.GetPopularMoviesAsync(Constants.numberOfContentToShow, user.userLanguage.ToString()));
                            moviesCategory = Constants.popularStr;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.topRatedStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.topRatedStr]))
                        {
                            searchMovies.AddRange(await tmdbClient.GetTopRatedMoviesAsync(Constants.numberOfContentToShow, user.userLanguage.ToString()));
                            moviesCategory = Constants.topRatedStr; ;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.upcomingStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.upcomingStr]))
                        {
                            searchMovies.AddRange(await tmdbClient.GetUpcomingMoviesAsync(Constants.numberOfContentToShow, user.userLanguage.ToString()));
                            moviesCategory = Constants.upcomingStr;
                        }
                        else
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.unknownCategoryStr]);
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.unknownCategoryStr]);
                            break;
                        }
                        string textToSendMovies = BuildStringToShowList(searchMovies, pageNumberMovies);
                        if (user.userLanguage == UserLanguage.uk)
                            await client.SendTextMessageAsync(message.Chat.Id, $"<b>{Dictionaries.dictionaryUkr[Constants.moviesStr]} \"{Dictionaries.dictionaryUkr[moviesCategory]}\"</b>\n\n{textToSendMovies}", replyMarkup: BuildInlineKeyboardUkr(false, true, pageNumberMovies, searchMovies, ListType.Movies, moviesCategory), parseMode: ParseMode.Html);
                        else
                            await client.SendTextMessageAsync(message.Chat.Id, $"<b>{Dictionaries.dictionaryEng[moviesCategory]} {Dictionaries.dictionaryEng[Constants.moviesStr].ToLower()}</b>\n\n{textToSendMovies}", replyMarkup: BuildInlineKeyboardEng(false, true, pageNumberMovies, searchMovies, ListType.Movies, moviesCategory), parseMode: ParseMode.Html);
                        break;
                    case UserState.TVShows:
                        if (message.Text is null)
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseCategoryStr], replyMarkup: TVShowsMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseCategoryStr], replyMarkup: TVShowsMenuButtonsEng());
                            break;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.backStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.backStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsEng());
                            user.userState = UserState.MainMenu;
                            break;
                        }
                        int pageNumberTVShows = 1;
                        string tvShowsCategory;
                        List<IListable> searchTVShows = new List<IListable>();
                        if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.airingTodayStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.airingTodayStr]))
                        {
                            searchTVShows.AddRange(await tmdbClient.GetAiringTodayTVShowsAsync(Constants.numberOfContentToShow, user.userLanguage.ToString()));
                            tvShowsCategory = Constants.airingTodayStr;

                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.popularStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.popularStr]))
                        {
                            searchTVShows.AddRange(await tmdbClient.GetPopularTVShowsAsync(Constants.numberOfContentToShow, user.userLanguage.ToString()));
                            tvShowsCategory = Constants.popularStr;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.topRatedStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.topRatedStr]))
                        {
                            searchTVShows.AddRange(await tmdbClient.GetTopRatedTVShowsAsync(Constants.numberOfContentToShow, user.userLanguage.ToString()));
                            tvShowsCategory = Constants.topRatedStr;
                        }
                        else if (message.Text.Contains(Dictionaries.dictionaryUkr[Constants.onAirStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.onAirStr]))
                        {
                            searchTVShows.AddRange(await tmdbClient.GetOnAirTVShowsAsync(Constants.numberOfContentToShow, user.userLanguage.ToString()));
                            tvShowsCategory = Constants.onAirStr;
                        }
                        else
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.unknownCategoryStr]);
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.unknownCategoryStr]);
                            break;
                        }
                        string textToSendTVShows = BuildStringToShowList(searchTVShows, pageNumberTVShows);
                        if (user.userLanguage == UserLanguage.uk)
                            await client.SendTextMessageAsync(message.Chat.Id, $"<b>{Dictionaries.dictionaryUkr[Constants.tvShowsStr]} \"{Dictionaries.dictionaryUkr[tvShowsCategory]}\"</b>\n\n{textToSendTVShows}", replyMarkup: BuildInlineKeyboardUkr(false, true, pageNumberTVShows, searchTVShows, ListType.TVShows, tvShowsCategory), parseMode: ParseMode.Html);
                        else
                            await client.SendTextMessageAsync(message.Chat.Id, $"<b>{Dictionaries.dictionaryEng[tvShowsCategory]} {Dictionaries.dictionaryEng[Constants.tvShowsStr].ToLower()}</b>\n\n{textToSendTVShows}", replyMarkup: BuildInlineKeyboardEng(false, true, pageNumberTVShows, searchTVShows, ListType.TVShows, tvShowsCategory), parseMode: ParseMode.Html);
                        break;
                    case UserState.LanguageChange:
                        if(message.Text is null)
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseLanguageStr], replyMarkup: LanguageMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseLanguageStr], replyMarkup: LanguageMenuButtonsEng());
                            break;
                        }
                        else if(message.Text.Contains(Dictionaries.dictionaryUkr[Constants.backStr]) || message.Text.Contains(Dictionaries.dictionaryEng[Constants.backStr]))
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsUkr());
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsEng());
                        }
                        else if(message.Text.Contains(Dictionaries.dictionaryUkr[Constants.ukrainianStr]))
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsUkr());
                            user.userLanguage = UserLanguage.uk;
                        }
                        else if(message.Text.Contains(Dictionaries.dictionaryEng[Constants.englishStr]))
                        {
                            await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.chooseOptionStr], replyMarkup: MainMenuButtonsEng());
                            user.userLanguage = UserLanguage.en;
                        }
                        else
                        {
                            if (user.userLanguage == UserLanguage.uk)
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryUkr[Constants.unknownLanguageStr]);
                            else
                                await client.SendTextMessageAsync(message.Chat.Id, Dictionaries.dictionaryEng[Constants.unknownLanguageStr]);
                            break;
                        }
                        user.userState = UserState.MainMenu;
                        break;
                }
                user.username = message.From.Username;
                user.firstName = message.From.FirstName;
                user.lastName = message.From.LastName;
                user.lastTimeActivity = DateTime.Now;
                break;
            case UpdateType.CallbackQuery:
                CallbackQuery? callbackQuery = update.CallbackQuery;
                user = await database.GetUser(callbackQuery.From.Id);
                if (user is null)
                {
                    user = new MovieBot.User(callbackQuery.From.Id, callbackQuery.From.FirstName, callbackQuery.From.LastName, callbackQuery.From.Username, UserState.Unknown, UserLanguage.uk, DateTime.Now, DateTime.Now);
                    database.AddUser(user);
                }
                string[]? command = callbackQuery?.Data?.Split("/");
                if(command?[0] == MediaType.Movie.ToString())
                {
                    int movieId = int.Parse(Regex.Replace(callbackQuery.Data, @"[^\d]", ""));
                    MovieToShow movieToShow = await GetMovieAsync(movieId, user.userLanguage.ToString());
                    if(user.userLanguage == UserLanguage.uk)
                        await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"{movieToShow.ToStringUkr()}", parseMode: ParseMode.Html);
                    else
                        await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"{movieToShow.ToStringEng()}", parseMode: ParseMode.Html);
                }
                else if(command?[0] == MediaType.TVShow.ToString())
                {
                    int tvShowId = int.Parse(Regex.Replace(callbackQuery.Data, @"[^\d]", ""));
                    TVShowToShow tvShowToShow = await GetTVShowAsync(tvShowId, user.userLanguage.ToString());
                    if(user.userLanguage == UserLanguage.uk)
                        await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"{tvShowToShow.ToStringUkr()}", parseMode: ParseMode.Html);
                    else
                        await client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"{tvShowToShow.ToStringEng()}", parseMode: ParseMode.Html);
                }
                else if ((command?[0] == ListType.Movies.ToString()) || (command?[0] == ListType.TVShows.ToString()))
                {
                    string category;
                    bool nextPageFlag = true;
                    bool prevPageFlag = true;
                    string textToSend;
                    int page = int.Parse(Regex.Replace(command[2], @"[^\d]", ""));
                    if((page * Constants.numberOfContentToShow) >= Constants.maxNumberOfContent) nextPageFlag = false;
                    if (page == 1) prevPageFlag = false;
                    if(command?[0] == ListType.Movies.ToString())
                    {
                        List<IListable> searchMovies = new List<IListable>();
                        if (command[1] == Constants.nowPlayingStr)
                        {
                            searchMovies.AddRange(await tmdbClient.GetNowPlayingMoviesAsync(Constants.numberOfContentToShow * page, user.userLanguage.ToString()));
                            category = command[1];
                        }
                        else if (command[1] == Constants.popularStr)
                        {
                            searchMovies.AddRange(await tmdbClient.GetPopularMoviesAsync(Constants.numberOfContentToShow * page, user.userLanguage.ToString()));
                            category = command[1];
                        }
                        else if (command[1] == Constants.topRatedStr)
                        {
                            searchMovies.AddRange(await tmdbClient.GetTopRatedMoviesAsync(Constants.numberOfContentToShow * page, user.userLanguage.ToString()));
                            category = command[1];
                        }
                        else if (command[1] == Constants.upcomingStr)
                        {
                            searchMovies.AddRange(await tmdbClient.GetUpcomingMoviesAsync(Constants.numberOfContentToShow * page, user.userLanguage.ToString()));
                            category = command[1];
                        }
                        else break;
                        searchMovies = searchMovies.Skip((page - 1) * Constants.numberOfContentToShow).ToList();
                        textToSend = BuildStringToShowList(searchMovies, page);
                        if(user.userLanguage == UserLanguage.uk)
                            await client.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, $"<b>{Dictionaries.dictionaryUkr[Constants.moviesStr]} \"{Dictionaries.dictionaryUkr[category]}\"</b>\n\n{textToSend}", replyMarkup: BuildInlineKeyboardUkr(prevPageFlag, nextPageFlag, page, searchMovies, ListType.Movies, category), parseMode: ParseMode.Html); 
                        else
                            await client.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, $"<b>{Dictionaries.dictionaryEng[category]} {Dictionaries.dictionaryEng[Constants.moviesStr].ToLower()}</b>\n\n{textToSend}", replyMarkup: BuildInlineKeyboardEng(prevPageFlag, nextPageFlag, page, searchMovies, ListType.Movies, category), parseMode: ParseMode.Html); 

                    }
                    else if(command?[0] == ListType.TVShows.ToString())
                    {
                        List<IListable> searchTVShows = new List<IListable>();
                        if (command[1] == Constants.airingTodayStr)
                        {
                            searchTVShows.AddRange(await tmdbClient.GetAiringTodayTVShowsAsync(Constants.numberOfContentToShow * page, user.userLanguage.ToString()));
                            category = command[1];
                        }
                        else if(command[1] == Constants.popularStr)
                        {
                            searchTVShows.AddRange(await tmdbClient.GetPopularTVShowsAsync(Constants.numberOfContentToShow * page, user.userLanguage.ToString()));
                            category = command[1];
                        }
                        else if(command[1] == Constants.topRatedStr)
                        {
                            searchTVShows.AddRange(await tmdbClient.GetTopRatedTVShowsAsync(Constants.numberOfContentToShow * page, user.userLanguage.ToString()));
                            category = command[1];
                        }
                        else if(command[1] == Constants.onAirStr)
                        {
                            searchTVShows.AddRange(await tmdbClient.GetOnAirTVShowsAsync(Constants.numberOfContentToShow * page, user.userLanguage.ToString()));
                            category = command[1];
                        }
                        else break;
                        searchTVShows = searchTVShows.Skip((page - 1) * Constants.numberOfContentToShow).ToList();
                        textToSend = BuildStringToShowList(searchTVShows, page);
                        if(user.userLanguage == UserLanguage.uk)
                            await client.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, $"<b>{Dictionaries.dictionaryUkr[Constants.tvShowsStr]} \"{Dictionaries.dictionaryUkr[category]}\"</b>\n\n{textToSend}", replyMarkup: BuildInlineKeyboardUkr(prevPageFlag, nextPageFlag, page, searchTVShows, ListType.TVShows, category), parseMode: ParseMode.Html);
                        else
                            await client.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, $"<b>{Dictionaries.dictionaryEng[category]} {Dictionaries.dictionaryEng[Constants.tvShowsStr].ToLower()}</b>\n\n{textToSend}", replyMarkup: BuildInlineKeyboardEng(prevPageFlag, nextPageFlag, page, searchTVShows, ListType.TVShows, category), parseMode: ParseMode.Html);

                    }
                }
                else if (command?[0] == ListType.Search.ToString())
                {
                    string query;
                    bool nextPageFlag = true;
                    bool prevPageFlag = true;
                    string textToSend;
                    int page = int.Parse(Regex.Replace(command[2], @"[^\d]", ""));
                    if (page == 1) prevPageFlag = false;
                    query = command[1];
                    List<IListable> searchItems = new List<IListable>();
                    searchItems.AddRange(await tmdbClient.SearchMoviesAsync(Constants.maxNumberOfContent, query, user.userLanguage.ToString()));
                    searchItems.AddRange(await tmdbClient.SearchTVShowsAsync(Constants.maxNumberOfContent, query, user.userLanguage.ToString()));
                    searchItems = searchItems.Take(Constants.maxNumberOfContent).ToList();
                    if((page * Constants.numberOfContentToShow) >= searchItems.Count) nextPageFlag = false;
                    searchItems = searchItems.Skip((page - 1) * Constants.numberOfContentToShow).Take(Constants.numberOfContentToShow).ToList();
                    textToSend = BuildStringToShowList(searchItems, page);
                    if(user.userLanguage == UserLanguage.uk)
                        await client.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, textToSend, replyMarkup: BuildInlineKeyboardUkr(prevPageFlag, nextPageFlag, page, searchItems, ListType.Search, query));
                    else
                        await client.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, textToSend, replyMarkup: BuildInlineKeyboardEng(prevPageFlag, nextPageFlag, page, searchItems, ListType.Search, query));

                }
                user.username = callbackQuery.From.Username;
                user.firstName = callbackQuery.From.FirstName;
                user.lastName = callbackQuery.From.LastName;
                user.lastTimeActivity = DateTime.Now;
                break;
        }
        database.UpdateUser(user);
    }

    private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.Message);
    }

    private static async Task<MovieToShow> GetMovieAsync(int movieId, string language)
    {
        MovieDetails movieDetails = await tmdbClient.GetMovieDetailsAsync(movieId, language);
        string title = movieDetails.title;
        string originalTitle = movieDetails.originalTitle;
        string posterPath = tmdbClient.GetPosterUrl(Constants.defaultPosterWidth, movieDetails.posterPath);
        DateTime? releaseDate = movieDetails.releaseDate;
        List<string> genres = movieDetails.genres.Select(x => x.name).ToList();
        int runtime = movieDetails.runtime;
        float rating = movieDetails.voteAverage;
        string description = movieDetails.overview;
        Credits credits = await tmdbClient.GetCreditsAsync("movie", movieId, language);
        List<string> directors = credits.crew.Where(x => x.job == "Director").Select(x => x.name).ToList();
        List<string> productionCountries = movieDetails.productionCountries.Select(x => x.name).ToList();
        List<string> cast = credits.cast.Select(x => x.name).ToList();
        Int128 budget = movieDetails.budget;
        return new MovieToShow(title, originalTitle, posterPath, releaseDate, genres, runtime, rating, description, directors, productionCountries, cast, budget);
    }

    private static async Task<TVShowToShow> GetTVShowAsync(int tvShowId, string language)
    {
        TVShowDetails tvShowDetails = await tmdbClient.GetTVShowDetailsAsync(tvShowId, language);
        string title = tvShowDetails.name;
        string originalTitle = tvShowDetails.originalName;
        string posterPath = tmdbClient.GetPosterUrl(Constants.defaultPosterWidth, tvShowDetails.posterPath);
        DateTime? firstEpisodeReleaseDate = tvShowDetails.firstAirDate;
        List<string> genres = tvShowDetails.genres.Select(x => x.name).ToList();
        int episodeRuntime = tvShowDetails.episodeRuntime.FirstOrDefault();
        float rating = tvShowDetails.voteAverage;
        string description = tvShowDetails.overview;
        Credits credits = await tmdbClient.GetCreditsAsync("tv", tvShowId, language);
        List<string> creators = tvShowDetails.createdBy.Select(x => x.name).ToList();
        List<string> productionCountries = tvShowDetails.productionCountries.Select(x => x.name).ToList();
        List<string> cast = credits.cast.Select(x => x.name).ToList();
        int numberOfSeasons = tvShowDetails.numberOfSeasons;
        int numberOfEpisodes = tvShowDetails.numberOfEpisodes;
        return new TVShowToShow(title, originalTitle, posterPath, firstEpisodeReleaseDate, genres, episodeRuntime, 
            rating, description, creators, productionCountries, cast, numberOfSeasons, numberOfEpisodes);
    }

    private static string BuildStringToShowList(List<IListable> list, int pageNumber)
    {
        int startIndex = (pageNumber * Constants.numberOfContentToShow) - (Constants.numberOfContentToShow - 1);
        StringBuilder stringBuilder = new StringBuilder();
        int i = startIndex;
        foreach (IListable item in list)
        {
            stringBuilder.AppendLine($"{item.getEmoji()} {i}. {item.getName()} ({item.getRating()}⭐, {item.getReleaseDate()?.Year.ToString()}🗓)");
            stringBuilder.AppendLine();
            i++;
        }
        return stringBuilder.ToString();
    }

    private static InlineKeyboardMarkup BuildInlineKeyboardUkr(bool prevPage, bool nextPage, int pageNumber, List<IListable> list, ListType listType, string query)
    {
        InlineKeyboardButton[] upperInlineKeyboard = new InlineKeyboardButton[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            upperInlineKeyboard[i] = InlineKeyboardButton.WithCallbackData(text: ((pageNumber * list.Count) - (list.Count - (i + 1))).ToString(), callbackData: $"{list[i].getMediaType()}/{list[i].getID()}");
        }
        List<InlineKeyboardButton> lowerInlineKeyboard = new List<InlineKeyboardButton>();
        if (prevPage)
        {
            lowerInlineKeyboard.Add(InlineKeyboardButton.WithCallbackData(text: Dictionaries.dictionaryUkr[Constants.previousStr], callbackData: $"{listType}/{query}/{Constants.pageStr}" + (pageNumber - 1).ToString()));
        }
        lowerInlineKeyboard.Add(InlineKeyboardButton.WithCallbackData(text: $"{Dictionaries.dictionaryUkr[Constants.pageStr]}: {pageNumber}", callbackData: $"{Constants.pageStr}{pageNumber}"));
        if (nextPage)
        {
            lowerInlineKeyboard.Add(InlineKeyboardButton.WithCallbackData(text: Dictionaries.dictionaryUkr[Constants.nextStr], callbackData: $"{listType}/{query}/{Constants.pageStr}" + (pageNumber + 1).ToString()));
        }
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            upperInlineKeyboard,
            lowerInlineKeyboard.ToArray()
        });
        return inlineKeyboard;
    }

    private static InlineKeyboardMarkup BuildInlineKeyboardEng(bool prevPage, bool nextPage, int pageNumber, List<IListable> list, ListType listType, string query)
    {
        InlineKeyboardButton[] upperInlineKeyboard = new InlineKeyboardButton[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            upperInlineKeyboard[i] = InlineKeyboardButton.WithCallbackData(text: ((pageNumber * list.Count) - (list.Count - (i + 1))).ToString(), callbackData: $"{list[i].getMediaType()}/{list[i].getID()}");
        }
        List<InlineKeyboardButton> lowerInlineKeyboard = new List<InlineKeyboardButton>();
        if (prevPage)
        {
            lowerInlineKeyboard.Add(InlineKeyboardButton.WithCallbackData(text: Dictionaries.dictionaryEng[Constants.previousStr], callbackData: $"{listType.ToString()}/{query}/{Constants.pageStr}" + (pageNumber - 1).ToString()));
        }
        lowerInlineKeyboard.Add(InlineKeyboardButton.WithCallbackData(text: $"{Dictionaries.dictionaryEng[Constants.pageStr]}: {pageNumber}", callbackData: $"{Constants.pageStr}{pageNumber}"));
        if (nextPage)
        {
            lowerInlineKeyboard.Add(InlineKeyboardButton.WithCallbackData(text: Dictionaries.dictionaryEng[Constants.nextStr], callbackData: $"{listType.ToString()}/{query}/{Constants.pageStr}" + (pageNumber + 1).ToString()));
        }
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            upperInlineKeyboard,
            lowerInlineKeyboard.ToArray()
        });
        return inlineKeyboard;
    }

    private static ReplyKeyboardMarkup MainMenuButtonsUkr()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🔎 {Dictionaries.dictionaryUkr[Constants.searchStr]}", $"🎥 {Dictionaries.dictionaryUkr[Constants.moviesStr]}"},
            new KeyboardButton[]{$"📺 {Dictionaries.dictionaryUkr[Constants.tvShowsStr]}", $"🌍 {Dictionaries.dictionaryUkr[Constants.languageStr]}"},
            new KeyboardButton[]{$"❓ {Dictionaries.dictionaryUkr[Constants.informationStr]}"}
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }

    private static ReplyKeyboardMarkup MainMenuButtonsEng()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🔎 {Dictionaries.dictionaryEng[Constants.searchStr]}", $"🎥 {Dictionaries.dictionaryEng[Constants.moviesStr]}"},
            new KeyboardButton[]{$"📺 {Dictionaries.dictionaryEng[Constants.tvShowsStr]}", $"🌍 {Dictionaries.dictionaryEng[Constants.languageStr]}"},
            new KeyboardButton[]{$"❓ {Dictionaries.dictionaryEng[Constants.informationStr]}"}
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }

    private static ReplyKeyboardMarkup SearchMenuButtonsUkr()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🔙 {Dictionaries.dictionaryUkr[Constants.backStr]}"},
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }

    private static ReplyKeyboardMarkup SearchMenuButtonsEng()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🔙 {Dictionaries.dictionaryEng[Constants.backStr]}"},
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }

    private static ReplyKeyboardMarkup MoviesMenuButtonsUkr()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🔥 {Dictionaries.dictionaryUkr[Constants.nowPlayingStr]}", $"📈 {Dictionaries.dictionaryUkr[Constants.popularStr]}"},
            new KeyboardButton[]{$"🔝 {Dictionaries.dictionaryUkr[Constants.topRatedStr]}", $"🔜 {Dictionaries.dictionaryUkr[Constants.upcomingStr]}"},
            new KeyboardButton[]{$"🔙 {Dictionaries.dictionaryUkr[Constants.backStr]}"}
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }

    private static ReplyKeyboardMarkup MoviesMenuButtonsEng()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🔥 {Dictionaries.dictionaryEng[Constants.nowPlayingStr]}", $"📈 {Dictionaries.dictionaryEng[Constants.popularStr]}"},
            new KeyboardButton[]{$"🔝 {Dictionaries.dictionaryEng[Constants.topRatedStr]}", $"🔜 {Dictionaries.dictionaryUkr[Constants.upcomingStr]}"},
            new KeyboardButton[]{$"🔙 {Dictionaries.dictionaryEng[Constants.backStr]}"}
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }
    private static ReplyKeyboardMarkup TVShowsMenuButtonsUkr()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🔥 {Dictionaries.dictionaryUkr[Constants.airingTodayStr]}", $"{Dictionaries.dictionaryUkr[Constants.popularStr]}"},
            new KeyboardButton[]{$"🔝 {Dictionaries.dictionaryUkr[Constants.topRatedStr]}", $"🔛 {Dictionaries.dictionaryUkr[Constants.onAirStr]}"},
            new KeyboardButton[]{$"🔙 {Dictionaries.dictionaryUkr[Constants.backStr]}"}
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }

    private static ReplyKeyboardMarkup TVShowsMenuButtonsEng()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🔥 {Dictionaries.dictionaryEng[Constants.airingTodayStr]}", $"{Dictionaries.dictionaryEng[Constants.popularStr]}"},
            new KeyboardButton[]{$"🔝 {Dictionaries.dictionaryEng[Constants.topRatedStr]}", $"🔛 {Dictionaries.dictionaryEng[Constants.onAirStr]}"},
            new KeyboardButton[]{$"🔙 {Dictionaries.dictionaryEng[Constants.backStr]}"}
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }
    private static ReplyKeyboardMarkup LanguageMenuButtonsUkr()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🇺🇦 {Dictionaries.dictionaryUkr[Constants.ukrainianStr]}"},
            new KeyboardButton[]{$"🇬🇧 {Dictionaries.dictionaryEng[Constants.englishStr]}"},
            new KeyboardButton[]{$"🔙 {Dictionaries.dictionaryUkr[Constants.backStr]}"},
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }

    private static ReplyKeyboardMarkup LanguageMenuButtonsEng()
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[]{$"🇺🇦 {Dictionaries.dictionaryUkr[Constants.ukrainianStr]}"},
            new KeyboardButton[]{$"🇬🇧 {Dictionaries.dictionaryEng[Constants.englishStr]}"},
            new KeyboardButton[]{$"🔙 {Dictionaries.dictionaryEng[Constants.backStr]}"},
        })
        {
            ResizeKeyboard = true
        };
        return replyKeyboardMarkup;
    }
}