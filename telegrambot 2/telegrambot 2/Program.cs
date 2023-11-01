using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("6803216014:AAEDeyqRunc4Lyp9eCnlj9hdyU3-XnxwZK4");
using CancellationTokenSource cts = new();
CryptoPriceFetcher cryptoPriceFetcher = new CryptoPriceFetcher();
var (usdPrice, jpyPrice, eurPrice) = await CryptoPriceFetcher.GetBitcoinPricesAsync();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
updateHandler: HandleUpdateAsync,
pollingErrorHandler: HandlePollingErrorAsync,
receiverOptions: receiverOptions,
cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    if (message.Text == "/start")
    {
        Message messages = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Welcome to the currency Bot! Type '/usdPrice', '/jpyPrice', or '/eurPrice' to get the respective currency prices.",
        cancellationToken: cancellationToken);

    }
    else if (message.Text == "/usdPrice")
    {
        Message messages = await botClient.SendTextMessageAsync(
       chatId: chatId,
       text: $"Current USD price is : {usdPrice}",
       cancellationToken: cancellationToken);
    }
    else if (message.Text == "/eurPrice")
    {
        Message messages = await botClient.SendTextMessageAsync(
       chatId: chatId,
       text: $"Current eur price is : {eurPrice}",
       cancellationToken: cancellationToken);
    }
    else if (message.Text == "/jpyPrice")
    {
        Message messages = await botClient.SendTextMessageAsync(
       chatId: chatId,
       text: $"Current jpy price is : {jpyPrice}",
       cancellationToken: cancellationToken);
    }

}
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

