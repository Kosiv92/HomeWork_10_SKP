using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramLibrary
{
    /// <summary>
    /// Сущность хранящая список клиентов телеграм-бота
    /// </summary>
    internal class TelegramClientManager : IClientManager
    {
        /// <summary>
        /// Словарь сопоставления телеграм-бота и его id
        /// </summary>
        private Dictionary<long, IAppClient> _clients = new Dictionary<long, IAppClient>();

        /// <summary>
        /// Событие добавления нового клиента
        /// </summary>
        public event Action<IAppClient> ClientAdded;

        /// <summary>
        /// Событие добавления нового сообщения
        /// </summary>
        public event Action<Update> MessageAdded;

        public Dictionary<long, IAppClient> Clients { get => _clients; set => _clients = value; }
                
        /// <summary>
        /// Проверка существует ли клиент с таким id в списке клиентов
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsClientExist(long id) { return _clients.ContainsKey(id); }

        /// <summary>
        /// Метод добавления клиента в списко клиентов
        /// </summary>
        /// <param name="update"></param>
        public void AddClient(Update update)
        {
            _clients.Add(update.Message.Chat.Id, new TelegramClient(update.Message.Chat.Username, update.Message.Chat.Id));
            ClientAdded?.Invoke(_clients[update.Message.Chat.Id]);
        }

        /// <summary>
        /// Метод добавления сообщения
        /// </summary>
        /// <param name="update"></param>
        public void AddMessage(Update update) { MessageAdded?.Invoke(update); }

    }
}
