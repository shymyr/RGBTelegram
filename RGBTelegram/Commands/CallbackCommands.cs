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

            StringBuilder resp = new StringBuilder();
            var ChatId = update.CallbackQuery.Message.Chat.Id;
            switch (text)
            {
                case "KAZ":
                    resp.AppendLine("Тілді таңдаңыз.");
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
                    await _sessionService.Update(session, OperationType.auth);
                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regTelNumber, session.language), 
                        ParseMode.Markdown, replyMarkup: _languageText.GetPhoneMarkup(session.language));                   
                    break;
                case "Registration":
                    await _sessionService.Update(session, OperationType.regTelNumber);
                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regTelNumber, session.language),
                        ParseMode.Markdown, replyMarkup: _languageText.GetPhoneMarkup(session.language));

                    break;
                case "Menu"://Главное меню
                    if (session.country == null)
                    {
                        resp.AppendLine("Елді таңдаңыз:");
                        resp.AppendLine("Выберите страну:");
                        var country = new InlineKeyboardMarkup(new[]
                                              {
                                                new[]{ new InlineKeyboardButton("Қазақстан") { Text = "Қазақстан", CallbackData = "KAZ" } },
                                                 new[]{ new InlineKeyboardButton("Кыргызстан ") { Text = "Кыргызстан ", CallbackData = "KGZ" } }
                                            });
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, resp.ToString(), ParseMode.Markdown, replyMarkup: country);
                        await _sessionService.Update(session, OperationType.start);
                    }
                    else
                    {
                        await _sessionService.Update(session, OperationType.menu);
                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.country, session.language),
                       ParseMode.Markdown, replyMarkup: _languageText.GetKeyboard(session));
                    }
                    break;
                case "Promotion"://"Об акции"
                    if (session.country == null || session.language == null)
                    {
                        resp.AppendLine("Әрі қарай қызмет алу үшін ел мен тілді таңдаңыз");
                        resp.AppendLine("Для дальнешего действия, выберите страну и язык");
                        resp.AppendLine("Андан аркы аракет үчүн өлкөңүздү жана тилиңизди тандаңыз");
                        var country = new InlineKeyboardMarkup(new[]
                                               {
                                                new[]{ new InlineKeyboardButton("Қазақстан") { Text = "Қазақстан", CallbackData = "KAZ" } },
                                                 new[]{ new InlineKeyboardButton("Кыргызстан ") { Text = "Кыргызстан ", CallbackData = "KGZ" } }
                                            });
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, resp.ToString(), ParseMode.Markdown, replyMarkup: country);
                        await _sessionService.Update(session, OperationType.start);
                    }
                    else
                    {
                        if (session.language != null)
                        {
                            var about = await _service.About(((int)session.country), session.language);
                            await _botClient.SendTextMessageAsync(ChatId, about.Items.FirstOrDefault().name, ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, session.Authorized));
                            await _sessionService.Update(session, OperationType.Promotion);
                        }
                    }
                    break;
                case "Promocode"://Ввести код

                    break;
                case "ProRule": //"Правила акции"
                    await _botClient.SendTextMessageAsync(ChatId, "Ждём текст или событию про Правила акции!", ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, session.Authorized));
                    await _sessionService.Update(session, OperationType.ProRule);
                    break;
                case "MyPromocodes"://"Мои промокоды и призы"
                    await _botClient.SendTextMessageAsync(ChatId, "Ждём текст или событию про Мои промокоды и призы!", ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, session.Authorized));
                    await _sessionService.Update(session, OperationType.MyPromocodes);
                    break;
                case "Questions"://"Вопросы и ответы"
                    await _botClient.SendTextMessageAsync(ChatId, "Ждём текст или событию про Вопросы и ответы!", ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, session.Authorized));
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
                            await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: _languageText.GetKeyboard(session));
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
