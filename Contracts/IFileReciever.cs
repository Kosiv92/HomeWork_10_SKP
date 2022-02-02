using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HomeWork_10_SKP
{
    public interface IFileReciever
    {
        Task SaveFile(Update update);
    }
}
