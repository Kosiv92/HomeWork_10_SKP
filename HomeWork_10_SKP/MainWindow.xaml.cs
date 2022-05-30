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
using System.Windows.Threading;
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
        MainViewModel mainViewModel;

        public MainWindow()
        {
            mainViewModel = new MainViewModel(this.Dispatcher);                      

            InitializeComponent();
                        
            DataContext = mainViewModel;
                        
            textBox_msgToSend.KeyDown += (s, e) => { if (e.Key == Key.Return) { mainViewModel.SendMessageToClient(textBox_msgToSend.Text); } };

            //void SendMsg()
            //{
            //    var selectedClient = ClientList.SelectedItem as IAppClient;

            //    if (selectedClient != null)
            //    {
            //        mainViewModel.TelegramBotKeeper.Bot.SendTextMessageAsync(selectedClient.Id, textBox_msgToSend.Text);

            //        //selectedClient.Messages.Add(textBox_msgToSend.Text);
            //        mainViewModel.TelegramBotKeeper.ClientManager.Clients[selectedClient.Id].Messages.Add(textBox_msgToSend.Text);
            //    }

            //    textBox_msgToSend.Text = String.Empty;
            //}

            //this.Closed += SaveToDB;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            JSONSerializer.JSONSerializeClients(mainViewModel.Clients);
        }
    }
}
