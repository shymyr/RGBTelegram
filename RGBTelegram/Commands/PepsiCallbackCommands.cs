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
    public class PepsiCallbackCommands : PepsiBaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly ISessionService _sessionService;
        private readonly IRegService _regService;
        private readonly IServiceCall _service;
        private readonly ILanguageText _languageText;
        public PepsiCallbackCommands(ISessionService sessionService, IServiceCall service, IRegService regService, PepsiBot telegramBot, ILanguageText languageText)
        {
            _sessionService = sessionService;
            _botClient = telegramBot.GetPepsiBot().Result;
            _regService = regService;
            _service = service;
            _languageText = languageText;
        }
        public override string Name => "callback";

        public override async Task ExecuteAsync(Update update, PepsiSession session)
        {
            var text = update.CallbackQuery.Data;
            PepsiRegistration registration = new PepsiRegistration();
            var ChatId = update.CallbackQuery.Message.Chat.Id;
            StringBuilder resp = new StringBuilder();

            var mainMenu = _languageText.GetKeyboard(session);
            switch (text)
            {
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
                        await _sessionService.PepsiUpdate(session, OperationType.start);
                    }
                    else
                    {
                        await _sessionService.PepsiUpdate(session, OperationType.menu);
                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.country, session.language),
                       ParseMode.Markdown, replyMarkup: _languageText.GetKeyboard(session));
                    }
                    break;
                case "Skip":
                    registration = await _regService.PepsiGetOrCreate(ChatId);
                    await _regService.PepsiUpdate(registration, ChatId, middlename: text);
                    await _sessionService.PepsiUpdate(session, OperationType.birth_day);
                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.birth_day, session.language), ParseMode.Markdown);
                    break;
                default:
                    switch (session.Type)
                    {
                        case OperationType.gender:
                            registration = await _regService.PepsiGetOrCreate(ChatId);
                            await _regService.PepsiUpdate(registration, ChatId, gender: text);

                            var sign = await _service.Register(registration);
                            if (sign.success)
                            {
                                await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regSMS, session.language), parseMode: ParseMode.Markdown);
                                await _sessionService.PepsiUpdate(session, OperationType.regSMSConfirm);
                            }
                            else
                            {
                                await _botClient.SendTextMessageAsync(ChatId, sign.message + ". " + sign.field,
                                   ParseMode.Markdown, replyMarkup: mainMenu);
                            }
                            break;
                        case OperationType.country:
                            var lan = text == "kazLanguage" ? Language.KAZ : (text == "kgzLanguage" ? Language.KGZ : Language.Rus);
                            await _sessionService.PepsiUpdate(session, OperationType.language, language: lan);
                            resp.AppendLine(await _languageText.GetTextFromLanguage(OperationType.country, lan));
                            await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: _languageText.GetKeyboard(session));
                            break;

                        case OperationType.regregion:
                            registration = await _regService.PepsiGetOrCreate(ChatId);
                            await _regService.PepsiUpdate(registration, ChatId, region_id: int.Parse(text));
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
                                await _sessionService.PepsiUpdate(session, OperationType.regcity);
                            }
                            break;
                        case OperationType.regcity:
                            registration = await _regService.PepsiGetOrCreate(ChatId);
                            await _regService.PepsiUpdate(registration, ChatId, city_id: int.Parse(text));
                            if (session.country == Country.KAZ)
                            {
                                await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Укажите ИИН:" : "ЖСН енгізіңіз:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.PepsiUpdate(session, OperationType.regIIN);
                            }
                            else
                            {
                                await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Введи свою фамилию:" : "Фамилияңды киргиз:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.PepsiUpdate(session, OperationType.first_name);
                            }
                            break;
                    }
                    break;
            }

            await _botClient.DeleteMessageAsync(ChatId, update.CallbackQuery.Message.MessageId);
        }
    }
}
