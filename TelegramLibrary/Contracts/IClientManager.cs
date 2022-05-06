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
    public interface IClientManager
    {
        public Dictionary<long, IAppClient> Clients { get; set; }

        public bool IsClientExist(long id);

        public void AddClient(Update update);

        public void AddMessage(Update update);

        event Action<IAppClient> ClientAdded;

        event Action<Update> MessageAdded;

    }
}
