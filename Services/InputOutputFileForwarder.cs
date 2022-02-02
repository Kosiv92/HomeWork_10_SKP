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
        TelegramBotKeeper _botKeeper;

        readonly FileReceiver _receiver;

        readonly FileSender _sender;

        readonly Repository _repository;

        public InputOutputFileForwarder(TelegramBotKeeper bot)
        {
            _botKeeper = bot;

            _receiver = new FileReceiver(this);

            _sender = new FileSender(this);

            _repository = new Repository();
        }
        public void ServeUpdate(Update update)
        {
            throw new NotImplementedException();
        }

        public FileReceiver Receiver { get { return _receiver; } }

        public FileSender Sender { get { return _sender; } }

        public Repository Repository { get { return _repository; } }

        public TelegramBotKeeper BotKeeper { get { return _botKeeper; } }

    }

    public class FileReceiver
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
        public async void SaveFileToRepository(Update update)
        {
            Func<Update, Task> save = methods[update.Message.Type];

            save(update);
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

            await DoawnloadFile(path, file);                        
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

            await DoawnloadFile(path, file);
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

            await DoawnloadFile(path, file);
        }

        async Task DoawnloadFile(string path, Telegram.Bot.Types.File file)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            await _inputOutputFileForwarder.BotKeeper.Bot.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            fs.Dispose();
        }


    }

    public class FileSender
    {
        readonly InputOutputFileForwarder _inputOutputFileForwarder;

        public FileSender(InputOutputFileForwarder inputOutputFileForwarder)
        {
            _inputOutputFileForwarder = inputOutputFileForwarder;
        }


        /// <summary>
        /// Метод отправки выбранного пользователем файла в его чат
        /// </summary>        
        /// <param name="fileName">Имя файла</param>
        /// <param name="chatId">ID чата</param>
        public async void SendFileToChat(string fileName, ChatId chatId)
        {
            string fullFileName = _inputOutputFileForwarder.Repository.PathToRepository + "\\" + fileName;

            try
            {
                using (FileStream stream = System.IO.File.OpenRead(fullFileName))
                {
                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, fullFileName);
                    await _inputOutputFileForwarder.BotKeeper.Bot.SendDocumentAsync(chatId, inputOnlineFile);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                await _inputOutputFileForwarder.BotKeeper.Bot.SendTextMessageAsync(chatId: chatId, text: $"File \"{fileName}\" does not exists");
            }
        }
    }
}
