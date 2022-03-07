﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramLibrary
{
    public interface IAppClient
    {
        /// <summary>
        /// Name of current client
        /// </summary>
        /// <returns></returns>
        string Name { get;}

        /// <summary>
        /// Unique identificator of current client
        /// </summary>
        /// <returns></returns>
        long Id { get; }

        /// <summary>
        /// States that show which of services are used by subscriber
        /// </summary>
        ClientState State { get; set; }

    }
}