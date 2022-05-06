using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramLibrary;

namespace HomeWork_10_SKP
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields

        private object @lock = new object();

        private ServiceCollection services;

        private ITelegramBot telegramBotKeeper;

        private IAppClient selectedClient;

        private ICommand _sendMessage;

        private MainWindow _window;

        private string _textBySupport;

        #endregion

        #region Properties

        public ObservableCollection<ClientViewModel> Clients { get; set; }

        public ITelegramBot TelegramBotKeeper
        {
            get { return telegramBotKeeper; }
            set { telegramBotKeeper = value; }
        }

        public IAppClient SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChanged();
            }
        }

        public string TextBySupport 
            {
                get => _textBySupport;
            set { _textBySupport = value;
                OnPropertyChanged();
            }
            }

        #endregion

        public MainViewModel(MainWindow window)
        {
            _window = window;
            
            services = new ServiceCollection();

            ServiceExtension.AddTelegramBot(services);

            ServiceProvider container = services.BuildServiceProvider(validateScopes: false);

            IServiceScope scope = container.CreateScope();

            telegramBotKeeper = scope.ServiceProvider.GetService<ITelegramBot>();

            telegramBotKeeper.UpdateHandler = scope.ServiceProvider.GetService<ITelegramUpdateHandler>();

            Clients = new ObservableCollection<ClientViewModel>();

            BindingOperations.EnableCollectionSynchronization(Clients, @lock);

            telegramBotKeeper.ClientManager.ClientAdded += AddClientToObsCollection;

            telegramBotKeeper.ClientManager.MessageAdded += AddMessageToClientMessageList;

            SendMessage = new RelayCommand(SendMessageToClient);
        }

        /// <summary>
        /// Добавление новой ViewModel клиента в коллекцию
        /// </summary>
        /// <param name="newClient">Новый клиент телеграм-бота для добавления в коллекцию ViewModel</param>
        public void AddClientToObsCollection(IAppClient newClient)
        {
            var client = ClientViewModel.Create(newClient);
            Clients.Add(client);
        }

        public void AddMessageToClientMessageList(Update update)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].Id == update.Message.Chat.Id) Clients[i].Messages.Add(update.Message.Text);

            }
        }

        public void SendMessageToClient(object parameter)
        {
            //Dispatcher.CurrentDispatcher.Invoke(() => { });
            ClientViewModel selectedClient = (ClientViewModel)_window.ClientList.SelectedItem;
            var id = selectedClient.Id;
            telegramBotKeeper.UpdateHandler.SendMessageToClient(id, "Bot: " + (string)parameter);            
            TextBySupport = "";
        }

        public ICommand SendMessage
        {
            get => _sendMessage;
            set
            {
                _sendMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
