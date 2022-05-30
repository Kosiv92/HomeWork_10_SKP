using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_10_SKP
{
    /// <summary>
    /// Класс описывающий сущность объекта "Сообщение"
    /// </summary>
    [JsonObject(IsReference = true)]    
    public class Message
    {
        #region Properties
        public string Author { get;set; }
        public string Text { get; set; }

        #endregion

        #region Methods
        public override string ToString()
        {
            return Author + ": " + Text;
        }

        #endregion
    }
}
