using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace HomeWork_10_SKP
{

    public class InputOutputFileForwarder : ITelegramUpdateHandler
    {
                        
        readonly Repository _repository;

        Dictionary<MessageType, Func<Update, Task>> types;

        public InputOutputFileForwarder()
        {
            _repository = new Repository();

            CreateAvailableTypes();
        }
        public void ServeUpdate(Update update)
        {            
            Func<Update, Task> serveUpdate = types[update.Message.Type];
            serveUpdate(update);
        }

        public IFileReciever Receiver
        {
            get
            {
                if (Receiver == null) throw new Exception("Receiver is not defined");
                else return Receiver;
            }
            set { Receiver = value; }
        }

        public IFileSender Sender {
            get
            {
                if (Sender == null) throw new Exception("Sender is not defined");
                else return Sender;
            }
            set { Sender = value; }
        }

        public Repository Repository { get { return _repository; } }               

        private void CreateAvailableTypes()
        {
            var types = new Dictionary<MessageType, Func<Update, Task>>
            {
                { MessageType.Document, Receiver.SaveFile },
                { MessageType.Video, Receiver.SaveFile },
                { MessageType.Audio, Receiver.SaveFile },                
            };
        }
               

    }

}
