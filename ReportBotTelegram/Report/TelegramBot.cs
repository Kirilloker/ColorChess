using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


public static class TelegramBot
{
    private static ITelegramBotClient botClient_;
    private static Update update_;
    private static CancellationToken cancellationToken_;
    private static string lastUserMes = "";

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        botClient_ = botClient;
        update_ = update;
        cancellationToken_ = cancellationToken;

        if (update.Message is not { } message)
            return;
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        // Если написал сообщение не админ
        if (Config.IdAdmins.Contains(chatId.ToString()) == false)
        {
            Message mes = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Sorry, i don't have access",
                cancellationToken: cancellationToken);
        }
        else
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { KeyBoardMessage.InfoForUser },
                new KeyboardButton[] { KeyBoardMessage.CountRegistrationAll },
                new KeyboardButton[] { KeyBoardMessage.CountRegistrationInRange },
                new KeyboardButton[] { KeyBoardMessage.CountEndGameAll },
                new KeyboardButton[] { KeyBoardMessage.CountEndGameInRange},
                new KeyboardButton[] { KeyBoardMessage.CountGameNotFinishAll},
                new KeyboardButton[] { KeyBoardMessage.CountGameNotFinishInRange},
            })
            {
                ResizeKeyboard = true
            };

            string replyMessage = MessageCreator.GetReply(messageText, lastUserMes);
            lastUserMes = messageText;

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: replyMessage,
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
    }


    static public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

}



