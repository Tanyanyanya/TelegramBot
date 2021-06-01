using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;
using TelegramBot.Commands;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TelegramBot
{
    class Program
    {
        static TelegramBotClient Bot;

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1837406629:AAH4U60V98FdQsoN5WqUyxTpSVjfMNvFf78");
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += Bot_OnCallbackQueryReceived;

            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void Bot_OnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text)
                return;

            string name = $"{message.From.FirstName}{message.From.LastName}";
            Console.WriteLine($"{name} отправил сообщение {message.Text}");

            switch (message.Text)
            {
                case "/start":
                    string text = @"
Hey! This is an Astronomical Bot that will help you find some information about space stuff.
Here are some commands you can enter to get a response:
/picture_of_the_day - To get a cosmic picture of the day
/asteroids_base - To get a list of asteroids
/Mars_Exploration - For information about Mars exploration
/Type_of_eclipse - To get information about the types of eclipse
/Weather_info - Types and explanation of space weather
If you want to find photo by tag write <i>Search *your tag*</i>. <b>Use capital letters</b>. Example: Search Sun";
                    await Bot.SendTextMessageAsync(message.From.Id, text, parseMode: ParseMode.Html);
                    break;
                case "/asteroids_base":
                    var asterBase = await AstroClient.GetAsteroidsFromDb();
                    await ShowAsteroids(message.From.Id, asterBase);
                    break;
                case "/picture_of_the_day":
                    var picOfDay = await AstroClient.GetPicOfDay();
                    await ShowPictureOfTheDay(message.From.Id, picOfDay);
                    break;
                case "/Mars_Exploration":
                    var marsInfo = await AstroClient.GetMarsExplorInfoFromDb();
                    await ShowMarsInfo(message.From.Id, marsInfo);
                    break;
                case "/Type_of_eclipse":
                    var eclipseType = await AstroClient.GetEclipseTypes();
                    await ShowEclipseTypes(message.From.Id, eclipseType);
                    break;
                case "/Weather_info":
                    var weatherTypes = await AstroClient.GetSpaceWeatherTypes();
                    await ShowWeatherTypes(message.From.Id, weatherTypes);
                    break;
                case "/keyboard":
                    var InlineKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("/asteroids_base"),
                            new KeyboardButton("/picture_of_the_day")

                        },
                        new[]
                        {
                            new KeyboardButton("/Mars_Exploration"),
                            new KeyboardButton("/Type_of_eclipse")
                        }
                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "text", replyMarkup: InlineKeyboard);
                    break;
                default:
                    try
                    {
                        await AdditionalCommand(e);
                    }
                    catch(Exception)
                    {
                        string errorText = "Error. Check the correctness of the entered data or try anoter one";
                        await Bot.SendTextMessageAsync(message.From.Id, errorText);
                    }
                    break;

            }

        }

        public static async Task AdditionalCommand(MessageEventArgs e)
        {
            var message = e.Message;
            if (message.Text.ToLower().Trim().StartsWith("asteroid"))
            {
                await AsteroidSearch(message.Text, e);
                return;
            }
            else if (message.Text.ToLower().Trim().StartsWith("eclipse"))
            {
                await EclipseInCity(message.Text, e);
            }
            else if (message.Text.ToLower().Trim().StartsWith("weather"))
            {
                await NextWeather(message.Text, e);
            }
            else if (message.Text.ToLower().Trim().StartsWith("search"))
            {
                await SearchPhoto(message.Text, e); /////////////
            }
            else
            {
                await Bot.SendTextMessageAsync(message.From.Id, "<b>Unknown command</b>", parseMode: ParseMode.Html);
            }
        }






        public static async Task ShowPictureOfTheDay(int id, PicOfDayModel picture)
        {
            if (picture == null)
            {
                string text = "<b>No data</b>";
                await Bot.SendTextMessageAsync(id, text, parseMode: ParseMode.Html);
                return;
            }

            string dayPicture = $"<b>{picture.Title}</b> " +
                $"\n{picture.Explanation}";

            string thisPhoto = $"{picture.Hdurl}";



            await Bot.SendPhotoAsync(id, photo: thisPhoto, caption: dayPicture, parseMode: ParseMode.Html);

        }



        public static async Task ShowAsteroids(int id, List<AsteroidsResponse> asteroidsList)
        {
            if (asteroidsList == null)
            {
                string text = "<b>No data</b>";
                await Bot.SendTextMessageAsync(id, text, parseMode: ParseMode.Html);
                return;
            }

            string ListOfasteroids = $"There is list of Asteroids based on their closest approach date to Earth: " +
                $"\n   Name -   ID   -  Magnitude ";

            foreach (var asteroid in asteroidsList)
            {
                string asteroids = $"\n💫 <i>{asteroid.name} - {asteroid.id} - {asteroid.magnitude} </i>";
                ListOfasteroids += asteroids;
            }

            string _text = $"You can find a specific asteroid by its ID if you enter \"Asteroid with id <b>*ID*</b>\" ";

            await Bot.SendTextMessageAsync(id, ListOfasteroids, parseMode: ParseMode.Html);
            await Bot.SendTextMessageAsync(id, _text, parseMode: ParseMode.Html);
        }




        public static async Task ShowMarsInfo(int id, List<MarsExplorationModel> marsInfoList)
        {
            if (marsInfoList == null)
            {
                string text = "<b>No data</b>";
                await Bot.SendTextMessageAsync(id, text, parseMode: ParseMode.Html);
                return;
            }

            string vocab = $"You can find a chronology of Mars exploration below ";

            foreach (var info in marsInfoList)
            {
                string wordItem = $"\n🔎<b><i>{info.Date} </i></b>" +
                    $"\n{info.Description}" +
                    $"\n ";
                vocab += wordItem;
            }

            await Bot.SendTextMessageAsync(id, vocab, parseMode: ParseMode.Html);
        }







        public static async Task AsteroidSearch(string enterId, MessageEventArgs eve)
        {

            string requestId = enterId.Substring(16).Trim();


            var response = await AstroClient.GetTranslate($"{requestId}");

            if (response == null)
            {
                await Bot.SendTextMessageAsync(eve.Message.From.Id, @"<b>I can't find asteroid whith that id(</b>", parseMode: ParseMode.Html);
                return;
            }

            string text = $"Here is the asteroid you were looking for" +
                $"\n<i>{(requestId).ToUpper()}</i> - <b>{response.name} - {response.magnitude}</b>";

            await Bot.SendTextMessageAsync(eve.Message.From.Id, text, parseMode: ParseMode.Html);
        }


        public static async Task ShowEclipseTypes(int id, List<TypeOfEclipseResponse> eclipseInfoList)
        {
            if (eclipseInfoList == null)
            {
                string text = "<b>No data</b>";
                await Bot.SendTextMessageAsync(id, text, parseMode: ParseMode.Html);
                return;
            }

            string eclipseData = $"Here are all the types of lunar eclipses that scientists have identified: ";

            foreach (var type in eclipseInfoList)
            {
                string eclipseInfo = $"\n\n<b><i>{type.Name} </i></b>" +
                    $"\n{type.Info}" +
                    $"\n{type.Url}";
                eclipseData += eclipseInfo;
            }

            string _text = $"You can find out the next lunar eclipse and the quality of visibility in a particular city" +
                $"\n Just write me \"Eclipse in <b>*City*</b>\"" +
                $"\n\n<b>Warning!</b> Write only capital with a capital letter";

            await Bot.SendTextMessageAsync(id, eclipseData, parseMode: ParseMode.Html);
            await Bot.SendTextMessageAsync(id, _text, parseMode: ParseMode.Html);
        }

        public static async Task EclipseInCity(string city, MessageEventArgs eve)
        {

            string requestCity = city.Substring(10).Trim();


            var eclipse = await AstroClient.GetEclipseInCity($"{requestCity}");

            if (eclipse == null)
            {
                await Bot.SendTextMessageAsync(eve.Message.From.Id, @"<b>I can't find information about that city. Can you try another one?(</b>", parseMode: ParseMode.Html);
                return;
            }

            string text = $"The next eclipse in <b>{(requestCity).ToUpper()}</b> will be in <b>{eclipse.Date}</b>" +
                $"\nType of lunar eclipse: { eclipse.Type}" +
                $"\n " +
                $"\n Here is information about the visibility of the eclipse phases:" +
                $"\n Penumbral Exlipse Begins: { eclipse.Pen_ecl_beg}" +
                $"\n Partiar Exlipse Begins:    { eclipse.Par_ecl_beg}" +
                $"\n Total Exlipse Begins:       { eclipse.Tot_ecl_beg}" +
                $"\n Middle Exlipse:                 { eclipse.Middle_ecl}" +
                $"\n Total Exlipse Ends:         { eclipse.Tot_ecl_end}" +
                $"\n Partiar Exlipse Ends:       { eclipse.Par_ecl_end}" +
                $"\n Penumbral Exlipse Ends:    { eclipse.Pen_ecl_end}" +
                $"";

            await Bot.SendTextMessageAsync(eve.Message.From.Id, text, parseMode: ParseMode.Html);
        }


        public static async Task ShowWeatherTypes(int id, List<SpaceWeatherResponse> weatherInfoList)
        {
            if (weatherInfoList == null)
            {
                string text = "<b>No data</b>";
                await Bot.SendTextMessageAsync(id, text, parseMode: ParseMode.Html);
                return;
            }

            string weatherType = $"Here are all weather types: ";

            foreach (var type in weatherInfoList)
            {
                string weatherInfo = $"\n\n<b><i>{type.Name}  {type.FullName}</i></b>" +
                    $"\n{type.Info}";
                weatherType += weatherInfo;
            }
            string _text = $"You can find out about the nearest space weather if you enter" +
               $"\n \"Weather notification <b>*Type of weather*</b>\"" +
               $"\n\n<b>Warning!</b> For type use short name" +
               $"\nFor example:" +
               $"\nWeather notification CME";

            await Bot.SendTextMessageAsync(id, weatherType, parseMode: ParseMode.Html);
            await Bot.SendTextMessageAsync(id, _text, parseMode: ParseMode.Html);
        }



        public static async Task NextWeather(string type, MessageEventArgs eve)
        {

            string requestType = type.Substring(20).Trim();


            var spaceWeather = await AstroClient.GetNextWeatherByType($"{requestType}");

            if (spaceWeather == null)
            {
                await Bot.SendTextMessageAsync(eve.Message.From.Id, @"<b>I can't find information about that weather type. Can you try another one?</b>", parseMode: ParseMode.Html);
                return;
            }

            string text = $"Next <b>{(spaceWeather).FullName}</b> will be in <b>{spaceWeather.IssueTime}</b>" +
                $"\nHere is some information provided by special services" +
                $"\n " +
                $"\n {spaceWeather.MessageBody}";

            await Bot.SendTextMessageAsync(eve.Message.From.Id, text, parseMode: ParseMode.Html);
        }

        public static async Task SearchPhoto(string tag, MessageEventArgs eve)
        {

            string requestTag = tag.Substring(6).Trim();


            var photo = await AstroClient.GetPhotoByTag($"{requestTag}");

            if (photo == null)
            {
                await Bot.SendTextMessageAsync(eve.Message.From.Id, @"<b>I can't find photo with this tag. Can you try another one?(</b>", parseMode: ParseMode.Html);
                return;
            }

            string text = $"For #<b>{(photo).Tag}</b> I have this photo";

            string thisPhoto = $"{photo.Url}";

            await Bot.SendPhotoAsync(chatId: eve.Message.From.Id, photo: thisPhoto,caption: text, parseMode: ParseMode.Html) ;
        }


    }
}
