using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramLibrary;


namespace HomeWork_10_SKP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<TelegramClient> Clients;

        public MainWindow()
        {                        
            var services = new ServiceCollection();
            
            ServiceExtension.AddTelegramBot(services);

            ServiceProvider container = services.BuildServiceProvider(validateScopes: false);

            IServiceScope scope = container.CreateScope();

            ITelegramBot telegramBotKeeper = scope.ServiceProvider.GetService<ITelegramBot>();

            telegramBotKeeper.UpdateHandler = scope.ServiceProvider.GetService<ITelegramUpdateHandler>();
                        
            telegramBotKeeper.StartReceiveUpdates();

            MessageBox.Show("Applicatin has been runing!");

            InitializeComponent();

            ClientList.ItemsSource = telegramBotKeeper.ClientManager.Clients;
        }
    }
}
