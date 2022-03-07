using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramLibrary
{
    public interface ITelegramBot
    {
        /// <summary>
        /// Start to recieve updates
        /// </summary>
        public void StartReceiveUpdates();

        /// <summary>
        /// Condition to get access to list of clients
        /// </summary>
        /// <returns></returns>
        public IClientManager ClientManager{ get; set; }

        public ITelegramUpdateHandler UpdateHandler { get; set; }

        public TelegramBotClient Bot { get; }
    }
}
