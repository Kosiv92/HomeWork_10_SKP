using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramLibrary;

namespace HomeWork_10_SKP
{
    internal class MainViewModel
    {
        private IAppClient selectedClient;

        private IClientManager clientManager;

        public ObservableCollection<IAppClient> Clients { get; set; }

        public MainViewModel(IClientManager clientManager)
        {
            this.clientManager = clientManager;

            Clients = new ObservableCollection<IAppClient>(clientManager.Clients.Values);

            clientManager.ClientAdded += AddClientToObsCollection;
        }

        public IAppClient SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }
        public void AddClientToObsCollection(IAppClient newClient)
        {
            Clients.Add(newClient);
        }

        public void AddMessageToClientMessageList(Update update)
        {            
            for(int i = 0; i < Clients.Count; i++)
            {
                if(Clients[i].Id == update.Message.Chat.Id) Clients[i].Messages.Add(update.Message.Text);                
            }            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
