﻿using System;
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
    public class InputOutputHelper
    {
        
        readonly string pathToRepository;

        public InputOutputHelper(string path)
            {
                pathToRepository = path;
            }

        /// <summary>
        /// Метод отправки выбранного пользователем файла в его чат
        /// </summary>
        /// <param name="botClient">Чат-бот от которого поступает запрос</param>
        /// <param name="fileName">Имя файла</param>
        /// <param name="chatId">ID чата</param>
        public async void Upload(TelegramBotClient botClient, string fileName, ChatId chatId)
        {
            string fullFileName = pathToRepository + "\\" + fileName;                        

            try
            {                
                using (FileStream stream = System.IO.File.OpenRead(fullFileName))
                {
                    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, fullFileName);
                    await botClient.SendDocumentAsync(chatId, inputOnlineFile);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                await botClient.SendTextMessageAsync(chatId: chatId, text: $"File \"{fileName}\" does not exists");
            }
        }

        /// <summary>
        /// Метод сохранения передаваемого пользователем документа в репозиторий
        /// </summary>
        /// <param name="botClient">Чат-бот от которого поступает запрос</param>
        /// <param name="update">Передаваемый пользователем update содержащий файл</param>
        public async void Download(TelegramBotClient botClient, Update update)
        {
            Telegram.Bot.Types.File file;

            string path = "";

            switch (update.Message.Type)
            {
                case Telegram.Bot.Types.Enums.MessageType.Document:
                    path = pathToRepository + update.Message.Document.FileName;
                    file = await botClient.GetFileAsync(update.Message.Document.FileId);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Audio:
                    path = pathToRepository + update.Message.Audio.FileName;
                    file = await botClient.GetFileAsync(update.Message.Audio.FileId);
                    break;
                case Telegram.Bot.Types.Enums.MessageType.Video:
                    path = pathToRepository + update.Message.Video.FileName;
                    file = await botClient.GetFileAsync(update.Message.Video.FileId);
                    break;
                default:
                    return;                  
            }

            FileStream fs = new FileStream(path, FileMode.Create);
            await botClient.DownloadFileAsync(file.FilePath, fs);
            fs.Close();

            fs.Dispose();
        }        



    }
}
