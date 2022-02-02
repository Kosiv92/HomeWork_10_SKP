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
            InitializeComponent();

            Clients = new ObservableCollection<TelegramClient>();

            ClientList.ItemsSource = Clients;

            string token = System.IO.File.ReadAllText("token.txt"); //получения уникального идентификатора (токена)

            TelegramBotKeeper telegramBotKeeper = new TelegramBotKeeper(token); //инициализация клиента API серверов Telegram

            ITelegramUpdateHandler messageHandler = new MessageHandler(telegramBotKeeper.UpdateReceiver); //создаем реализацию интерфейса для обработки входящих обновлений

            telegramBotKeeper.UpdateReceiver.UpdateHandler = messageHandler; //передаем реализацию обработчика входящих обновлений 

            telegramBotKeeper.UpdateReceiver.StartReceiveUpdates(); //запуск приема обновлений от клиентов бота                      
            
        }
    }
}
