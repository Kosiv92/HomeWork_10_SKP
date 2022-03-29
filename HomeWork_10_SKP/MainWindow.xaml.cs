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
        //ObservableCollection<TelegramClient> Clients;

        public MainWindow()
        {
            var services = new ServiceCollection();

            ServiceExtension.AddTelegramBot(services);

            ServiceProvider container = services.BuildServiceProvider(validateScopes: false);

            IServiceScope scope = container.CreateScope();

            ITelegramBot telegramBotKeeper = scope.ServiceProvider.GetService<ITelegramBot>();

            telegramBotKeeper.UpdateHandler = scope.ServiceProvider.GetService<ITelegramUpdateHandler>();

            telegramBotKeeper.StartReceiveUpdates();

            MessageBox.Show("Application has been runing!");

            InitializeComponent();

            MainViewModel mainViewModel = new MainViewModel(telegramBotKeeper.ClientManager);

            DataContext = mainViewModel;

            //Подписка метода по добавлению нового клиента из ViewModel к событию из Model
            telegramBotKeeper.ClientManager.ClientAdded += mainViewModel.AddClientToObsCollection;

            //telegramBotKeeper.ClientManager.MessageAdded += mainViewModel.AddMessageToClientMessageList;

            SendMessageButton.Click += delegate { SendMsg(); };
            textBox_msgToSend.KeyDown += (s, e) => { if (e.Key == Key.Return) { SendMsg(); } };

            void SendMsg()
            {
                var selectedClient = ClientList.SelectedItem as IAppClient;
                                
                telegramBotKeeper.Bot.SendTextMessageAsync(selectedClient.Id, textBox_msgToSend.Text);
                                
                //selectedClient.Messages.Add(textBox_msgToSend.Text);
                telegramBotKeeper.ClientManager.Clients[selectedClient.Id].Messages.Add(textBox_msgToSend.Text);

                textBox_msgToSend.Text = String.Empty;
            }

            //this.Closed += SaveToDB;
        }
    }
}
