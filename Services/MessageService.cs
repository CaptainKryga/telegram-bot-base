using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Service.Services;

public class MessageService(ITelegramBotClient botClient)
{
	public async Task SendWelcomeMessage(long chatId, CancellationToken cancellationToken)
	{
		await botClient.SendMessage(
			chatId: chatId,
			text: "Hello!\n",
			cancellationToken: cancellationToken);
	}
}