using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HomeWork_10_SKP
{
    public class TelegramBotUpdateReceiver
    {
        #region Поля

        /// <summary>
        /// Объект хранящий телеграм-бота
        /// </summary>
        readonly TelegramBotKeeper _telegramBotKeeper;

        /// <summary>
        /// Настройки разрешенных к обработке обновлений
        /// </summary>
        ReceiverOptions receiveOptions = new ReceiverOptions
        {
            AllowedUpdates = { } // receive all update types
        };

        #endregion

        #region Свойства

        /// <summary>
        /// Свойство доступа к хранителю телеграм бота
        /// </summary>
        public TelegramBotKeeper BotKeeper { get => _telegramBotKeeper; }

        /// <summary>
        /// Свойство доступа к обработчику входящих обновлений
        /// </summary>
        public ITelegramUpdateHandler UpdateHandler
        {
            get
            {
                if (UpdateHandler == null) throw new Exception("UpdateHandler is not defined");
                else return UpdateHandler;
            }
            set { UpdateHandler = value; }
        }

        #endregion

        #region Методы

        /// <summary>
        /// Конструктор объектов класса
        /// </summary>
        /// <param name="botKeeper"></param>
        public TelegramBotUpdateReceiver(TelegramBotKeeper botKeeper) => _telegramBotKeeper = botKeeper;

        /// <summary>
        /// Запуск приема входящих обновлений
        /// </summary>
        public void StartReceiveUpdates()
        {
            using var cts = new CancellationTokenSource();
            _telegramBotKeeper.Bot.StartReceiving(HandleUpdate, HandleError, receiveOptions, cancellationToken: cts.Token);
            Console.ReadLine();
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

            //if (!_telegramBotKeeper.TelegramClients.ContainsKey(update.Message.Chat.Id)) _telegramBotKeeper.TelegramClients[update.Message.Chat.Id] = new TelegramClient(update.Message.Chat.Username, update.Message.Chat.Id);

            UpdateHandler.ServeUpdate(update);

            Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "You said:\n" + update.Message.Text,
                    cancellationToken: cancellationToken);
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
