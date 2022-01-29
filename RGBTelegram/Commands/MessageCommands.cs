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
            var mainMenu = new InlineKeyboardMarkup(new[]
                                            {
                                                new[]{ new InlineKeyboardButton("Главное меню") { Text = "Главное меню", CallbackData = "Menu" } }
                                            });
            switch (text)
            {
                case "/start":
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Добро пожаловать в Бот! Здесь вы можете Нужен текст что будет здесь", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.start);
                    break;
                case "Авторизация":
                    var phone = new ReplyKeyboardMarkup(new[]
                                           {
                                                    new KeyboardButton("Поделиться телефоном")
                                                     {
                                                        RequestContact = true
                                                     }
                                                });
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Отправьте свой номер телефона с помощью кнопки \"Поделиться телефоном\"", ParseMode.Markdown, replyMarkup: phone);
                    await _sessionService.Update(session, OperationType.auth);
                    break;
                case "Регистрация":
                    var ph = new ReplyKeyboardMarkup(new[]
                                           {
                                                    new KeyboardButton("Поделиться телефоном")
                                                     {
                                                        RequestContact = true
                                                     }
                                                });
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Для регистрации отправьте номер телефона", ParseMode.Markdown, replyMarkup: ph);
                    await _sessionService.Update(session, OperationType.regTelNumber);

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
                            if (session.Type == OperationType.regTelNumber)
                            {
                                var call = await _service.CheckPhone(update.Message.Contact.PhoneNumber.Replace("+", ""));
                                if (call.status == 200)
                                {
                                    if (!call.success)
                                    {
                                        await _regService.GetOrCreate(update.Message.Chat.Id, update.Message.Contact.PhoneNumber.Replace("+", ""));
                                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                        await _sessionService.Update(session, OperationType.regPass);
                                    }
                                    else
                                    {
                                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                        await _sessionService.Update(session, OperationType.regPass);
                                    }
                                }
                                else
                                { }
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

                                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Выберите необходимую операцию из нижеперечисленных кнопок:", ParseMode.Markdown, replyMarkup: inlineKeyboard);
                                    await _sessionService.Update(session, OperationType.menu, authorised: true);
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, call.data.FirstOrDefault().message + ". Попробуйте заново ввести пароль", ParseMode.Markdown);
                                }
                                #endregion
                                break;
                            case OperationType.regTelNumber:
                                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.regPass);
                                break;
                            case OperationType.regPass:
                                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Укажите фамилию", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.regFName);
                                break;
                            case OperationType.regFName:
                                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.regLName);
                                break;
                            case OperationType.regLName:
                                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.regMName);
                                break;
                            case OperationType.regMName:
                                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.reggender);
                                break;
                            case OperationType.reggender:
                                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.regfamily_stat);
                                break;
                            default:
                                await _botClient.SendTextMessageAsync(update.Message.Chat.Id, "Извините, я вас не понимаю. Выберите действию доступную вам!", ParseMode.Markdown, replyMarkup: mainMenu);
                                break;
                        }
                    }
                    break;
            }

            //await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, message, ParseMode.Markdown);
        }
    }
}
