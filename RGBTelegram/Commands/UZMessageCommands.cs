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
    public class UZMessageCommands : UZBaseCommand
    {
        private readonly ISessionService _sessionService;
        private readonly IServiceCall _service;
        private readonly IRegService _regService;
        private readonly ILanguageText _languageText;
        public UZMessageCommands(ISessionService sessionService, IServiceCall service, IRegService regService, ILanguageText languageText)
        {
            _sessionService = sessionService;
            _service = service;
            _regService = regService;
            _languageText = languageText;
        }
        public override string Name => "message";

        public override async Task ExecuteAsync(Update update, UZSession session, TelegramBotClient _botClient)
        {
            var text = update.Message.Text;
            var ChatId = update.Message.Chat.Id;
            //await _botClient.SendTextMessageAsync(ChatId, text);
            UZRegistration registration = new UZRegistration();
            StringBuilder resp = new StringBuilder();
            var me = _botClient.GetMeAsync().Result.Username;
            switch (text)
            {
                #region Registration
                case "Регистрация":
                case "Ro‘yxatdan o‘tish":
                    ReplyKeyboardMarkup phone;
                    if (text == "Регистрация")
                    {
                        resp.AppendLine("Для регистрации отправьте мне свой номер телефона с помощью кнопки «Поделиться телефоном»");
                        phone = new ReplyKeyboardMarkup(new KeyboardButton("Поделиться телефоном") { Text = "Поделиться телефоном", RequestContact = true });
                    }
                    else
                    {
                        resp.AppendLine("Telefon raqamingizni “Telefonni ulashish” tugmasi yordamida yuboring");
                        phone = new ReplyKeyboardMarkup(new KeyboardButton("Telefonni ulashish") { Text = "Telefonni ulashish", RequestContact = true });
                    }
                    phone.ResizeKeyboard = true;
                    phone.OneTimeKeyboard = true;
                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: phone);
                    await _sessionService.UZUpdate(session, UZOperType.register);
                    break;
                #endregion
                #region "Правила акции"
                case "Правила акции":
                case "Aksiya Qoidalari":
                    var terms = me == "Asu_promo_bot" ? await _service.TermsASU( session.language): await _service.TermsPiala(session.language);
                    if (terms.success)
                    {
                        await _botClient.SendDocumentAsync(
                          chatId: ChatId,
                          document: new InputOnlineFile(new Uri(terms.Items.FirstOrDefault(x => x.id == 2).name)),
                          caption: terms.Items.FirstOrDefault(x => x.id == 1).name
                      );
                        await _sessionService.UZUpdate(session, UZOperType.menu);
                    }
                    else
                        await _botClient.SendTextMessageAsync(ChatId, terms.message);
                    break;
                #endregion
                #region "Об Акции"
                case "Об Акции":
                case "Aksiya haqida":                    
                    var about = me == "Asu_promo_bot" ? await _service.AboutASU(session.language) : await _service.AboutPiala(session.language);
                    if (about.success)
                    {
                        await _botClient.SendTextMessageAsync(ChatId, about.message, ParseMode.Markdown, replyMarkup: _languageText.GetUZKeyboard(UZOperType.menu, session.language));
                        await _sessionService.UZUpdate(session, UZOperType.menu);
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(ChatId, about.message, parseMode: ParseMode.Markdown, replyMarkup: _languageText.GetUZKeyboard(UZOperType.menu, session.language));
                    }

                    break;
                #endregion
                #region "Список ЦВП"
                case "Список ЦВП":
                case "SBM ro‘yxati":
                   
                    break;
                #endregion
                #region "Вопросы и ответы"
                case "Вопросы и ответы":
                case "Savollar va javoblar":
                    var faqs = me == "Asu_promo_bot" ? await _service.FaqsAsu(session.language) : await _service.FaqsPiala(session.language);
                    if (faqs.success)
                    {
                        await _botClient.SendDocumentAsync(
                          chatId: ChatId,
                          document: new InputOnlineFile(new Uri(faqs.Items.FirstOrDefault(x => x.id == 2).name)),
                          replyMarkup: _languageText.GetUZKeyboard(UZOperType.menu, session.language)
                      );
                        //await _botClient.SendTextMessageAsync(ChatId, faqs.message,replyMarkup: _languageText.GetUZKeyboard(UZOperType.menu, session.language));
                        await _sessionService.UZUpdate(session, UZOperType.menu);
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(ChatId, faqs.message, parseMode: ParseMode.Markdown, replyMarkup: _languageText.GetUZKeyboard(UZOperType.menu, session.language));
                    }
                    break;
                #endregion
                case "/language":
                    await _sessionService.UZUpdate(session, UZOperType.language);
                    await _botClient.SendTextMessageAsync(ChatId, "Выберите язык:", ParseMode.Markdown, replyMarkup: _languageText.GetLanguage(Country.UZB));
                    break;
                case "/start":
                    await _sessionService.UZUpdate(session, UZOperType.languageSet);
                    await _botClient.SendTextMessageAsync(ChatId, "Выберите язык:", ParseMode.Markdown, replyMarkup: _languageText.GetLanguage(Country.UZB));
                    break;
                default:
                    if (update.Message.Contact != null)
                    {
                        var uzPhone = update.Message.Contact.PhoneNumber.Replace("+", "");
                        var check = await _service.UZCheckRegistration(uzPhone);
                        if (check.status == 200)
                        {
                            if (check.success)
                            {
                                await _botClient.SendTextMessageAsync(ChatId, "Вы уже регистрированы", replyMarkup: new ReplyKeyboardRemove());
                            }
                            else
                            {
                                registration = await _regService.UZGetOrCreate(ChatId);
                                await _regService.UZUpdate(registration, ChatId, phone: uzPhone);
                                var regions = await _service.GetRegions(3);
                                if (regions.status == 200)
                                {
                                    // await _regService.UZUpdate(registration, ChatId, region_id: int.Parse(text));
                                    List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();
                                    regions.Items.ForEach(rr =>
                                    {
                                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton(rr.name) { Text = rr.name, CallbackData = rr.id.ToString() } });
                                    });
                                    var regs = new InlineKeyboardMarkup(Buttons);
                                    await _sessionService.UZUpdate(session, UZOperType.region);
                                    await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Укажите регион:" : "Hududni belgilang:", replyMarkup: regs);
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, regions.message);
                                }
                            }
                        }
                        else
                            await _botClient.SendTextMessageAsync(ChatId, check.data.FirstOrDefault().message, replyMarkup: new ReplyKeyboardRemove());
                    }
                    else
                    {
                        registration = await _regService.UZGetOrCreate(ChatId);
                        switch (session.Type)
                        {
                            case UZOperType.languageSet:
                                var lan = text == "Русский" ? Language.Rus : Language.UZB;
                                await _botClient.SendTextMessageAsync(ChatId, text == "Русский" ? "Что умеет этот бот? Регистрируйтесь и выигрывайте призы!" : "Bu bot nima qila oladi ? Ro‘yxatdan o‘ting va sovrinlarni yutib oling!");
                                await _sessionService.UZUpdate(session, UZOperType.menu, language: lan);
                                await _botClient.SendTextMessageAsync(ChatId, text == "Русский" ? "Главное меню" : "Asosiy menyu", replyMarkup: _languageText.GetUZKeyboard(UZOperType.menu, lan));
                                break;
                            case UZOperType.language:
                                var lanCh = text == "Русский" ? Language.Rus : Language.UZB;
                                await _sessionService.UZUpdate(session, UZOperType.start, language: lanCh);
                                resp.AppendLine(await _languageText.GetTextFromUZ(UZOperType.language, lanCh));
                                await _botClient.SendTextMessageAsync(ChatId, resp.ToString());
                                break;
                            case UZOperType.phone:
                                var regions = await _service.GetRegions(3);
                                if (regions.status == 200)
                                {
                                    await _regService.UZUpdate(registration, ChatId, region_id: int.Parse(text));
                                    List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();
                                    regions.Items.ForEach(rr =>
                                    {
                                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton(rr.name) { Text = rr.name, CallbackData = rr.id.ToString() } });
                                    });
                                    var regs = new InlineKeyboardMarkup(Buttons);
                                    await _sessionService.UZUpdate(session, UZOperType.region);
                                    await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Укажите регион:" : "Hududni belgilang:", replyMarkup: regs);
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, regions.message);
                                }
                                break;
                            case UZOperType.name:
                                await _regService.UZUpdate(registration, ChatId, name: text);
                                await _sessionService.UZUpdate(session, UZOperType.surname);
                                await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Укажите фамилию:" : "Iltimos, familiyangizni kiriting:", replyMarkup: new ReplyKeyboardRemove());
                                break;
                            case UZOperType.surname:
                                await _regService.UZUpdate(registration, ChatId, surname: text);
                                await _sessionService.UZUpdate(session, UZOperType.middle_name);
                                var skip = new InlineKeyboardMarkup(new[]
                                         {
                                                new[]{ new InlineKeyboardButton(session.language== Language.Rus? "Пропустить": "Oʻtkazib yuborish")
                                                { Text = session.language == Language.Rus ? "Пропустить" : "Oʻtkazib yuborish", CallbackData = "Skip" } }
                                            });
                                await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Укажите отчество(При наличий):" : "Iltimos, otasining ismini kiriting(agar mavjud bo'lsa):", ParseMode.Markdown, replyMarkup: skip);
                                break;
                            case UZOperType.middle_name:
                                await _regService.UZUpdate(registration, ChatId, middle_name: text);
                                resp.AppendLine(await _languageText.GetTextFromUZ(UZOperType.middle_name, session.language));
                                await _sessionService.UZUpdate(session, UZOperType.birthdate);
                                await _botClient.SendTextMessageAsync(ChatId, resp.ToString());
                                break;
                            case UZOperType.birthdate:
                                try
                                {
                                    await _regService.UZUpdate(registration, ChatId, birthdate: text);
                                    await _botClient.SendTextMessageAsync(ChatId, "что должно произойти при регистрации? ждем ответа от Бекайдара и Марата");
                                }
                                catch
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, "Не удалось конвертировать дату, введите в формате ДД.ММ.ГГГГ (пример 25.01.1991):", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                }
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
