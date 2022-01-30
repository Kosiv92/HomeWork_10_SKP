using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HomeWork_10_SKP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<TelegramClient> Clients;

        public MainWindow()
        {
            InitializeComponent();

            Clients = new ObservableCollection<TelegramClient>();

            ClientList.ItemsSource = Clients;

            string token = System.IO.File.ReadAllText("token.txt"); //получения уникального идентификатора (токена)

            TelegramBotKeeper telegramBotKeeper = new TelegramBotKeeper(token, this); //инициализация клиента API серверов Telegram

            telegramBotKeeper.StartReceiveUpdates();

            //TelegramBotClient _bot = new TelegramBotClient(token);

            //StartReceiveUpdates();

            //void StartReceiveUpdates()
            //{
            //    using var cts = new CancellationTokenSource();
            //    telegramBotKeeper.Bot.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), cts.Token);
            //    //_bot.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), cts.Token);
            //    Console.ReadLine();
            //    cts.Cancel();
            //}

            //static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            //{
            //    var ErrorMessage = exception switch
            //    {
            //        ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            //        _ => exception.ToString()
            //    };

            //    return Task.CompletedTask;
            //}

            ///// <summary>
            ///// Метод обработки обновлений от пользователя
            ///// </summary>
            ///// <param name="botClient"></param>
            ///// <param name="update"></param>
            ///// <param name="cancellationToken"></param>
            ///// <returns></returns>
            //async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            //{
            //    if (update.Type != UpdateType.Message) return;

            //    telegramBotKeeper.MessageHandler.ServeUpdate(update);

            //    this.Dispatcher.Invoke(() =>
            //    {
            //        var client = new TelegramClient(update.Message.Chat.FirstName, update.Message.Chat.Id);
            //        if (!Clients.Contains(client)) Clients.Add(client);
            //        Clients[Clients.IndexOf(client)].AddMessage($"{client.NickName}: {update.Message.Text}");
            //    });

            //    SendMessageButton.Click += delegate { SendMessage(); };
            //    textBox_msgToSend.KeyDown += (s, e) => { if (e.Key == Key.Return) { SendMessage(); } };

            //}

            //void SendMessage()
            //{
            //    var concreteClient = Clients[Clients.IndexOf(ClientList.SelectedItem as TelegramClient)];
            //    string responseMsg = $"Support: {textBox_msgToSend}";
            //    concreteClient.Messages.Add(responseMsg);

            //    //telegramBotKeeper.Bot.SendTextMessageAsync(concreteClient.Id, textBox_msgToSend.Text);

            //    telegramBotKeeper.Bot.SendTextMessageAsync(concreteClient.Id, textBox_msgToSend.Text);

            //    string logText = $"{DateTime.Now}: >> {concreteClient.Id} {concreteClient.NickName} {responseMsg}\n";
            //    System.IO.File.AppendAllText("data.log", logText);

            //    textBox_msgToSend.Text = string.Empty;


            //}
        }
    }
}
