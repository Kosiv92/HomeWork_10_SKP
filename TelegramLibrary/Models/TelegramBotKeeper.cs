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

namespace TelegramLibrary
{
    public class TelegramBotKeeper : ITelegramBot
    {
        #region Fields

        /// <summary>
        /// Telegram-bot which is interacted with updates
        /// </summary>
        readonly TelegramBotClient _bot;

        /// <summary>
        /// Manager for interaction with clients of  Telegram-Bot
        /// </summary>
        private IClientManager _clientManager;

        /// <summary>
        /// Обработчик входящих обновлений
        /// </summary>
        private ITelegramUpdateHandler _updateHandler;

        /// <summary>
        /// Настройки разрешенных к обработке обновлений
        /// </summary>
        ReceiverOptions receiveOptions = new ReceiverOptions
        {
            AllowedUpdates = { } // receive all update types
        };

        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        #endregion

        #region Properties

        /// <summary>
        /// Condition for get access to telegram-bot
        /// </summary>
        public TelegramBotClient Bot { get => _bot; }

        /// <summary>
        /// Condition for get access to client manager
        /// </summary>
        public IClientManager ClientManager
        {
            get => _clientManager;
            set => _clientManager = value;
        }

        /// <summary>
        /// Свойство доступа к обработчику входящих обновлений
        /// </summary>
        public ITelegramUpdateHandler UpdateHandler
        {
            get
            {
                if (_updateHandler == null) throw new Exception("UpdateHandler is not defined");
                else return _updateHandler;
            }
            set { _updateHandler = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor of object
        /// </summary>
        /// <param name="token"></param>
        /// <param name="clientManager"></param>
        /// <param name="updateHandler"></param>
        public TelegramBotKeeper(IClientManager clientManager)
        {
            GetSettings();
                        
            string token = _settings["token"];

            _bot = new TelegramBotClient(token);

            _clientManager = clientManager;

            _updateHandler = null;
        }
                
        private void GetSettings()
        {
            string[] str_lines = System.IO.File.ReadAllLines("config.ini");
            foreach (string line in str_lines)
            {
                string[] lineContent = line.Split('=');
                _settings.Add(lineContent[0], lineContent[1]);
            }                        
        }

        /// <summary>
        /// Запуск приема входящих обновлений
        /// </summary>
        public void StartReceiveUpdates()
        {
            using var cts = new CancellationTokenSource();
            _bot.StartReceiving(HandleUpdate, HandleError, receiveOptions, cancellationToken: cts.Token);
            //Console.ReadLine();
        }

        /// <summary>
        /// Прием входящих обновлений
        /// </summary>
        /// <param name="botClient">Телеграм-бот</param>
        /// <param name="update">Входящее обновление</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message) return;

            if (!_clientManager.IsClientExist(update.Message.Chat.Id)) _clientManager.AddClient(update);

            UpdateHandler.ServeUpdateFromClient(update);

            //Message sentMessage = await botClient.SendTextMessageAsync(
            //        chatId: update.Message.Chat.Id,
            //        text: "You said:\n" + update.Message.Text,
            //        cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Обработка возникающих ошибок/исключений
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            return Task.CompletedTask;
        }

        #endregion
    }
}
