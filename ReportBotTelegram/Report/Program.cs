using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient(Config.Token);
using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};


botClient.StartReceiving(
    updateHandler: TelegramBot.HandleUpdateAsync,
    pollingErrorHandler: TelegramBot.HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Bot start");
Console.ReadLine();

cts.Cancel();