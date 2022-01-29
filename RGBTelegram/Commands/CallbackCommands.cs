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
            var mainMenu = new InlineKeyboardMarkup(new[]
                                            {
                                                new[]{ new InlineKeyboardButton("Главное меню") { Text = "Главное меню", CallbackData = "Menu" } }
                                            });
            switch (text)
            {
                case "Authorization":
                    var phone = new ReplyKeyboardMarkup(new[]
                                          {
                                                    new KeyboardButton("Поделиться телефоном")
                                                     {
                                                        RequestContact = true
                                                     }
                                                });
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Отправьте свой номер телефона с помощью кнопки \"Поделиться телефоном\"", ParseMode.Markdown, replyMarkup: phone);
                    await _sessionService.Update(session, OperationType.auth);
                    break;
                case "Registration":
                    var ph = new ReplyKeyboardMarkup(new[]
                                          {
                                                    new KeyboardButton("Поделиться телефоном")
                                                     {
                                                        RequestContact = true
                                                     }
                                                });
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Для регистрации отправьте номер телефона", ParseMode.Markdown, replyMarkup: ph);
                    await _sessionService.Update(session, OperationType.regTelNumber);
                    break;
                case "Menu"://Главное меню
                    InlineKeyboardMarkup inlineKeyboard;
                    if (session.Authorized)
                    {
                        inlineKeyboard = new InlineKeyboardMarkup(new[]
                                                {
                                                new[]{ new InlineKeyboardButton("Об акции"){Text="Об акции", CallbackData = "Promotion"}},
                                                new[]{ new InlineKeyboardButton("Ввести код"){Text = "Ввести код", CallbackData = "Promocode"} },
                                                new[]{ new InlineKeyboardButton("Правила акции"){Text = "Правила акции", CallbackData = "ProRule"}},
                                                new[]{ new InlineKeyboardButton("Мои промокоды и призы"){Text = "Мои промокоды и призы", CallbackData = "MyPromocodes"} },
                                                new[]{ new InlineKeyboardButton("Вопросы и ответы") { Text = "Вопросы и ответы", CallbackData = "Questions" } }
                                            });
                    }
                    else
                        inlineKeyboard = new InlineKeyboardMarkup(new[]
                                           {
                                                new[]{ new InlineKeyboardButton("Авторизация") {Text= "Авторизация", CallbackData = "Authorization"}},
                                                new[]{ new InlineKeyboardButton("Регистрация") {Text= "Регистрация", CallbackData = "Registration"}}
                                            });

                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Выберите необходимую операцию из нижеперечисленных кнопок:", ParseMode.Markdown, replyMarkup: inlineKeyboard);
                    await _sessionService.Update(session, OperationType.menu);
                    break;
                case "Promotion"://"Об акции"
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Ждём текст или событию об Акции!", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.Promotion);
                    break;
                case "Promocode"://Ввести код

                    break;
                case "ProRule": //"Правила акции"
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Ждём текст или событию про Правила акции!", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.ProRule);
                    break;
                case "MyPromocodes"://"Мои промокоды и призы"
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Ждём текст или событию про Мои промокоды и призы!", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.MyPromocodes);
                    break;
                case "Questions"://"Вопросы и ответы"
                    await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Ждём текст или событию про Вопросы и ответы!", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.Questions);
                    break;
            }
        }
    }
}
