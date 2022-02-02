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
        /// Телеграм-бот с которым осуществляется взаимодействие
        /// </summary>
        readonly TelegramBotClient _bot;                

        /// <summary>
        /// Список клиентов (осуществляющих взаимодействие через чат) телеграм-бота
        /// </summary>
        private Dictionary<long, TelegramClient> _telegramClients;

        /// <summary>
        /// Объект обрабатывающий обновления поступающие от клиентов бота
        /// </summary>
        TelegramBotUpdateReceiver _updateReceiver;

        #endregion

        #region Свойства

        /// <summary>
        /// Свойство доступа к телеграм-боту с которым осуществляется взаимодействие
        /// </summary>
        public TelegramBotClient Bot { get => _bot; }

        /// <summary>
        /// Свойство доступа к обработчику обновлений
        /// </summary>
        public TelegramBotUpdateReceiver UpdateReceiver { get => _updateReceiver; }

        /// <summary>
        /// Свойство доступа к списку клиентов
        /// </summary>
        public Dictionary<long, TelegramClient> TelegramClients
        {
            get { return _telegramClients; }
            set { _telegramClients = value; }
        }

        #endregion
                
        public TelegramBotKeeper(string token)
        {
            _bot = new TelegramBotClient(token);

            _updateReceiver = new TelegramBotUpdateReceiver(this);                       

            _telegramClients = new Dictionary<long, TelegramClient>();
        } 

    }
}
