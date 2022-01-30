using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_10_SKP
{
    public interface IServiceSubscriber : IAppClient
    {
        /// <summary>
        /// States that show which of services are used by subscriber
        /// </summary>
        ClientState State { get; set; }
    }
}
