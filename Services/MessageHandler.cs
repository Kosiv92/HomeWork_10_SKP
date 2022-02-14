using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HomeWork_10_SKP
{
    public class MessageHandler : ITelegramUpdateHandler
    {
        #region Поля и Свойства

        readonly InputOutputFileForwarder _iOHelper;

        internal Dictionary<long, ClientState> _userState;
                
        readonly TelegramBotUpdateReceiver _updateReceiver;

        public InputOutputFileForwarder IOHelper { get { return _iOHelper; } }

        public TelegramBotUpdateReceiver UpdateReceiver { get { return _updateReceiver;} }

        /// <summary>
        /// Номер выбранного файла из списка файлов
        /// </summary>
        int numberOfFile = 0;

        List<List<KeyboardButton>> _defaultKeyboard = new List<List<KeyboardButton>>
        {
            new List<KeyboardButton>{ new KeyboardButton(UploadText) },
            new List<KeyboardButton>{ new KeyboardButton(ListText) },
            new List<KeyboardButton>{ new KeyboardButton(WeatherText) }
        };

        #endregion

        #region Константы

        const string ListText = "ShowList";
        const string WeatherText = "Weather";
        const string UploadText = "Upload";
        const string PrevFileText = "<";
        const string NextFileText = ">";
        const string CancelText = "Cancel";

        #endregion

        #region Методы

        /// <summary>
        /// Конструкторы объекта класса
        /// </summary>
        /// <param name="bot">Класс хранящий конфигурацию телеграм-бота</param>
        public MessageHandler(TelegramBotUpdateReceiver updateReceiver)
        {
            _updateReceiver = updateReceiver;

            _iOHelper = new InputOutputFileForwarder();

            _userState = new Dictionary<long, ClientState>();
        }

        #region Методы получения кнопок

        /// <summary>
        /// Получение кнопок основного меню
        /// </summary>
        /// <returns></returns>
        private IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup(_defaultKeyboard) { ResizeKeyboard = true };
        }

        /// <summary>
        /// Получение кнопок меню для скачивания файлов
        /// </summary>
        /// <param name="files">Список файлов репозитория</param>
        /// <param name="position">Номер файла в списке (выбор для скачивания)</param>
        /// <returns></returns>
        private IReplyMarkup GetUploadButtons(FileInfo[] files, int position)
        {
            string filename = files[position].Name;

            List<List<KeyboardButton>> uploadKeyBoard = new List<List<KeyboardButton>>()
                {
                    new List<KeyboardButton>{ new KeyboardButton(PrevFileText) },
                    new List<KeyboardButton>{ new KeyboardButton(filename) },
                    new List<KeyboardButton>{ new KeyboardButton(NextFileText) },
                    new List<KeyboardButton>{ new KeyboardButton(CancelText) }
                 };

            return new ReplyKeyboardMarkup(uploadKeyBoard) { ResizeKeyboard = true };
        }

        #endregion

        /// <summary>
        /// Метод обработки обновлений от клиентов
        /// </summary>
        /// <param name="update">Входящее обновление от клиента</param>
        public void ServeUpdate(Update update)
        {

            switch (update.Message.Type)
            {
                case MessageType.Photo:
                //_iOHelper.Receiver.SaveFileToRepository(update);
                //break;
                case MessageType.Document:
                //_iOHelper.Receiver.SaveFileToRepository(update);
                //break;
                case MessageType.Audio:
                    _iOHelper.Receiver.SaveFile(update);
                    break;
                case MessageType.Text:
                    ServeTextUpdate(update);
                    break;
            }
        }

        /// <summary>
        /// Метод обработки текстовых сообщений от клиентов
        /// </summary>
        /// <param name="update"></param>
        private void ServeTextUpdate(Update update)
        {
            SaveLogger(update);

            if (_updateReceiver.BotKeeper.TelegramClients[update.Message.Chat.Id].State.isFileSendOn == true) UploadHandler(update);
            else if (_updateReceiver.BotKeeper.TelegramClients[update.Message.Chat.Id].State.isWeatherSearchOn == true) WeatherHandler(update);
            else
            {
                _updateReceiver.BotKeeper.Bot.SendTextMessageAsync(update.Message.Chat.Id, text: "Choose action", replyMarkup: GetButtons());
                switch (update.Message.Text)
                {
                    case ListText:
                        _updateReceiver.BotKeeper.Bot.SendTextMessageAsync(update.Message.Chat.Id, text: IOHelper.Repository.GetFileList());
                        break;
                    case WeatherText:
                        TurnOnWeatherSearch(_updateReceiver.BotKeeper.TelegramClients[update.Message.Chat.Id]);
                        break;
                    case UploadText:
                        TurnOnFileSendingMode(_updateReceiver.BotKeeper.TelegramClients[update.Message.Chat.Id]);
                        break;
                }
            }
        }

        private void SaveLogger(Update update)
        {
            string logMessage = $"{DateTime.Now}: {update.Message.Chat.FirstName} {update.Message.Chat.Id} {update.Message.Text}";

            System.IO.File.AppendAllText("data.log", $"{logMessage}\n");

            Debug.WriteLine(logMessage);
        }

        #region Методы связанные с отправкой погоды клиенту

        /// <summary>
        /// Установка режима отправки данных о погоде клиенту
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        async private void TurnOnWeatherSearch(TelegramClient client)
        {
            client.State.isFileSendOn = false;
            client.State.isWeatherSearchOn = true;

            await _updateReceiver.BotKeeper.Bot.SendTextMessageAsync(chatId: client.Id, text: "Write name of city which weather you need to know!", replyMarkup: new ReplyKeyboardMarkup("Cancel"));
        }

        /// <summary>
        /// Обработчик запросов погоды
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        private void WeatherHandler(Update update)
        {

            if (update.Message.Text == CancelText)
            {
                _updateReceiver.BotKeeper.TelegramClients[update.Message.Chat.Id].State.isWeatherSearchOn = false;
                ServeUpdate(update);
            }
            else
            {
                SendWeatherForecast(update);
            }
        }

        /// <summary>
        /// Метод отправки клиенту данных о температуре
        /// </summary>
        /// <param name="update"></param>
        async private void SendWeatherForecast(Update update)
        {
            string temperature = WeatherInformer.WeatherRequest(update.Message.Text);
            await _updateReceiver.BotKeeper.Bot.SendTextMessageAsync(chatId: update.Message.Chat.Id, text: temperature);
        }

        #endregion


        #region Методы связанные с отправкой файлов клиенту

        /// <summary>
        /// Установка режима отправки файлов клиенту
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        async private void TurnOnFileSendingMode(TelegramClient client)
        {
            client.State.isWeatherSearchOn = false;
            client.State.isFileSendOn = true;

            FileInfo[] files = _iOHelper.Repository.GetFilesName();

            await _updateReceiver.BotKeeper.Bot.SendTextMessageAsync(client.Id, text: "Choose file to upload", replyMarkup: GetUploadButtons(files, numberOfFile));
        }

        /// <summary>
        /// Обработчик запросов на отправку файлов клиенту
        /// </summary>
        /// <param name="update">Обновление от клиента</param>
        async private Task UploadHandler(Update update)
        {
            FileInfo[] files = _iOHelper.Repository.GetFilesName();

            await _updateReceiver.BotKeeper.Bot.SendTextMessageAsync(update.Message.Chat.Id, text: "Choose file to upload", replyMarkup: GetUploadButtons(files, numberOfFile));

            switch (update.Message.Text)
            {
                case CancelText:
                    _updateReceiver.BotKeeper.TelegramClients[update.Message.Chat.Id].State.isFileSendOn = false;
                    numberOfFile = 0;
                    ServeUpdate(update);
                    break;
                case PrevFileText:
                    if (numberOfFile > 0) numberOfFile--;
                    else numberOfFile = files.Length - 1;
                    break;
                case NextFileText:
                    if (numberOfFile < files.Length - 1) numberOfFile++;
                    else numberOfFile = 0;
                    break;
                case UploadText:
                    break;
                default:
                    _iOHelper.Sender.SendFile(update.Message.Text, update.Message.Chat.Id);
                    break;
            }
        }
        #endregion

        #endregion

    }

    //public class UploadHandler : ITelegramUpdateHandler
    //{
    //    const string ListText = "ShowList";        
    //    const string UploadText = "Upload";
    //    const string PrevFileText = "<";
    //    const string NextFileText = ">";
    //    const string CancelText = "Cancel";

    //    readonly InputOutputFileForwarder _iOHelper;

    //    FileInfo[] files;

    //    /// <summary>
    //    /// Номер выбранного файла из списка файлов
    //    /// </summary>
    //    int numberOfFile = 0;

    //    public InputOutputFileForwarder IOHelper { get { return _iOHelper; } }

    //    public UploadHandler()
    //    {
    //        _iOHelper = new InputOutputFileForwarder();
    //    }

    //    public void ServeUpdate(Update update)
    //    {
    //        files = _iOHelper.Repository.GetFilesName();


    //    }

    //    async private Task UploadHandler(Update update)
    //    {
    //        FileInfo[] files = _iOHelper.Repository.GetFilesName();

    //        await _updateReceiver.BotKeeper.Bot.SendTextMessageAsync(update.Message.Chat.Id, text: "Choose file to upload", replyMarkup: GetUploadButtons(files, numberOfFile));

    //        switch (update.Message.Text)
    //        {
    //            case CancelText:
    //                _updateReceiver.BotKeeper.TelegramClients[update.Message.Chat.Id].State.isFileSendOn = false;
    //                numberOfFile = 0;
    //                ServeUpdate(update);
    //                break;
    //            case PrevFileText:
    //                if (numberOfFile > 0) numberOfFile--;
    //                else numberOfFile = files.Length - 1;
    //                break;
    //            case NextFileText:
    //                if (numberOfFile < files.Length - 1) numberOfFile++;
    //                else numberOfFile = 0;
    //                break;
    //            case UploadText:
    //                break;
    //            default:
    //                _iOHelper.Sender.SendFile(update.Message.Text, update.Message.Chat.Id);
    //                break;
    //        }
    //    }
    //}

}
