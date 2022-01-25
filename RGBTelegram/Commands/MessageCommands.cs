using RGBTelegram.Entities;
using RGBTelegram.Services;
using RGBTelegram.vpluse;
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
        private readonly IAuthService _authService;
        private readonly IServiceCall _service;
        public MessageCommands(ISessionService sessionService, IAuthService authService, IServiceCall service, TelegramBot telegramBot)
        {
            _sessionService = sessionService;
            _authService = authService;
            _service = service;
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
                    await _sessionService.Update(session, OperationType.start);
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
                    await _sessionService.Update(session, OperationType.auth);
                    break;
                default:
                    if (update.Message.Contact != null)
                    {
                        await _authService.GetOrCreate(update.Message.Chat.Id, update.Message.Contact.PhoneNumber.Replace("+", ""));
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Укажите пароль:", ParseMode.Markdown,replyMarkup:null);
                        await _sessionService.Update(session, OperationType.telNumber);
                    }
                    else
                    {
                        switch (session.Type)
                        {
                            case OperationType.telNumber:
                                var data = await _authService.GetOrCreate(update.Message.Chat.Id);
                                data.password = text;
                                var call = await _service.AuthByPassword(data);
                                if (call.success)
                                {
                                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                                            {
                                                new InlineKeyboardButton("Об акции"){Text="Об акции", CallbackData = "Promotion"},
                                                new InlineKeyboardButton("Ввести код"){Text = "Ввести код", CallbackData = "Promocode"},
                                                new InlineKeyboardButton("Правила акции"){Text = "Правила акции", CallbackData = "ProRule"},
                                                new InlineKeyboardButton("Мои промокоды и призы"){Text = "Мои промокоды и призы", CallbackData = "MyPromocodes"},
                                                new InlineKeyboardButton("Вопросы и ответы"){Text="Об акции", CallbackData = "Questions"}
                                            });

                                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Добро пожаловать! Нажмите на одну из кнопок ниже:", ParseMode.Markdown, replyMarkup: inlineKeyboard);
                                    await _sessionService.Update(session, OperationType.menu);
                                }
                                else
                                { 
                                
                                }

                                break;
                        }
                    }
                    break;
            }

            //await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, message, ParseMode.Markdown);
        }
    }
}
