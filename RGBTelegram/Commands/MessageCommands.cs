using RGBTelegram.Entities;
using RGBTelegram.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RGBTelegram.Commands
{
    public class MessageCommands : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly ISessionService _sessionService;
        public MessageCommands(ISessionService sessionService, TelegramBot telegramBot)
        {
            _sessionService = sessionService;
            _botClient = telegramBot.GetBot().Result;
        }
        public override string Name => "message";
        public override async Task ExecuteAsync(Update update, UserSession session)
        {
            var text = update.Message.Text;

            switch (text)
            {
                case "/start":
                    var keyboard =
                         new ReplyKeyboardMarkup(new[]
                         {
                             new[] { new KeyboardButton("Авторизация") },
                             new[] { new KeyboardButton("Регистрация") }
                         });

                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Добро пожаловать в Бот!", ParseMode.Markdown, replyMarkup: keyboard);
                    await _sessionService.Update(session);
                    break;
                case "Авторизация":
                    var phone = new ReplyKeyboardMarkup(new[]
                                           {
                                                    new KeyboardButton("Отправить номер телефона")
                                                     {
                                                        RequestContact = true
                                                     }
                                                });
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Для авторизации отправьте номер телефона", ParseMode.Markdown, replyMarkup: phone);
                    await _sessionService.Update(session);
                    break;
                default:
                    switch (session.Type)
                    {
                        case OperationType.Passw:

                            break;
                    }
                    break;
            }

            //await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, message, ParseMode.Markdown);
        }
    }
}
