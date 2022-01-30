using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace HomeWork_10_SKP
{

    public class InputOutputFileForwarder
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

        public FileReceiver Receiver { get { return _receiver; } }

        public FileSender Sender { get { return _sender;} }

        public Repository Repository { get { return _repository; } }

        public TelegramBotKeeper BotKeeper { get { return _botKeeper; } }

    }

    public class FileReceiver
    {
        readonly InputOutputFileForwarder _inputOutputFileForwarder;

        public FileReceiver(InputOutputFileForwarder inputOutputFileForwarder)
        {
            _inputOutputFileForwarder = inputOutputFileForwarder;
        }

        /// <summary>
        /// Метод загрузки полученного от телеграм-бота файла
        /// </summary>        
        /// <param name="update">Входящее обновление</param>
        public async void SaveFileToRepository(Update update)
        {
            Telegram.Bot.Types.File file;

            string path = "";

            switch (update.Message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Document:
                    path = _inputOutputFileForwarder.Repository.PathToRepository + update.Message.Document.FileName;
                    file = await _inputOutputFileForwarder.BotKeeper.Bot.GetFileAsync(update.Message.Document.FileId);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Audio:
                    path = _inputOutputFileForwarder.Repository.PathToRepository + update.Message.Audio.FileName;
                    file = await _inputOutputFileForwarder.BotKeeper.Bot.GetFileAsync(update.Message.Audio.FileId);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Video:
                    path = _inputOutputFileForwarder.Repository.PathToRepository + update.Message.Video.FileName;
                    file = await _inputOutputFileForwarder.BotKeeper.Bot.GetFileAsync(update.Message.Video.FileId);
                    break;
                default:
                    return;
            }

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
