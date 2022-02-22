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
    public class MessageCommands : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly ISessionService _sessionService;
        private readonly IAuthService _authService;
        private readonly IServiceCall _service;
        private readonly IRegService _regService;
        private readonly ILanguageText _languageText;
        public MessageCommands(ISessionService sessionService, IAuthService authService, IServiceCall service, IRegService regService, TelegramBot telegramBot, ILanguageText languageText)
        {
            _sessionService = sessionService;
            _authService = authService;
            _service = service;
            _regService = regService;
            _botClient = telegramBot.GetBot().Result;
            _languageText = languageText;
        }
        public override string Name => "message";
        public override async Task ExecuteAsync(Update update, UserSession session)
        {
            var text = update.Message.Text;
            Registration registration = new Registration();
            var mainMenu = new InlineKeyboardMarkup(new[]
                                            {
                                                new[]{ new InlineKeyboardButton("Главное меню") { Text = "Главное меню", CallbackData = "Menu" } }
                                            });

            StringBuilder resp = new StringBuilder();
            var ChatId = update.Message.Chat.Id;
            switch (text)
            {
                case "/mainmenu":
                    if (session.country == 0 || session.language == 0)
                    {
                        resp.AppendLine("Әрі қарай қызмет алу үшін ел мен тілді таңдаңыз");
                        resp.AppendLine("Для дальнешего действия, выберите страну и язык");
                        resp.AppendLine("Андан аркы аракет үчүн өлкөңүздү жана тилиңизди тандаңыз");
                        var country1 = new InlineKeyboardMarkup(new[]
                                               {
                                                new[]{ new InlineKeyboardButton("Қазақстан") { Text = "Қазақстан", CallbackData = "KAZ" } },
                                                 new[]{ new InlineKeyboardButton("Кыргызстан ") { Text = "Кыргызстан ", CallbackData = "KGZ" } }
                                            });
                        await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: country1);
                        await _sessionService.Update(session, OperationType.menu);
                    }
                    else
                    {
                        await _sessionService.Update(session, OperationType.menu);
                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.menu, session.language),
                            ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, session.Authorized));

                    }
                    break;
                case "/start":
                    resp.AppendLine("Тіркеліңіз, қақпақ астындағы кодтарды белсендіріңіз және сыйлықтар ұтып алу мүмкіндігіне ие болыңыз!");
                    resp.AppendLine("Регистрируйтесь, активируйте коды из под крышек и получите шанс выиграть призы!");
                    resp.AppendLine();
                    resp.AppendLine("Елді таңдаңыз:");
                    resp.AppendLine("Выберите страну:");

                    var country = new InlineKeyboardMarkup(new[]
                                           {
                                                new[]{ new InlineKeyboardButton("Қазақстан") { Text = "Қазақстан", CallbackData = "KAZ" } },
                                                 new[]{ new InlineKeyboardButton("Кыргызстан ") { Text = "Кыргызстан ", CallbackData = "KGZ" } }
                                            });
                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: country);
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
                    await _botClient.SendTextMessageAsync(ChatId, "Отправьте свой номер телефона с помощью кнопки \"Поделиться телефоном\"", ParseMode.Markdown, replyMarkup: phone);
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
                    await _botClient.SendTextMessageAsync(ChatId, "Для регистрации отправьте номер телефона", ParseMode.Markdown, replyMarkup: ph);
                    await _sessionService.Update(session, OperationType.regTelNumber);

                    break;
                default:
                    if (update.Message.Contact != null)
                    {
                        if (session.Type == OperationType.auth)
                        {
                            await _authService.GetOrCreate(ChatId, update.Message.Contact.PhoneNumber.Replace("+", ""));
                            await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.auth,session.language), ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
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
                                        await _regService.GetOrCreate(ChatId, update.Message.Contact.PhoneNumber.Replace("+", ""));
                                        await _botClient.SendTextMessageAsync(ChatId, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                        await _sessionService.Update(session, OperationType.regPass);
                                    }
                                    else
                                    {
                                        await _botClient.SendTextMessageAsync(ChatId, "Номер зарегистрирован. Пройдите авторизацию, пожалуйста. ", ParseMode.Markdown, replyMarkup: mainMenu);
                                        //await _sessionService.Update(session, OperationType.regPass);
                                    }
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, call.data.FirstOrDefault().message, replyMarkup: new ReplyKeyboardRemove());
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (session.Type)
                        {
                            case OperationType.telNumber:
                                #region Keyboards
                                var data = await _authService.GetOrCreate(ChatId);
                                data.password = text;
                                await _authService.Update(data, passwod: text);
                                var call = await _service.AuthByPassword(data);
                                if (call.success)
                                {
                                    await _sessionService.Update(session, OperationType.menu, authorised: true);
                                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.menu,session.language),
                                        ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language,true));
                                    
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, call.data.FirstOrDefault().message + ". Попробуйте заново ввести пароль", ParseMode.Markdown);
                                }
                                #endregion
                                break;
                            case OperationType.regTelNumber:
                                await _botClient.SendTextMessageAsync(ChatId, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.regPass);
                                break;
                            case OperationType.regPass:
                                registration = await _regService.GetOrCreate(ChatId);
                                await _regService.Update(registration, ChatId, password: text);
                                var famKeyboard = new InlineKeyboardMarkup(new[]
                                            {
                                                new[]{ new InlineKeyboardButton("В браке не состою") { Text = "В браке не состою", CallbackData = "fam1" } },
                                                new[]{ new InlineKeyboardButton("Женат/замужем") { Text = "Женат/замужем", CallbackData = "fam2" } },
                                                new[]{ new InlineKeyboardButton("В разводе") { Text = "В разводе", CallbackData = "fam3" } },
                                                new[]{ new InlineKeyboardButton("Вдовец/вдова") { Text = "Вдовец/вдова", CallbackData = "fam4" } }
                                            });
                                await _botClient.SendTextMessageAsync(ChatId, "Укажите Семейный статус", ParseMode.Markdown, replyMarkup: famKeyboard);
                                await _sessionService.Update(session, OperationType.regfamily_stat);
                                break;
                            case OperationType.regIIN:
                                await _botClient.SendTextMessageAsync(ChatId, "Укажите ИИН:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.regSMS);
                                break;
                            case OperationType.regSMS:
                                var correctIIN = await _service.CorrectIIN(text);
                                if (correctIIN)
                                {
                                    registration = await _regService.GetOrCreate(ChatId);
                                    await _regService.Update(registration, ChatId, iin: text);
                                    var sign = await _service.Register(registration);
                                    if (sign.success)
                                    {
                                        await _botClient.SendTextMessageAsync(ChatId, "На ваш номер отправлен СМС код подтверждения. Пожалуйста, введите код", parseMode: ParseMode.Markdown);
                                        await _sessionService.Update(session, OperationType.regSMSConfirm);
                                    }
                                    else
                                    {
                                        await _botClient.SendTextMessageAsync(ChatId, sign.message+". "+sign.field,
                                           ParseMode.Markdown, replyMarkup: mainMenu);
                                    }
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, "Вы ввели некорректный ИИН. Введите корректный ИИН либо вернитесь на Главную страницу",
                                        ParseMode.Markdown, replyMarkup: mainMenu);
                                }
                                break;
                            case OperationType.regSMSConfirm:
                                registration = await _regService.GetOrCreate(ChatId);
                                var confirmed = await _service.SignUpConfirm(registration.phone, text);
                                if (confirmed.success)
                                {
                                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                                           {
                                                new[]{ new InlineKeyboardButton("Об акции"){Text="Об акции", CallbackData = "Promotion"}},
                                                new[]{ new InlineKeyboardButton("Ввести код"){Text = "Ввести код", CallbackData = "Promocode"} },
                                                new[]{ new InlineKeyboardButton("Правила акции"){Text = "Правила акции", CallbackData = "ProRule"}},
                                                new[]{ new InlineKeyboardButton("Мои промокоды и призы"){Text = "Мои промокоды и призы", CallbackData = "MyPromocodes"} },
                                                new[]{ new InlineKeyboardButton("Вопросы и ответы") { Text = "Вопросы и ответы", CallbackData = "Questions" } }
                                            });
                                    await _botClient.SendTextMessageAsync(ChatId, $"Вы успешно зарегистрировались. Выберите необходимую операцию!",
                                        ParseMode.Markdown, replyMarkup: inlineKeyboard);
                                    await _sessionService.Update(session, OperationType.menu, true);
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, confirmed.data.First().message + ". " + confirmed.data.First().field, ParseMode.Markdown, replyMarkup: mainMenu);
                                }
                                
                                break;
                            //case OperationType.reggender:
                            //    await _botClient.SendTextMessageAsync(ChatId, "Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                            //    await _sessionService.Update(session, OperationType.regfamily_stat);
                            //    break;
                            default:
                                await _botClient.SendTextMessageAsync(ChatId, "Извините, я вас не понимаю. Выберите действию доступную вам!", ParseMode.Markdown, replyMarkup: mainMenu);
                                break;
                        }
                    }
                    break;
            }

            //await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, message, ParseMode.Markdown);
        }

     
    }
}
