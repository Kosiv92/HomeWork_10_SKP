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

namespace TelegramLibrary
{

    public class InputOutputFileForwarder : ITelegramUpdateHandler
    {
        #region Fields
        
        ITelegramBot _botKeeper;

        readonly Repository _repository;
                
        Dictionary<MessageType, Func<Update, Task>> methods;

        #endregion

        #region Conditions

        public Repository Repository { get { return _repository; } }

        #endregion


        public InputOutputFileForwarder(ITelegramBot telegramBotKeeper)
        {
            _repository = new Repository();

            _botKeeper = telegramBotKeeper;                       

            CreateMethods();
        }
        public void ServeUpdateFromClient(Update update)
        {            
            Func<Update, Task> serveUpdate = methods[update.Message.Type];
            serveUpdate(update);
        }

        public void SendMessageToClient(long id, string text)
        {
            _botKeeper.Bot.SendTextMessageAsync(id, text);
            _botKeeper.ClientManager.Clients[id].Messages.Add(text);
        }



        //private void CreateAvailableTypes()
        //{
        //    var types = new Dictionary<MessageType, Func<Update, Task>>
        //    {
        //        { MessageType.Document, SaveDocumentToRepository },
        //        { MessageType.Video, SaveVideoToRepository },
        //        { MessageType.Audio, SaveAudioToRepository },                
        //    };
        //}

        public async void SendFile(string fileName, ChatId chatId)
        {
            string fullFileName = _repository.PathToRepository + "\\" + fileName;

            try
            {
                using (FileStream stream = System.IO.File.OpenRead(fullFileName))
                {
                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, fullFileName);
                    await _botKeeper.Bot.SendDocumentAsync(chatId, inputOnlineFile);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                await _botKeeper.Bot.SendTextMessageAsync(chatId: chatId, text: $"File \"{fileName}\" does not exists");
            }
        }

        private void CreateMethods()
        {
            methods = new Dictionary<MessageType, Func<Update, Task>>
            {
                { MessageType.Document, SaveDocumentToRepository },
                { MessageType.Video, SaveVideoToRepository },
                { MessageType.Audio, SaveAudioToRepository },
            };
        }

        /// <summary>
        /// Метод загрузки полученного от телеграм-бота файла
        /// </summary>        
        /// <param name="update">Входящее обновление</param>
        public async Task SaveFile(Update update)
        {
            Func<Update, Task> save = methods[update.Message.Type];

            await save(update);
        }

        /// <summary>
        /// Сохранить документ в репозитории
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task SaveDocumentToRepository(Update update)
        {
            Telegram.Bot.Types.File file;
            string path = "";

            path = _repository.PathToRepository + update.Message.Document.FileName;
            file = await _botKeeper.Bot.GetFileAsync(update.Message.Document.FileId);

            await DownloadFile(path, file);
        }

        /// <summary>
        /// Сохранить аудиофайл в репозитории
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task SaveAudioToRepository(Update update)
        {
            Telegram.Bot.Types.File file;
            string path = "";

            path = _repository.PathToRepository + update.Message.Audio.FileName;
            file = await _botKeeper.Bot.GetFileAsync(update.Message.Audio.FileId);

            await DownloadFile(path, file);
        }

        /// <summary>
        /// Сохранить видеофайл в репозитории
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task SaveVideoToRepository(Update update)
        {
            Telegram.Bot.Types.File file;
            string path = "";

            path = _repository.PathToRepository + update.Message.Video.FileName;
            file = await _botKeeper.Bot.GetFileAsync(update.Message.Video.FileId);

            await DownloadFile(path, file);
        }

        async Task DownloadFile(string path, Telegram.Bot.Types.File file)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            await _botKeeper.Bot.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            fs.Dispose();
        }
                
    }

}
