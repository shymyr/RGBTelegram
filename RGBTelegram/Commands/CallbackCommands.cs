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
using Telegram.Bot.Types.InputFiles;
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
            InlineKeyboardMarkup mainMenu = new InlineKeyboardMarkup(new[]
                                           {
                                                new[]{ new InlineKeyboardButton("Меню") { Text = "Меню", CallbackData = "mainmenu" } }
                                            });


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
                    if (session.country == 0)
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
                case "Skip":
                    registration = await _regService.GetOrCreate(ChatId);
                    await _regService.Update(registration, ChatId, middlename: text);
                    await _sessionService.Update(session, OperationType.birth_day);
                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.birth_day, session.language), ParseMode.Markdown);
                    break;
                case "mainmenu":
                    if (session.country == 0 || session.language == 0)
                    {
                        resp.AppendLine("Әрі қарай қызмет алу үшін ел мен тілді таңдаңыз");
                        resp.AppendLine("Для дальнешего действия, выберите страну и язык");
                        resp.AppendLine("Андан аркы аракет үчүн өлкөңүздү жана тилиңизди тандаңыз");
                        var country = new InlineKeyboardMarkup(new[]
                                               {
                                                new[]{ new InlineKeyboardButton("Қазақстан") { Text = "Қазақстан", CallbackData = "KAZ" } },
                                                 new[]{ new InlineKeyboardButton("Кыргызстан ") { Text = "Кыргызстан ", CallbackData = "KGZ" } }
                                            });
                        await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: country);
                        await _sessionService.Update(session, OperationType.menu);
                    }
                    else
                    {
                        await _sessionService.Update(session, OperationType.menu);
                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.menu, session.language),
                            ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, session.Authorized));

                    }
                    break;
                case "History"://История промокодов
                    if (session.country == 0 || session.language == 0)
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
                        var history = await _service.GetBundles(session.Token);
                        switch (history.status)
                        {
                            case 200:
                                resp.AppendLine(session.language == Language.KAZ ? "Сыйлықтар:" : (session.language == Language.Rus ? "Подарки:" : "Белектер:"));

                                history.gifts.ForEach(gift =>
                                {
                                    resp.AppendLine("   - " + (session.language == Language.KAZ ? gift.namekz : (session.language == Language.Rus ? gift.nameru : gift.namekg)));
                                });
                                resp.AppendLine("Промокод:");
                                history.attempts.ForEach(att =>
                                {
                                    resp.AppendLine("   - " + att.promocode);
                                });

                                await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: mainMenu);
                                
                                break;
                            case 401:
                                switch (session.language)
                                {
                                    case Language.Rus:
                                        resp.AppendLine("Промокодтардың тарихын алу үшін жүйеге кіру керек.");
                                        break;
                                    case Language.KGZ:
                                        resp.AppendLine("Промокоддордун тарыхын алуу үчүн сиз кирүү керек.");
                                        break;
                                    case Language.KAZ:
                                        resp.AppendLine("Для получения историю промокодов, необходимо пройти авторизоваться.");
                                        break;
                                }
                                await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: mainMenu);
                                break;
                            default:

                                break;
                        }
                       
                    }
                    break;
                case "Promotion"://"Об акции"
                    if (session.country == 0 || session.language == 0)
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
                        var about = await _service.About(((int)session.country), session.language);
                        await _botClient.SendTextMessageAsync(ChatId, about.Items.FirstOrDefault().name, ParseMode.Markdown, replyMarkup: mainMenu);
                        await _sessionService.Update(session, OperationType.Promotion);
                    }
                    break;
                case "Promocode"://Ввести код
                    await _botClient.SendTextMessageAsync(ChatId, "Промокод:", ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.Promocode);
                    break;
                case "ProRule": //"Правила акции"
                    if (session.country == 0 || session.language == 0)
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
                        var terms = await _service.Terms(((int)session.country), session.language);
                        if (terms.success)
                        {
                            await _botClient.SendDocumentAsync(
                              chatId: ChatId,
                              document: new InputOnlineFile(new Uri(terms.Items.FirstOrDefault(x => x.id == 2).name)),
                              caption: terms.Items.FirstOrDefault(x => x.id == 1).name
                          );
                            await _sessionService.Update(session, OperationType.ProRule);
                        }
                        else
                            await _botClient.SendTextMessageAsync(683782447, terms.message);
                    }
                    break;
                case "CallCenter"://"Телефон горячей линии"
                    await _botClient.SendTextMessageAsync(ChatId, _languageText.GetCallCenter(session.country, session.language), ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.menu);
                    break;
                case "WinnerList"://"Список победителей"
                    await _botClient.SendTextMessageAsync(ChatId, _languageText.GetWinnerList(session.country, session.language), ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.menu);
                    break;
                default:
                    switch (session.Type)
                    {
                        case OperationType.gender:
                            registration = await _regService.GetOrCreate(ChatId);
                            await _regService.Update(registration, ChatId, gender: text);

                            var sign = await _service.Register(registration);
                            if (sign.success)
                            {
                                await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regSMS, session.language), parseMode: ParseMode.Markdown);
                                await _sessionService.Update(session, OperationType.regSMSConfirm);
                            }
                            else
                            {
                                await _botClient.SendTextMessageAsync(ChatId, sign.message + ". " + sign.field,
                                   ParseMode.Markdown, replyMarkup: mainMenu);
                            }
                            break;
                        case OperationType.country:
                            var lan = text == "kazLanguage" ? Language.KAZ : (text == "kgzLanguage" ? Language.KGZ : Language.Rus);
                            await _sessionService.Update(session, OperationType.language, language: lan);
                            resp.AppendLine(await _languageText.GetTextFromLanguage(OperationType.country, lan));
                            await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: _languageText.GetKeyboard(session));
                            break;

                        case OperationType.regregion:
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
                                await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regcity, session.language), replyMarkup: city);
                                await _sessionService.Update(session, OperationType.regcity);
                            }
                            break;
                        case OperationType.regcity:
                            registration = await _regService.GetOrCreate(ChatId);
                            await _regService.Update(registration, ChatId, city_id: int.Parse(text));
                            if (session.country == Country.KAZ)
                            {
                                await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Укажите ИИН:" : "ЖСН енгізіңіз:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.regIIN);
                            }
                            else
                            {
                                await _botClient.SendTextMessageAsync(ChatId, "Фамилия:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.first_name);
                            }
                            break;
                    }
                    break;
            }
            await _botClient.DeleteMessageAsync(ChatId, update.CallbackQuery.Message.MessageId);
        }
    }
}
