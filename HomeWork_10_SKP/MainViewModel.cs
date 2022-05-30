using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        private ITelegramBot _telegramBotKeeper;

        private ClientViewModel _selectedClient;

        private ICommand _sendMessage;

        public static Dispatcher Dispatcher { get; private set; }

        private string _textBySupport;

        #endregion

        #region Properties

        public ObservableCollection<ClientViewModel> Clients { get; set; }

        public ITelegramBot TelegramBotKeeper
        {
            get { return _telegramBotKeeper; }
            set { _telegramBotKeeper = value; }
        }

        public ClientViewModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public string TextBySupport
        {
            get => _textBySupport;
            set
            {
                _textBySupport = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainViewModel(Dispatcher dispatcher)
        {            
            services = new ServiceCollection();

            ServiceExtension.AddTelegramBot(services);

            ServiceProvider container = services.BuildServiceProvider(validateScopes: false);

            IServiceScope scope = container.CreateScope();

            _telegramBotKeeper = scope.ServiceProvider.GetService<ITelegramBot>();

            _telegramBotKeeper.UpdateHandler = scope.ServiceProvider.GetService<ITelegramUpdateHandler>();

            _telegramBotKeeper.ClientManager.ClientAdded += AddClientToObsCollection;

            _telegramBotKeeper.ClientManager.MessageAdded += AddMessageToClientList;

            SendMessage = new RelayCommand(SendMessageToClient);

            _telegramBotKeeper.StartReceiveUpdates();

            Dispatcher = dispatcher;

            if (System.IO.File.Exists("clients.json"))
            {
                Clients = JSONSerializer.JSONDeserializeClients();
                MessageBox.Show("Обнаружен файл с данными! Данные успешно загружены!");
            }
            else
            {
                Clients = new ObservableCollection<ClientViewModel>();
                MessageBox.Show("Файл с данными не обнаружен!");
            }

            BindingOperations.EnableCollectionSynchronization(Clients, @lock);
        }

        /// <summary>
        /// Добавление новой ViewModel клиента в коллекцию
        /// </summary>
        /// <param name="newClient">Новый клиент телеграм-бота для добавления в коллекцию ViewModel</param>
        public void AddClientToObsCollection(IAppClient newClient)
        {
            bool exist = false;

            foreach (var clientViewModel in Clients)
            {
                if (clientViewModel.Id == newClient.Id) exist = true;
            }

            if (!exist)
            {
                var client = new ClientViewModel() { Id = newClient.Id, Name = newClient.Name, Messages = new ObservableCollection<HomeWork_10_SKP.Message>() };

                Clients.Add(client);
            }

        }

        public void AddMessageToClientList(Update update)
        {
            var client = FindClientById(update.Message.Chat.Id);

            client.AddMessage(update.Message.Chat.Username, update.Message.Text);                        
        }

        public void SendMessageToClient(object parameter)
        {            
            var id = _selectedClient.Id;
            var text = (string)parameter;
            _telegramBotKeeper.UpdateHandler.SendMessageToClient(id, text);
            _selectedClient.AddMessage("Bot", text);
            TextBySupport = "";
        }

        private ClientViewModel FindClientById(long id)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].Id == id) return Clients[i];
            }
            return null;
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
