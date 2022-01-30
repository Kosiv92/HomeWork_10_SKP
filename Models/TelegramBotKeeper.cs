using System;
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
    public class TelegramBotKeeper
    {
        #region Поля

        /// <summary>
        /// Токен для подключения приложения к соответствующему боту Telegram
        /// </summary>
        readonly string token;

        /// <summary>
        /// Телеграм-бот с которым осуществляется взаимодействие
        /// </summary>
        readonly TelegramBotClient _bot;

        /// <summary>
        /// Репозиторий для хранения файлов
        /// </summary>
        readonly Repository _repository;

        int numberOfFile = 0;

        /// <summary>
        /// Обработчик входящих (от телеграм-бота) обновлений
        /// </summary>
        readonly MessageHandler _messageHandler;

        /// <summary>
        /// Список клиентов (осуществляющих взаимодействие через чат) телеграм-бота
        /// </summary>
        private Dictionary<long, TelegramClient> _telegramClients ;

        public MainWindow mainWindow;

        #endregion

        #region Свойства

        /// <summary>
        /// Телеграм-бот с которым осуществляется взаимодействие
        /// </summary>
        public TelegramBotClient Bot { get => _bot; }

        /// <summary>
        /// Репозиторий для хранения файлов
        /// </summary>
        public Repository Repository { get => _repository; }

        public MessageHandler MessageHandler { get => _messageHandler; }

        public Dictionary<long, TelegramClient> TelegramClients
        {
            get { return _telegramClients; }
            set { _telegramClients = value; }
        }

        #endregion

        #region Константы

        const string VideoText = "Video";
        const string MusicText = "Music";
        const string SchoolText = "School";
        const string ListText = "ShowList";
        const string WeatherText = "Weather";
        const string UploadText = "Upload";


        #endregion

        public TelegramBotKeeper(string token, MainWindow mainWindow)
        {
            this.token = token;

            _bot = new TelegramBotClient(token);

            _messageHandler = new MessageHandler(this);

            TelegramClients = new Dictionary<long, TelegramClient>();

            _repository = _messageHandler.IOHelper.Repository;

            this.mainWindow = mainWindow;

        }


        /// <summary>
        /// Метод запуска приема обновлений от клиентов
        /// </summary>
        public void StartReceiveUpdates()
        {
            using var cts = new CancellationTokenSource();
            Bot.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), cts.Token);
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

            if (!_telegramClients.ContainsKey(update.Message.Chat.Id))
            {

            }
            _telegramClients[update.Message.Chat.Id] = new TelegramClient(update.Message.Chat.Username, update.Message.Chat.Id);


            _messageHandler.ServeUpdate(update);

        }

    }
}
