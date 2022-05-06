using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramLibrary;

namespace HomeWork_10_SKP
{
    public class ClientViewModel : BaseViewModel
    {
        #region Fields

        private string _name;

        private long _id;

        private ObservableCollection<string> _messages;

        #endregion

        #region Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public long Id => _id;

        public ObservableCollection<string> Messages
        {
            get => _messages;
            set => _messages = value;
        }


        #endregion

        public static ClientViewModel Create(IAppClient client)
        {
            return new ClientViewModel
            {
                _name = client.Name,
                _id = client.Id,
                _messages = new ObservableCollection<string>(client.Messages)
            };
        }
    }
}

