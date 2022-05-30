using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramLibrary
{
    [JsonObject(IsReference = true)]
    public interface IAppClient
    {
        /// <summary>
        /// Имя клиент
        /// </summary>
        /// <returns></returns>
        string Name { get;}

        /// <summary>
        /// Уникальный идентификатор клиента
        /// </summary>
        /// <returns></returns>
        long Id { get; }

        [JsonIgnore]
        /// <summary>
        /// Статус клиента
        /// </summary>
        ClientState State { get; set; }                
 
    }
}
