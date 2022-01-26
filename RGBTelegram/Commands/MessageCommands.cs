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
        private readonly IRegService _regService;
        public MessageCommands(ISessionService sessionService, IAuthService authService, IServiceCall service, IRegService regService, TelegramBot telegramBot)
        {
            _sessionService = sessionService;
            _authService = authService;
            _service = service;
            _regService = regService;
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
                case "Регистрация":
                    var ph = new ReplyKeyboardMarkup(new[]
                                           {
                                                    new KeyboardButton("Отправить номер телефона")
                                                     {
                                                        RequestContact = true
                                                     }
                                                });
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Для регистрации отправьте номер телефона", ParseMode.Markdown, replyMarkup: ph);
                    await _sessionService.Update(session, OperationType.registration);
                    
                    break;
                default:
                    if (update.Message.Contact != null)
                    {
                        if (session.Type == OperationType.auth)
                        {
                            await _authService.GetOrCreate(update.Message.Chat.Id, update.Message.Contact.PhoneNumber.Replace("+", ""));
                            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Укажите пароль:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                            await _sessionService.Update(session, OperationType.telNumber);
                        }
                        else
                        {
                            if (session.Type == OperationType.registration)
                            {
                                await _regService.GetOrCreate(update.Message.Chat.Id, update.Message.Contact.PhoneNumber.Replace("+", ""));
                            }
                        }
                    }
                    else
                    {
                        switch (session.Type)
                        {
                            case OperationType.telNumber:
                                #region Keyboards
                                var data = await _authService.GetOrCreate(update.Message.Chat.Id);
                                data.password = text;
                                await _authService.Update(data, passwod: text);
                                var call = await _service.AuthByPassword(data);
                                if (call.success)
                                {
                                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                                            {
                                                new[]{ new InlineKeyboardButton("Об акции"){Text="Об акции", CallbackData = "Promotion"}},
                                                new[]{ new InlineKeyboardButton("Ввести код"){Text = "Ввести код", CallbackData = "Promocode"} },
                                                new[]{ new InlineKeyboardButton("Правила акции"){Text = "Правила акции", CallbackData = "ProRule"}},
                                                new[]{ new InlineKeyboardButton("Мои промокоды и призы"){Text = "Мои промокоды и призы", CallbackData = "MyPromocodes"} },
                                                new[]{ new InlineKeyboardButton("Вопросы и ответы") { Text = "Вопросы и ответы", CallbackData = "Questions" } }
                                            });

                                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Добро пожаловать! Нажмите на одну из кнопок ниже:", ParseMode.Markdown, replyMarkup: inlineKeyboard);
                                    await _sessionService.Update(session, OperationType.menu);
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, call.data.FirstOrDefault().message + ". Попробуйте заново ввести пароль", ParseMode.Markdown);
                                }
                                #endregion
                                break;
                            case OperationType.regTelNumber:

                                break;
                        }
                    }
                    break;
            }

            //await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, message, ParseMode.Markdown);
        }
    }
}
