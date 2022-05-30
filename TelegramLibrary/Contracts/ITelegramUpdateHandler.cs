using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramLibrary
{
    public interface ITelegramUpdateHandler
    {
        void ServeUpdateFromClient(Update update);

        void SendMessageToClient(long id, string text);
    }
}
