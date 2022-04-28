using RGBTelegram.Entities;
using RGBTelegram.Services;
using RGBTelegram.vpluse;
using System;
using System.Collections.Generic;
using System.IO;
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
            var mainMenu = _languageText.GetKeyboard(session);

            StringBuilder resp = new StringBuilder();
            var ChatId = update.Message.Chat.Id;
            switch (text)
            {
                #region Об акции
                case "Promotion"://"Об акции"
                case "Науқан туралы":
                case "Акция жөнүндө":
                case "Об Акции":
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
                        try
                        {
                            var about = await _service.About(session.country == Country.KAZ ? 1 : 2, session.language);
                            if (about.success)
                            {
                                await _botClient.SendTextMessageAsync(ChatId, about.message, ParseMode.Markdown, replyMarkup: mainMenu);
                                await _sessionService.Update(session, OperationType.Promotion);
                            }
                            else
                            {
                                await _botClient.SendTextMessageAsync(ChatId, about.message, parseMode: ParseMode.Markdown, replyMarkup: mainMenu);
                            }
                        }
                        catch (Exception err)
                        {
                            await _botClient.SendTextMessageAsync(ChatId, err.Message, parseMode: ParseMode.Markdown, replyMarkup: mainMenu);
                        }
                    }
                    break;
                #endregion
                #region Авторизация
                case "Authorization":
                case "Авторизация":
                    await _sessionService.Update(session, OperationType.auth);
                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regTelNumber, session.language),
                        ParseMode.Markdown, replyMarkup: _languageText.GetPhoneMarkup(session.language));
                    break;
                #endregion
                #region Регистрация
                case "Тіркеу":
                case "Каттоо":
                case "Регистрация":
                case "Registration":
                    await _sessionService.Update(session, OperationType.regTelNumber);
                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regTelNumber, session.language),
                        ParseMode.Markdown, replyMarkup: _languageText.GetPhoneMarkup(session.language));
                    break;
                case "Қазақстан":
                case "Кыргызстан":
                    if (text == "Қазақстан")
                    {
                        resp.AppendLine("Тілді таңдаңыз.");
                        resp.AppendLine("Выберите язык.");
                    }
                    else
                    {
                        resp.AppendLine("Тилди тандаңыз.");
                        resp.AppendLine("Выберите язык.");
                    }
                    var language = _languageText.GetLanguage(text == "Қазақстан" ? Country.KAZ : Country.KGZ);
                    await _sessionService.Update(session, OperationType.country, country: text == "Қазақстан" ? Country.KAZ : Country.KGZ);
                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: language);
                    break;
                #endregion
                #region Ввести код
                case "Promocode"://Ввести код
                case "Активировать код":
                case "Кодты іске қосу":
                case "Кодду активдештирүү":
                    await _botClient.SendTextMessageAsync(ChatId, "Промокод:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                    await _sessionService.Update(session, OperationType.Promocode);
                    break;
                #endregion
                #region Правила акции
                case "ProRule": //"Правила акции"
                case "Науқан ережелері":
                case "Правила акции":
                case "Акциянын эрежелери":
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
                        await _botClient.SendTextMessageAsync(ChatId, terms.message);

                    break;
                #endregion
                #region Телефон горячей линии
                case "CallCenter"://"Телефон горячей линии"
                case "Телефон горячей линии":
                case "Тынымсыз байланыш телефону":
                case "Жедел желі телефоны":
                    await _botClient.SendTextMessageAsync(ChatId, _languageText.GetCallCenter(session.country, session.language), ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.menu);
                    break;
                #endregion
                #region "Список победителей"
                case "WinnerList"://"Список победителей"
                case "Жеңүүчүлөрдүн тизмеги":
                case "Список победителей":
                case "Жеңімпаздардың тізімі":
                    await _botClient.SendTextMessageAsync(ChatId, _languageText.GetWinnerList(session.country, session.language), ParseMode.Markdown, replyMarkup: mainMenu);
                    await _sessionService.Update(session, OperationType.menu);
                    break;
                #endregion
                #region История промокодов
                case "History"://История промокодов
                case "Промокодтар тарихы":
                case "История промокодов":
                case "Промокоддордун тарыхы":
                    if (session.expire.HasValue)
                    {
                        var expire = await _service.TokenExpire(session.expire.Value);
                        if (!expire)
                        {
                            var auth = await _authService.GetOrCreate(ChatId);
                            var data = await _service.AuthByPassword(auth);
                            if (data.success)
                            {
                                await _sessionService.Update(session, OperationType.menu, token: data.data.FirstOrDefault().message, expire: double.Parse(data.data.FirstOrDefault().field));
                            }
                            else
                            {
                                if (data.status == 422)
                                    if (data.data.FirstOrDefault().message == "Неверный логин или пароль")
                                    {
                                        await _sessionService.Update(session, OperationType.start, authorised: false);
                                        await _botClient.SendTextMessageAsync(ChatId, "Вам необходимо пройти авторизацию заново!", replyMarkup: mainMenu);
                                    }
                                    else
                                        await _botClient.SendTextMessageAsync(ChatId, data.data.FirstOrDefault().message, replyMarkup: mainMenu);
                                else
                                    await _botClient.SendTextMessageAsync(ChatId, data.data.FirstOrDefault().message, replyMarkup: mainMenu);
                                return;
                            }
                        }
                    }
                    else
                    {
                        var auth = await _authService.GetOrCreate(ChatId);
                        var data = await _service.AuthByPassword(auth);
                        if (data.success)
                        {
                            await _sessionService.Update(session, OperationType.menu, token: data.data.FirstOrDefault().message, expire: double.Parse(data.data.FirstOrDefault().field));
                        }
                        else
                        {
                            if (data.status == 422)
                                if (data.data.FirstOrDefault().message == "Неверный логин или пароль")
                                {
                                    await _sessionService.Update(session, OperationType.start, authorised: false);
                                    await _botClient.SendTextMessageAsync(ChatId, "Вам необходимо пройти авторизацию заново!", replyMarkup: mainMenu);
                                }
                                else
                                    await _botClient.SendTextMessageAsync(ChatId, data.data.FirstOrDefault().message, replyMarkup: mainMenu);
                            else
                                await _botClient.SendTextMessageAsync(ChatId, data.data.FirstOrDefault().message, replyMarkup: mainMenu);
                            return;
                        }
                    }
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
                            await _botClient.SendTextMessageAsync(ChatId, "Произошла ошибка, пожалуйста, повторите попытку ещё раз!", ParseMode.Markdown, replyMarkup: mainMenu);
                            break;
                    }
                    break;
                #endregion
                #region Смена языка
                case "/language":
                    if (!session.Authorized)
                    {
                        await _botClient.SendTextMessageAsync(ChatId, "Вам необходимо пройти авторизацию заново!", replyMarkup: mainMenu);
                    }
                    else
                    {
                        await _sessionService.Update(session, OperationType.languageChange);
                        await _botClient.SendTextMessageAsync(ChatId, "Выберите язык:", ParseMode.Markdown, replyMarkup: _languageText.GetLanguage(session.country));
                    }
                    break;
                #endregion
                #region Menu
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
                #endregion
                #region Start
                case "/start":
                    #region Start
                    if (session.Authorized)
                    {
                        await _sessionService.Update(session, OperationType.menu);
                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.menu, session.language),
                            ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, session.Authorized));
                    }
                    else
                    {
                        const string screen = "C:/nauryz.jpeg";
                        resp.AppendLine("Тіркеліңіз, қақпақ астындағы кодтарды белсендіріңіз және сыйлықтар ұтып алу мүмкіндігіне ие болыңыз!");
                        resp.AppendLine("Регистрируйтесь, активируйте коды из под крышек и получите шанс выиграть призы!");
                        resp.AppendLine();
                        resp.AppendLine("Елді таңдаңыз:");
                        resp.AppendLine("Выберите страну:");

                        List<List<KeyboardButton>> countries = new List<List<KeyboardButton>>();
                        countries.Add(new List<KeyboardButton>() { new KeyboardButton("Қазақстан") { Text = "Қазақстан" } });//KAZ
                        countries.Add(new List<KeyboardButton>() { new KeyboardButton("Кыргызстан") { Text = "Кыргызстан" } });//KGZ
                        var country = new ReplyKeyboardMarkup(countries);

                        using (var fileStream = new FileStream(screen, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            await _botClient.SendPhotoAsync(
                                chatId: ChatId,
                                photo: new InputOnlineFile(fileStream),
                                caption: resp.ToString(),
                                replyMarkup: country
                            );
                        }
                        //await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: country);
                        await _sessionService.Update(session, OperationType.start);
                    }
                    #endregion
                    break;
                #endregion
                #region Default
                default:
                    if (update.Message.Contact != null)
                    {
                        if (session.Type == OperationType.auth)
                        {
                            await _authService.GetOrCreate(ChatId, update.Message.Contact.PhoneNumber.Replace("+", ""));
                            await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.auth, session.language), ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
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
                                        var datareg1 = await _authService.GetOrCreate(ChatId);
                                        datareg1.phone = update.Message.Contact.PhoneNumber.Replace("+", "");
                                        await _authService.Update(datareg1, phone: update.Message.Contact.PhoneNumber.Replace("+", ""));

                                        await _regService.GetOrCreate(ChatId, update.Message.Contact.PhoneNumber.Replace("+", ""));
                                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regPass, session.language), ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                        await _sessionService.Update(session, OperationType.regPass);
                                    }
                                    else
                                    {
                                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regTelNumber1, session.language), ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                        await _botClient.SendTextMessageAsync(ChatId, "Меню:", ParseMode.Markdown, replyMarkup: mainMenu);
                                    }
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, call.data.FirstOrDefault().message, replyMarkup: mainMenu);
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (session.Type)
                        {
                            case OperationType.auth:
                            case OperationType.regTelNumber:
                                await _botClient.SendTextMessageAsync(ChatId, "Нельзя отправить номер телефона в виде текста. Пожалуйста, воспользуйтесь с кнопкой!", replyMarkup: mainMenu);
                                break;
                            case OperationType.country:
                                var lan = text == "Қазақ тілі" ? Language.KAZ : (text == "Кыргызский" ? Language.KGZ : Language.Rus);
                                await _sessionService.Update(session, OperationType.language, language: lan);
                                resp.AppendLine(await _languageText.GetTextFromLanguage(OperationType.country, lan));
                                await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: _languageText.GetKeyboard(session));
                                break;
                            case OperationType.Promocode:
                                #region Активировать промокод
                                var auth = await _authService.GetOrCreate(ChatId);
                                if (session.expire.HasValue)
                                {
                                    var expirePromo = await _service.TokenExpire(session.expire.Value);
                                    if (!expirePromo)
                                    {
                                        var dataPromo = await _service.AuthByPassword(auth);
                                        if (dataPromo.success)
                                        {
                                            await _sessionService.Update(session, OperationType.menu, token: dataPromo.data.FirstOrDefault().message, expire: double.Parse(dataPromo.data.FirstOrDefault().field));
                                        }
                                        else
                                        {
                                            if (dataPromo.status == 422)
                                                if (dataPromo.data.FirstOrDefault().message == "Неверный логин или пароль")
                                                {
                                                    await _sessionService.Update(session, OperationType.start, authorised: false);
                                                    await _botClient.SendTextMessageAsync(ChatId, "Вам необходимо пройти авторизацию заново!", replyMarkup: mainMenu);
                                                }
                                                else
                                                    await _botClient.SendTextMessageAsync(ChatId, "Произошла ощибка, повторите попытку ещё раз", replyMarkup: mainMenu);
                                            else
                                                await _botClient.SendTextMessageAsync(ChatId, "Произошла ощибка, повторите попытку ещё раз", replyMarkup: mainMenu);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    var dataPromo = await _service.AuthByPassword(auth);
                                    if (dataPromo.success)
                                    {
                                        await _sessionService.Update(session, OperationType.menu, token: dataPromo.data.FirstOrDefault().message, expire: double.Parse(dataPromo.data.FirstOrDefault().field));
                                    }
                                    else
                                    {
                                        if (dataPromo.status == 422)
                                            if (dataPromo.data.FirstOrDefault().message == "Неверный логин или пароль")
                                            {
                                                await _sessionService.Update(session, OperationType.start, authorised: false);
                                                await _botClient.SendTextMessageAsync(ChatId, "Вам необходимо пройти авторизацию заново!", replyMarkup: mainMenu);
                                            }
                                            else
                                                await _botClient.SendTextMessageAsync(ChatId, "Произошла ощибка, повторите попытку ещё раз", replyMarkup: mainMenu);
                                        else
                                            await _botClient.SendTextMessageAsync(ChatId, "Произошла ощибка, повторите попытку ещё раз", replyMarkup: mainMenu);
                                        return;
                                    }
                                }
                                PromoCode promo = new PromoCode();
                                promo.channel = 17;
                                promo.code = text;
                                promo.phone = auth.phone;
                                var promoResult = await _service.PromocodeActivation(promo, session.Token, session.language);
                                if (promoResult.success)
                                {
                                    resp.AppendLine(promoResult.message);
                                    if (promoResult.messages.Count > 0)
                                    {
                                        promoResult.messages.ForEach(mess =>
                                        {
                                            resp.AppendLine(session.language == Language.KAZ ? mess.kz :
                                                (session.language == Language.KGZ ? mess.kg : mess.ru));
                                        });
                                    }
                                    if (promoResult.gifts.Count > 0)
                                    {
                                        promoResult.gifts.ForEach(gift =>
                                        {
                                            resp.AppendLine(session.language == Language.KAZ ? gift.namekz :
                                                (session.language == Language.KGZ ? gift.namekg : gift.nameru));
                                        });
                                    }
                                    if (promoResult.attempts.Count > 0)
                                    {
                                        resp.AppendLine();
                                        resp.AppendLine("Промокод:");
                                        promoResult.attempts.ForEach(att =>
                                        {
                                            resp.AppendLine(att.promocode);
                                        });
                                    }
                                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: mainMenu);
                                    await _sessionService.Update(session, OperationType.Promotion);
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, promoResult.message, ParseMode.Markdown, replyMarkup: mainMenu);
                                }
                                #endregion
                                break;
                            case OperationType.birth_day:
                                try
                                {
                                    registration = await _regService.GetOrCreate(ChatId);
                                    await _regService.Update(registration, ChatId, birth_day: text);
                                    await _sessionService.Update(session, OperationType.gender);
                                    var sex = new InlineKeyboardMarkup(new[]
                                          {
                                                new[]{ new InlineKeyboardButton(session.language== Language.Rus? "Мужчина": "Эркек")
                                                { Text = session.language == Language.Rus ? "Мужчина" : "Эркек", CallbackData = "1" } },
                                                new[]{ new InlineKeyboardButton(session.language== Language.Rus? "Женщина": "Аял")
                                                { Text = session.language == Language.Rus ? "Женщина" : "Аял", CallbackData = "2" } }
                                            });
                                    await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Пол:" : "Жынысы:", ParseMode.Markdown, replyMarkup: sex);

                                }
                                catch
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, "Не удалось конвертировать дату, введите в формате ДД.ММ.ГГГГ (пример 25.01.1991):", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                }
                                break;
                            case OperationType.first_name:
                                registration = await _regService.GetOrCreate(ChatId);
                                await _regService.Update(registration, ChatId, first_name: text);
                                await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Имя:" : "Аты:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                await _sessionService.Update(session, OperationType.last_name);
                                break;
                            case OperationType.last_name:
                                registration = await _regService.GetOrCreate(ChatId);
                                await _regService.Update(registration, ChatId, last_name: text);
                                var skip = new InlineKeyboardMarkup(new[]
                                          {
                                                new[]{ new InlineKeyboardButton(session.language== Language.Rus? "Пропустить": "Өткөрүп жибер")
                                                { Text = session.language == Language.Rus ? "Пропустить" : "Өткөрүп жибер", CallbackData = "Skip" } }
                                            });
                                await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.last_name, session.language), ParseMode.Markdown, replyMarkup: skip);
                                await _sessionService.Update(session, OperationType.middlename);
                                break;
                            case OperationType.middlename:
                                registration = await _regService.GetOrCreate(ChatId);
                                await _regService.Update(registration, ChatId, middlename: text);
                                await _sessionService.Update(session, OperationType.birth_day);
                                await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.birth_day, session.language), ParseMode.Markdown);
                                break;
                            case OperationType.telNumber:
                                #region Keyboards
                                var data = await _authService.GetOrCreate(ChatId);
                                data.password = text;
                                await _authService.Update(data, passwod: text);
                                var call = await _service.AuthByPassword(data);
                                if (call.success)
                                {
                                    await _sessionService.Update(session, OperationType.menu, token: call.data.FirstOrDefault().message, authorised: true,
                                        expire: double.Parse(call.data.FirstOrDefault().field));
                                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.menu, session.language),
                                        ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, true));
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, call.data.FirstOrDefault().message + ". Попробуйте заново ввести пароль", ParseMode.Markdown);
                                }
                                #endregion
                                break;
                            //case OperationType.regTelNumber:
                            //    await _sessionService.Update(session, OperationType.regPass);
                            //    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regPass, session.language), ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                            //    break;
                            case OperationType.regPass:
                                var regions = await _service.GetRegions(((int)session.country));
                                if (regions.status == 200)
                                {
                                    var datareg = await _authService.GetOrCreate(ChatId);
                                    datareg.password = text;
                                    await _authService.Update(datareg, passwod: text);

                                    registration = await _regService.GetOrCreate(ChatId);
                                    await _regService.Update(registration, ChatId, password: text);
                                    List<List<InlineKeyboardButton>> Buttons1 = new List<List<InlineKeyboardButton>>();
                                    regions.Items.ForEach(rr =>
                                    {
                                        Buttons1.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton(rr.name) { Text = rr.name, CallbackData = rr.id.ToString() } });
                                    });
                                    var regs = new InlineKeyboardMarkup(Buttons1);
                                    await _sessionService.Update(session, OperationType.regregion);
                                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regregion, session.language), replyMarkup: regs);
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, regions.message, replyMarkup: mainMenu);
                                }
                                break;
                            case OperationType.regIIN:
                                var correctIIN = await _service.CorrectIIN(text);
                                if (correctIIN)
                                {
                                    registration = await _regService.GetOrCreate(ChatId);
                                    await _regService.Update(registration, ChatId, iin: text);
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
                                }
                                else
                                {
                                    var iin = session.language == Language.KAZ ? "Сіз қате ЖСН енгіздіңіз. Дұрыс ЖСН енгізіңіз немесе Негізгі бетке оралыңыз" : "Вы ввели некорректный ИИН. Введите корректный ИИН либо вернитесь на Главную страницу";
                                    await _botClient.SendTextMessageAsync(ChatId, iin, ParseMode.Markdown, replyMarkup: mainMenu);
                                }
                                break;
                            case OperationType.regSMSConfirm:
                                registration = await _regService.GetOrCreate(ChatId);
                                var confirmed = await _service.SignUpConfirm(registration.phone, text);
                                if (confirmed.success)
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regSMSConfirm, session.language),
                                                                            ParseMode.Markdown, replyMarkup: _languageText.GetMainMenu(session.language, true));
                                    await _sessionService.Update(session, OperationType.menu, authorised: true);
                                }
                                else
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, confirmed.data.First().message + ". " + confirmed.data.First().field, ParseMode.Markdown, replyMarkup: mainMenu);
                                }
                                break;
                            case OperationType.languageChange:
                                var lanCh = text == "Қазақ тілі" ? Language.KAZ : (text == "Кыргызский" ? Language.KGZ : Language.Rus);
                                await _sessionService.Update(session, OperationType.menu, language: lanCh);
                                resp.AppendLine(await _languageText.GetTextFromLanguage(OperationType.languageChange, lanCh));
                                await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: _languageText.GetKeyboard(session));
                                break;
                            default:
                                await _botClient.SendTextMessageAsync(ChatId, "Извините, я вас не понимаю. Выберите действию доступную вам!", ParseMode.Markdown, replyMarkup: mainMenu);
                                break;
                        }
                    }
                    break;
                #endregion
            }

            //await _botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, message, ParseMode.Markdown);
        }


    }
}
