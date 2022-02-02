using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HomeWork_10_SKP
{
    public interface ITelegramUpdateHandler
    {
        void ServeUpdate(Update update);
    }
}
