using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Service.Services;

namespace TelegramBot.Service.Controllers;

public class CommandController
{
	private readonly MessageService _messageService;

	public CommandController(MessageService messageService)
	{
		_messageService = messageService;
	}

	public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
	{
		if (update.Message is not { } message)
			return;

		if (message.Text is not { } messageText)
			return;

		var chatId = message.Chat.Id;

		switch (messageText)
		{
			case "/start":
				await _messageService.SendWelcomeMessage(chatId, cancellationToken);
				break;
		}
	}
}