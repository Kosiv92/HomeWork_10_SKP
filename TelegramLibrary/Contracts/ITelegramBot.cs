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
        /// Запуск приема обновления
        /// </summary>
        public void StartReceiveUpdates();
                
        public IClientManager ClientManager{ get; set; }

        public ITelegramUpdateHandler UpdateHandler { get; set; }

        public TelegramBotClient Bot { get; }
    }
}
