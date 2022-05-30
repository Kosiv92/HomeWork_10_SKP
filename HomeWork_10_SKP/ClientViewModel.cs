using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Telegram.Bot.Types;
using TelegramLibrary;

namespace HomeWork_10_SKP
{
    /// <summary>
    /// Модель-представление сущности "Клиент"
    /// </summary>
    [JsonObject(IsReference = true)]
    public class ClientViewModel : BaseViewModel
    {
        #region Fields

        private string _name;

        private long _id;

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

        public long Id
        {
            get => _id;
            set => _id = value;
        }

        public ObservableCollection<HomeWork_10_SKP.Message> Messages
        {
            get;
            set;
        } = new ObservableCollection<HomeWork_10_SKP.Message>();



        #endregion

        public void AddMessage(string author, string text)
        {
            var message = new HomeWork_10_SKP.Message() { Author = author, Text = text };
            MainViewModel.Dispatcher.Invoke(() => Messages.Add(message));
        }

    }
}

