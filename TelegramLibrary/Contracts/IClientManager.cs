using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramLibrary
{
    public interface IClientManager
    {
        public Dictionary<long, IAppClient> Clients { get; set; }                
    }
}
