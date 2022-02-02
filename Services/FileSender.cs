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
    public class FileSender : IFileSender
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
        public async void SendFile(string fileName, ChatId chatId)
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
