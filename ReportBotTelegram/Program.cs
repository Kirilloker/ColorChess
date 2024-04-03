using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

Config.LoadConfig();

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


// Чтобы не закрывался после начала
while (true) {
    await Task.Delay(1000); 
}


cts.Cancel();