﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;

namespace HomeWork_10_SKP
{
    public class TelegramBot
    {
        #region Поля

        readonly string token;

        readonly TelegramBotClient _botClient;

        int numberOfFile = 0;

        readonly MessageHandler messageHandler;

        private Dictionary<long, TelegramClient> _telegramClients;

        #endregion

        #region Свойства

        public TelegramBotClient BotClient { get => _botClient; }

        #endregion

        #region Константы

        const string VideoText = "Video";
        const string MusicText = "Music";
        const string SchoolText = "School";
        const string ListText = "ShowList";
        const string WeatherText = "Weather";
        const string UploadText = "Upload";
        const string PrevFileText = "<";
        const string NextFileText = ">";
        const string CancelText = "Cancel";

        #endregion

        public TelegramBot(string token)
        {
            this.token = token;

            _botClient = new TelegramBotClient(token);

            messageHandler = new MessageHandler(this);

            _telegramClients = new Dictionary<long, TelegramClient>();
        }




        /// <summary>
        /// Метод запуска приема обновлений от клиентов
        /// </summary>
        public void StartReceiveUpdates()
        {
            using var cts = new CancellationTokenSource();
            BotClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), cts.Token);
            Console.ReadLine();
            cts.Cancel();
        }

        /// <summary>
        /// Метод обнаружения ошибок и их вывода на экран консоли
        /// </summary>
        /// <param name="botClient">Бот получающий ошибку</param>
        /// <param name="exception">Ошибка(исключение)</param>
        /// <param name="cancellationToken">Токен прерывания</param>
        /// <returns></returns>        
        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            return Task.CompletedTask;
        }

        /// <summary>
        /// Метод обработки обновлений от пользователя
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            if (update.Type != UpdateType.Message) return;

            if(!_telegramClients.ContainsKey(update.Message.Chat.Id)) _telegramClients[update.Message.Chat.Id] = new TelegramClient(update.Message.Chat.Username, update.Message.Chat.Id);

            if (update.Message.Type != MessageType.Text) MessageHandler(update);
            else TextHandler(update);

        }
                
        /// <summary>
        /// Метод получения списка файлов в репозитории 
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        /// <returns></returns>
        private string GetFileList(Update update)
        {
            FileInfo[] files = Repository.GetFilesName();
            StringBuilder fileList = new StringBuilder();
            foreach (var file in files)
            {
                fileList.Append($"- {file.Name}\n");
            }

            return fileList.ToString();

        }

        /// <summary>
        /// Установка режима отправки данных о погоде клиенту
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        async private void TurnOnWeatherSearch(Update update)
        {
            if (_userState.ContainsKey(update.Message.Chat.Id)) _userState[update.Message.Chat.Id].WeatherSearchState = WeatherSearchState.IsOn;
            else _userState[update.Message.Chat.Id] = new ClientState { isWeatherSearchOn = WeatherSearchState.IsOn };
            //_userState[update.Message.Chat.Id] = new UserState { WeatherSearchState = WeatherSearchState.IsOn };

            await _botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, text: "Write name of city which weather you need to know!", replyMarkup: new ReplyKeyboardMarkup("Cancel"));
        }

        /// <summary>
        /// Обработчик запросов погоды
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        private void WeatherHandler(Update update)
        {

            if (update.Message.Text == CancelText)
            {
                _userState[update.Message.Chat.Id].WeatherSearchState = WeatherSearchState.IsOff;
                //_userState[update.Message.Chat.Id] = new UserState { WeatherSearchState = WeatherSearchState.IsOff };
                TextHandler(update);
            }
            else
            {
                SendWeatherForecast(update);
            }
        }

        /// <summary>
        /// Метод отправки клиенту данных о температуре
        /// </summary>
        /// <param name="update"></param>
        async private void SendWeatherForecast(Update update)
        {
            string temperature = WeatherHerald.WeatherRequest(update.Message.Text);
            await _botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, text: temperature);
        }

        /// <summary>
        /// Установка режима отправки файлов клиенту
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        async private void TurnOnFileUpload(Update update)
        {
            FileInfo[] files = Repository.GetFilesName();
            if (_userState.ContainsKey(update.Message.Chat.Id)) _userState[update.Message.Chat.Id].FileSendState = FileSendState.IsOn;
            else _userState[update.Message.Chat.Id] = new ClientState { isFileSendOn = FileSendState.IsOn };
            //_userState[update.Message.Chat.Id] = new UserState { FileSendState = FileSendState.IsOn };
            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, text: "Choose file to upload", replyMarkup: GetUploadButtons(files, numberOfFile));
        }

        /// <summary>
        /// Обработчик запросов на отправку файлов клиенту
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        async private void UploadHandler(Update update)
        {
            FileInfo[] files = Repository.GetFilesName();

            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, text: "Choose file to upload", replyMarkup: GetUploadButtons(files, numberOfFile));

            switch (update.Message.Text)
            {
                case CancelText:
                    _userState[update.Message.Chat.Id].FileSendState = FileSendState.IsOff;
                    //_userState[update.Message.Chat.Id] = new UserState { FileSendState = FileSendState.IsOff };
                    numberOfFile = 0;
                    TextHandler(update);
                    break;
                case PrevFileText:
                    if (numberOfFile > 0) numberOfFile--;
                    else numberOfFile = files.Length - 1;
                    break;
                case NextFileText:
                    if (numberOfFile < files.Length - 1) numberOfFile++;
                    else numberOfFile = 0;
                    break;
                case UploadText:
                    break;
                default:
                    //Upload(update.Message.Text, update.Message.Chat.Id);
                    Repository.Upload(_botClient, update.Message.Text, update.Message.Chat.Id);
                    break;
            }
        }


        /// <summary>
        /// Получение кнопок основного меню
        /// </summary>
        /// <returns></returns>
        private IReplyMarkup GetButtons()
        {

            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = VideoText }, new KeyboardButton { Text = MusicText } },
                    new List<KeyboardButton>{ new KeyboardButton { Text = SchoolText }, new KeyboardButton { Text = ListText } },
                    new List<KeyboardButton>{ new KeyboardButton { Text = WeatherText }, new KeyboardButton { Text = UploadText } }
                    },
                ResizeKeyboard = true
            };
        }


        /// <summary>
        /// Получение кнопок меню для скачивания файлов
        /// </summary>
        /// <param name="files">Список файлов репозитория</param>
        /// <param name="position">Номер файла в списке (выбор для скачивания)</param>
        /// <returns></returns>
        private IReplyMarkup GetUploadButtons(FileInfo[] files, int position)
        {
            string filename = files[position].Name;

            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = PrevFileText } },
                    new List<KeyboardButton>{ new KeyboardButton { Text = filename } },
                    new List<KeyboardButton>{ new KeyboardButton { Text = NextFileText } },
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Cancel" } }
                    },
                ResizeKeyboard = true
            };
        }
    }
}
