using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBot.Service.Controllers;
using TelegramBot.Service.Services;

string botToken = "######################################";

var botClient = new TelegramBotClient(botToken);
var receiverOptions = new ReceiverOptions
{
	AllowedUpdates = Array.Empty<UpdateType>(),
	DropPendingUpdates = true
};

//services
var messageService = new MessageService(botClient);
var commandController = new CommandController(messageService);

using var cts = new CancellationTokenSource();

botClient.StartReceiving(
	updateHandler: commandController.HandleUpdateAsync,
	errorHandler: HandlePollingErrorAsync,
	receiverOptions: receiverOptions,
	cancellationToken: cts.Token
);

var me = await botClient.GetMe();
Console.WriteLine($"Bot @{me.Username} activate!");
Console.WriteLine("< << <<< LOOP >>> >> >");
Console.ReadKey();

cts.Cancel();

static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
	var errorMessage = exception switch
	{
		ApiRequestException apiRequestException
			=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
		_ => exception.ToString()
	};

	Console.WriteLine(errorMessage);
	return Task.CompletedTask;
}