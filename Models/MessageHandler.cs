using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HomeWork_10_SKP
{
    public class MessageHandler
    {
        readonly InputOutputHelper iOHelper;

        internal Dictionary<long, ClientState> _userState;

        readonly TelegramBot _telegramBot;

        public MessageHandler(TelegramBot bot)
        {
            _telegramBot = bot;
            
            iOHelper = new InputOutputHelper(Repository.PathToRepository);

            _userState = new Dictionary<long, ClientState>();
        }

        /// <summary>
        /// Метод обработки обновлений от клиентов
        /// </summary>
        /// <param name="update">Входящее обновление от клиента</param>
        public void MessageProcess(Update update, TelegramClient client)
        {
            switch (update.Message.Type)
            {
                case MessageType.Photo:
                    iOHelper.Download(_telegramBot.BotClient, update);
                    break;
                case MessageType.Document:
                    iOHelper.Download(_telegramBot.BotClient, update);
                    break;
                case MessageType.Audio:
                    iOHelper.Download(_telegramBot.BotClient, update);
                    break;
                    case MessageType.Text:
                    TextHandler(update);
                    break;

            }
        }

        /// <summary>
        /// Метод обработки текстовых сообщений от клиентов
        /// </summary>
        /// <param name="update"></param>
        private void TextHandler(Update update)
        {
            if (_userState.ContainsKey(update.Message.Chat.Id) && (_userState[update.Message.Chat.Id].isWeatherSearchOn == true || _userState[update.Message.Chat.Id].isFileSendOn == true))
            {
                if (_userState[update.Message.Chat.Id].isWeatherSearchOn == true)
                {
                    WeatherHelper.RequestHandler(update);
                }
                else if (_userState[update.Message.Chat.Id].isFileSendOn == true)
                {
                    UploadHandler(update);
                }
            }
            else
            {
                _telegramBot.SendTextMessageAsync(update.Message.Chat.Id, text: "Choose action", replyMarkup: GetButtons());
                switch (update.Message.Text)
                {                    
                    case ListText:
                        _botClient.SendTextMessageAsync(update.Message.Chat.Id, text: GetFileList(update));
                        break;
                    case WeatherText:
                        TurnOnWeatherSearch(update);
                        break;
                    case UploadText:
                        TurnOnFileUpload(update);
                        break;
                }
            }
        }  
        

    }
}
