using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HomeWork_10_SKP
{
    public  class FileReceiver : IFileReciever
    {
        readonly InputOutputFileForwarder _inputOutputFileForwarder;

        Dictionary<MessageType, Func<Update, Task>> methods;

        public FileReceiver(InputOutputFileForwarder inputOutputFileForwarder)
        {
            _inputOutputFileForwarder = inputOutputFileForwarder;

            CreateMethods();
        }

        private void CreateMethods()
        {
            methods = new Dictionary<MessageType, Func<Update, Task>>
            {
                { MessageType.Document, SaveDocumentToRepository },
                { MessageType.Video, SaveVideoToRepository },
                { MessageType.Audio, SaveVideoToRepository },
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

            path = _inputOutputFileForwarder.Repository.PathToRepository + update.Message.Document.FileName;
            file = await _inputOutputFileForwarder.BotKeeper.Bot.GetFileAsync(update.Message.Document.FileId);

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

            path = _inputOutputFileForwarder.Repository.PathToRepository + update.Message.Audio.FileName;
            file = await _inputOutputFileForwarder.BotKeeper.Bot.GetFileAsync(update.Message.Audio.FileId);

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

            path = _inputOutputFileForwarder.Repository.PathToRepository + update.Message.Video.FileName;
            file = await _inputOutputFileForwarder.BotKeeper.Bot.GetFileAsync(update.Message.Video.FileId);

            await DownloadFile(path, file);
        }

        async Task DownloadFile(string path, Telegram.Bot.Types.File file)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            await _inputOutputFileForwarder.BotKeeper.Bot.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            fs.Dispose();
        }
    }
}
