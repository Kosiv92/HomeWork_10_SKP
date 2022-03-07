using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramLibrary
{
    /// <summary>
    /// Модель клиента Telegram-бота
    /// </summary>
    public class TelegramClient : INotifyPropertyChanged, IEquatable<TelegramClient>, IAppClient
    {

        /// <summary>
        /// Конструктор объекта
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="id"></param>
        public TelegramClient(string nickName, long id)
        {
            this.name = nickName;
            this.id = id;
            Messages = new ObservableCollection<string>();
            State = new ClientState();

        }

        #region Поля

        private string name; //логин клиента

        private long id; //уникальный идентификатор клиента               

        #endregion

        #region Свойства

        /// <summary>
        /// Свойство доступа к полю хранющему логин клиента
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Name)));
            }
        }

        /// <summary>
        /// Статус обработки обновлений от клиентов
        /// </summary>
        public ClientState State{ get; set; }

        /// <summary>
        /// Свойство доступа к полю храняющему уникальный идентификатор клиента
        /// </summary>
        public long Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Id)));
            }
        }

        /// <summary>
        /// Список всех сообщений в чате с клиентом
        /// </summary>
        public ObservableCollection<string> Messages { get; set; }

        #endregion

        #region Методы

        /// <summary>
        /// Метод добавления нового сообщения в список сообщений чата
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message) => Messages.Add(message);

        /// <summary>
        /// Метод сравнения другого клиента с текущим
        /// </summary>
        /// <param name="otherClient">Клиент для сравнения с тем из которого вызывается метод</param>
        /// <returns>Результат проверки</returns>
        public bool Equals(TelegramClient otherClient) => otherClient.Id == this.Id;

        #endregion

        #region События

        /// <summary>
        /// Событие для отправки уведомлений об изменении свойств объекта
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
