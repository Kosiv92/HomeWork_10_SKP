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
    internal class TelegramClientManager : IClientManager
    {
        private Dictionary<long, IAppClient> _clients = new Dictionary<long, IAppClient>();

        public event Action<IAppClient> ClientAdded;

        //public event Action<Update> MessageAdded;

        public Dictionary<long, IAppClient> Clients { get => _clients; set => _clients = value; }
                
        public bool IsClientExist(long id) { return _clients.ContainsKey(id); }

        public void AddClient(Update update)
        {
            _clients.Add(update.Message.Chat.Id, new TelegramClient(update.Message.Chat.Username, update.Message.Chat.Id));
            ClientAdded?.Invoke(_clients[update.Message.Chat.Id]);
        }
                
        //public void AddMessage(Update update)
        //{
        //    _clients[update.Message.Chat.Id].Messages.Add(update.Message.Text);            
        //    MessageAdded?.Invoke(update);
        //}

    }
}
