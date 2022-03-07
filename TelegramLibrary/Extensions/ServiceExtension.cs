using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramLibrary
{
    public static class ServiceExtension
    {
        public static void AddTelegramBot(this IServiceCollection services)
        {
            services.AddSingleton<TelegramBotKeeper>();
            services.AddSingleton<TelegramClientManager>();
            services.AddScoped<MessageHandler>();
            services.AddScoped<InputOutputFileForwarder>();
            services.AddSingleton<ITelegramBot>(c => c.GetRequiredService<TelegramBotKeeper>());
            services.AddSingleton<IClientManager>(c => c.GetRequiredService<TelegramClientManager>());
            services.AddScoped<ITelegramUpdateHandler>(c => c.GetRequiredService<MessageHandler>());
            services.AddScoped<IAppClient, TelegramClient>();
        }
    }
}
