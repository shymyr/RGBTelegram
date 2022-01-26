using RGBTelegram.Entities;
using RGBTelegram.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RGBTelegram.Commands
{
    public class CallbackCommands : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly ISessionService _sessionService;
        public CallbackCommands(ISessionService sessionService, TelegramBot telegramBot)
        {
            _sessionService = sessionService;
            _botClient = telegramBot.GetBot().Result;
        }
        public override string Name => "callback";

        public override async Task ExecuteAsync(Update update, UserSession session)
        {
            var text = update.CallbackQuery.Data;
            switch (text)
            {
                case "Promotion"://"Об акции"
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Добро пожаловать в Бот!", ParseMode.Markdown);
                    await _sessionService.Update(session, OperationType.registration);
                    break;
                case "Promocode":

                    break;
                case "ProRule": //"Правила акции"
        
                    break;
                case "MyPromocodes"://"Мои промокоды и призы"

                    break;
                case "Questions"://"Вопросы и ответы"

                    break;
            }
        }
    }
}
