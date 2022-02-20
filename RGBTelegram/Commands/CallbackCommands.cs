using RGBTelegram.Entities;
using RGBTelegram.Services;
using RGBTelegram.vpluse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IRegService _regService;
        private readonly IServiceCall _service;
        private readonly ILanguageText _languageText;
        public CallbackCommands(ISessionService sessionService, IServiceCall service, IRegService regService, TelegramBot telegramBot, ILanguageText languageText)
        {
            _sessionService = sessionService;
            _botClient = telegramBot.GetBot().Result;
            _regService = regService;
            _service = service;
            _languageText = languageText;
        }
        public override string Name => "callback";

        public override async Task ExecuteAsync(Update update, UserSession session)
        {
            var text = update.CallbackQuery.Data;
            Registration registration = new Registration();
            var mainMenu = new InlineKeyboardMarkup(new[]
                                            {
                                                new[]{ new InlineKeyboardButton("Главное меню") { Text = "Главное меню", CallbackData = "Menu" } }
                                            });
            StringBuilder resp = new StringBuilder();
            var ChatId = update.CallbackQuery.Message.Chat.Id;
            switch (text)
            {
                case "KAZ":
                    resp.AppendLine("Тілді таңдаңыз.");
                    resp.AppendLine("Тилди тандаңыз.");
                    resp.AppendLine("Выберите язык.");
                    var language = new InlineKeyboardMarkup(new[]
                                          {
                                                new[]{ new InlineKeyboardButton("Қазақ тілі") { Text = "Қазақ тілі", CallbackData = "kazLanguage" } },
                                                new[]{ new InlineKeyboardButton("Русский") { Text = "Русский ", CallbackData = "ruLanguage" } }
                                            });
                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: language);
                    await _sessionService.Update(session, OperationType.country, country: text == "KAZ" ? Country.KAZ : Country.KGZ);
                    break;
                case "KGZ":
                    resp.AppendLine("Тилди тандаңыз.");
                    resp.AppendLine("Выберите язык.");
                    var language1 = new InlineKeyboardMarkup(new[]
                                          {
                                              new[]{ new InlineKeyboardButton("Кыргызский ") { Text = "Кыргызский ", CallbackData = "kgzLanguage" } },
                                              new[]{ new InlineKeyboardButton("Русский") { Text = "Русский ", CallbackData = "ruLanguage" } }
                                            });
                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: language1);
                    await _sessionService.Update(session, OperationType.country, country: text == "KAZ" ? Country.KAZ : Country.KGZ);
                    break;
                case "Authorization":
                    var phone = new ReplyKeyboardMarkup(new[]
                                          {
                                                    new KeyboardButton("Поделиться телефоном")
                                                     {
                                                        RequestContact = true
                                                     }
                                                });
                    await _botClient.SendTextMessageAsync(ChatId, "Отправьте свой номер телефона с помощью кнопки \"Поделиться телефоном\"", ParseMode.Markdown, replyMarkup: phone);
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
                    await _botClient.SendTextMessageAsync(ChatId, "Для регистрации отправьте номер телефона", ParseMode.Markdown, replyMarkup: ph);
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
                                                new[]{ new InlineKeyboardButton("Об акции"){Text="Об акции", CallbackData = "Promotion"}},
                                                new[]{ new InlineKeyboardButton("Авторизация") {Text= "Авторизация", CallbackData = "Authorization"}},
                                                new[]{ new InlineKeyboardButton("Регистрация") {Text= "Регистрация", CallbackData = "Registration"}}
                                            });
                    await _botClient.SendTextMessageAsync(ChatId, "Выберите необходимую операцию из нижеперечисленных кнопок:", ParseMode.Markdown, replyMarkup: inlineKeyboard);
                    await _sessionService.Update(session, OperationType.menu);
                    break;
                case "Promotion"://"Об акции"
                    await _botClient.SendTextMessageAsync(ChatId, "Ждём текст или событию об Акции!", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.Promotion);
                    break;
                case "Promocode"://Ввести код

                    break;
                case "ProRule": //"Правила акции"
                    await _botClient.SendTextMessageAsync(ChatId, "Ждём текст или событию про Правила акции!", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.ProRule);
                    break;
                case "MyPromocodes"://"Мои промокоды и призы"
                    await _botClient.SendTextMessageAsync(ChatId, "Ждём текст или событию про Мои промокоды и призы!", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.MyPromocodes);
                    break;
                case "Questions"://"Вопросы и ответы"
                    await _botClient.SendTextMessageAsync(ChatId, "Ждём текст или событию про Вопросы и ответы!", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.Questions);
                    break;
                case "fam1":
                case "fam2":
                case "fam3":
                case "fam4":
                    var ss = text.ToCharArray().Last();
                    registration = await _regService.GetOrCreate(ChatId);
                    await _regService.Update(registration, ChatId, family_stat: ss.ToString());
                    var regions = await _service.GetRegions();
                    if (regions.status == 200)
                    {
                        List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();
                        regions.Items.ForEach(rr =>
                        {
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton(rr.name) { Text = rr.name, CallbackData = rr.id.ToString() } });
                        });
                        var regs = new InlineKeyboardMarkup(Buttons);

                        await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Выберите регион:", replyMarkup: regs);
                        await _sessionService.Update(session, OperationType.regcity);
                    }
                    break;
                default:
                    switch (session.Type)
                    {
                        case OperationType.country:
                            var lan = text == "kazLanguage" ? Language.KAZ : (text == "kgzLanguage" ? Language.KGZ : Language.Rus);
                            await _sessionService.Update(session, OperationType.language, language: lan);
                            resp.AppendLine(await _languageText.GetTextFromLanguage(OperationType.country, lan));
                            inlineKeyboard = _languageText.GetKeyboard(session);
                            await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: inlineKeyboard);
                            break;
                        case OperationType.regcity:
                            registration = await _regService.GetOrCreate(ChatId);
                            await _regService.Update(registration, ChatId, region_id: int.Parse(text));
                            var cities = await _service.GetCities(registration.region_id);
                            if (cities.status == 200)
                            {
                                List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();
                                cities.Items.ForEach(rr =>
                                {
                                    Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton(rr.name) { Text = rr.name, CallbackData = rr.id.ToString() } });
                                });
                                var city = new InlineKeyboardMarkup(Buttons);
                                await _botClient.SendTextMessageAsync(ChatId, "Выберите город:", replyMarkup: city);
                                await _sessionService.Update(session, OperationType.regIIN);
                            }
                            break;
                        case OperationType.regIIN:
                            registration = await _regService.GetOrCreate(ChatId);
                            await _regService.Update(registration, ChatId, city_id: int.Parse(text));
                            await _botClient.SendTextMessageAsync(ChatId, "Укажите ИИН:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                            await _sessionService.Update(session, OperationType.regSMS);
                            break;
                    }
                    break;
            }
            await _botClient.DeleteMessageAsync(ChatId, update.CallbackQuery.Message.MessageId);
        }
    }
}
