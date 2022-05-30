using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramLibrary
{
    /// <summary>
    /// Модель клиента Telegram-бота
    /// </summary>
    public class TelegramClient : IEquatable<TelegramClient>, IAppClient
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
            }
        }

        /// <summary>
        /// Статус обработки обновлений от клиентов
        /// </summary>
        public ClientState State { get; set; }

        /// <summary>
        /// Свойство доступа к полю храняющему уникальный идентификатор клиента
        /// </summary>
        public long Id
        {
            get { return this.id; }
            set
            {
                this.id = value;                
            }
        }

        #endregion

        #region Методы                

        /// <summary>
        /// Метод сравнения другого клиента с текущим
        /// </summary>
        /// <param name="otherClient">Клиент для сравнения с тем из которого вызывается метод</param>
        /// <returns>Результат проверки</returns>
        public bool Equals(TelegramClient otherClient) => otherClient.Id == this.Id;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        #endregion

    }
}
