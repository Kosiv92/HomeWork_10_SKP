using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramLibrary
{
    internal class TelegramClientManager : IClientManager
    {
        private Dictionary<long, IAppClient> _clients = new Dictionary<long, IAppClient>();

        public Dictionary<long, IAppClient> Clients { get => _clients; set => _clients = value; }
    }
}
