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
    public class PepsiMessageCommands : PepsiBaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly ISessionService _sessionService;
        private readonly IAuthService _authService;
        private readonly IServiceCall _service;
        private readonly IRegService _regService;
        private readonly IPepsiText _languageText;
        private readonly IRestoreService _restoreService;
        public PepsiMessageCommands(ISessionService sessionService, IAuthService authService, IServiceCall service, IRegService regService, PepsiBot telegramBot, IPepsiText languageText, IRestoreService restoreService)
        {
            _sessionService = sessionService;
            _authService = authService;
            _service = service;
            _regService = regService;
            _botClient = telegramBot.GetPepsiBot().Result;
            _languageText = languageText;
            _restoreService = restoreService;
        }
        public override string Name => "message";
        public override async Task ExecuteAsync(Update update, PepsiSession session)
        {
            StringBuilder resp = new StringBuilder();
            var text = update.Message.Text;
            PepsiRegistration registration = new PepsiRegistration();
            var mainMenu = _languageText.GetKeyboard(session);

            var ChatId = update.Message.Chat.Id;
            try
            {
                switch (text)
                {
                    #region Об акции
                    case "Promotion"://"Об акции"
                    case "Акция туралы 🗣":
                    case "Акция жөнүндө 🗣":
                    case "Об Акции 🗣":
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
                            await _sessionService.PepsiUpdate(session, OperationType.start);
                        }
                        else
                        {
                            try
                            {
                                var about = await _service.AboutPepsi(session.country == Country.KAZ ? 1 : 2, session.language);
                                if (about.success)
                                {
                                    await _botClient.SendTextMessageAsync(ChatId, about.message, replyMarkup: mainMenu);
                                    await _sessionService.PepsiUpdate(session, OperationType.Promotion);
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
                    case "Авторландырылу":
                    case "Авторизация":
                        await _sessionService.PepsiUpdate(session, OperationType.auth);
                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regTelNumber, session.language),
                            ParseMode.Markdown, replyMarkup: _languageText.GetPhoneMarkup(session.language));
                        break;
                    #endregion
                    #region Регистрация
                    case "Тіркелу":
                    case "Катталуу":
                    case "Регистрация":
                    case "Registration":
                        await _sessionService.PepsiUpdate(session, OperationType.regTelNumber);
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
                        await _sessionService.PepsiUpdate(session, OperationType.country, country: text == "Қазақстан" ? Country.KAZ : Country.KGZ);
                        await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: language);
                        break;
                    #endregion
                    #region Ввести код
                    case "Promocode"://Ввести код
                    case "Активировать промокод 📲":
                    case "Промокодты белсенді ету 📲":
                    case "Промокодду активдештирүү 📲":
                        await _botClient.SendTextMessageAsync(ChatId, session.language== Language.Rus?"Введи промокод:":(session.language== Language.KAZ? "Промокодты енгіз:" : "Промокодду киргиз:"), 
                            ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                        await _sessionService.PepsiUpdate(session, OperationType.Promocode);
                        break;
                    #endregion
                    #region Правила акции
                    case "ProRule": //"Правила акции"
                    case "Акция ережелері 📄":
                    case "Правила Акции 📄":
                    case "Акциянын эрежелери 📄":
                        var terms = await _service.TermsPepsi(((int)session.country), session.language);
                        if (terms.success)
                        {
                            //var doc = new InputOnlineFile(new Uri(terms.Items.FirstOrDefault(x => x.id == 2).name));

                            await _botClient.SendTextMessageAsync(ChatId, terms.Items.FirstOrDefault(x => x.id == 2).name);
                          //  await _botClient.SendDocumentAsync(
                          //    chatId: ChatId,

                          //    document: new InputOnlineFile(new Uri(terms.Items.FirstOrDefault(x => x.id == 2).name)),
                          //    caption: string.IsNullOrEmpty(terms.Items.FirstOrDefault(x => x.id == 1).name) ?
                          //    "Акция" : terms.Items.FirstOrDefault(x => x.id == 1).name
                          //);
                            await _sessionService.PepsiUpdate(session, OperationType.ProRule);
                        }
                        else
                            await _botClient.SendTextMessageAsync(ChatId, terms.message);

                        break;
                    #endregion
                    #region Телефон горячей линии
                    case "CallCenter"://"Телефон горячей линии"
                    case "Телефон горячей линии 📞":
                    case "Тынымсыз байланыш телефону 📞":
                    case "Қауырт желі телефоны 📞":
                        await _botClient.SendTextMessageAsync(ChatId, _languageText.GetCallCenter(session.country, session.language), ParseMode.Markdown, replyMarkup: mainMenu);
                        await _sessionService.PepsiUpdate(session, OperationType.menu);
                        break;
                    #endregion
                    #region "Список победителей"
                    case "WinnerList"://"Список победителей"
                    case "Жеңүүчүлөрдүн тизмеги 🥇":
                    case "Список победителей 🥇":
                    case "Жеңімпаздар тізімі 🥇":
                        await _botClient.SendTextMessageAsync(ChatId, _languageText.GetWinnerListPepsi(session.country, session.language), ParseMode.Markdown, replyMarkup: mainMenu);
                        await _sessionService.PepsiUpdate(session, OperationType.menu);
                        break;
                    #endregion
                    #region История промокодов
                    case "History"://История промокодов
                    case "Промокодтар тарихы 👀":
                    case "Менің промокодтарым мен жүлделерім 👀":
                    case "Мои промокоды и призы 👀":
                    case "Менин промокоддорум жана байгелерим 👀":
                        if (session.expire.HasValue)
                        {
                            var expire = await _service.TokenExpire(session.expire.Value);
                            if (!expire)
                            {
                                var auth = await _authService.PepsiGetOrCreate(ChatId);
                                var data = await _service.AuthByPassword(auth);
                                if (data.success)
                                {
                                    await _sessionService.PepsiUpdate(session, OperationType.menu, token: data.data.FirstOrDefault().message, expire: double.Parse(data.data.FirstOrDefault().field));
                                }
                                else
                                {
                                    if (data.status == 422)
                                        if (data.data.FirstOrDefault().message == "Неверный логин или пароль")
                                        {
                                            await _sessionService.PepsiUpdate(session, OperationType.start, authorised: false);
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
                            var auth = await _authService.PepsiGetOrCreate(ChatId);
                            var data = await _service.AuthByPassword(auth);
                            if (data.success)
                            {
                                await _sessionService.PepsiUpdate(session, OperationType.menu, token: data.data.FirstOrDefault().message, expire: double.Parse(data.data.FirstOrDefault().field));
                            }
                            else
                            {
                                if (data.status == 422)
                                    if (data.data.FirstOrDefault().message == "Неверный логин или пароль")
                                    {
                                        await _sessionService.PepsiUpdate(session, OperationType.start, authorised: false);
                                        await _botClient.SendTextMessageAsync(ChatId, "Вам необходимо пройти авторизацию заново!", replyMarkup: mainMenu);
                                    }
                                    else
                                        await _botClient.SendTextMessageAsync(ChatId, data.data.FirstOrDefault().message, replyMarkup: mainMenu);
                                else
                                    await _botClient.SendTextMessageAsync(ChatId, data.data.FirstOrDefault().message, replyMarkup: mainMenu);
                                return;
                            }
                        }
                        var history = await _service.GetBundlesPepsi(session.Token);
                        switch (history.status)
                        {
                            case 200:
                                resp.AppendLine(session.language == Language.KAZ ? "Жүлделер:" : (session.language == Language.Rus ? "Призы:" : "Байгелер:"));
                                history.gifts.ForEach(gift =>
                                {
                                    resp.AppendLine("   - " + (session.language == Language.KAZ ? gift.namekz : (session.language == Language.Rus ? gift.nameru : gift.namekg)));
                                });
                                resp.AppendLine(session.language == Language.KAZ ? "Белсенді етілген промокодтар:" : (session.language == Language.Rus ? "Активированные промокоды:" : "Активдештирилген Промокоддор:"));
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
                            await _sessionService.PepsiUpdate(session, OperationType.languageChange);
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
                            await _sessionService.PepsiUpdate(session, OperationType.menu);
                        }
                        else
                        {
                            await _sessionService.PepsiUpdate(session, OperationType.menu);
                            await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.menu, session.language),
                                ParseMode.Markdown, replyMarkup: _languageText.GetMainMenuPepsi(session.language, session.Authorized));

                        }
                        break;
                    #endregion
                    #region Start
                    case "/start":
                        #region Start
                        if (session.Authorized)
                        {
                            await _sessionService.PepsiUpdate(session, OperationType.menu);
                            await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.menu, session.language),
                                ParseMode.Markdown, replyMarkup: _languageText.GetMainMenuPepsi(session.language, session.Authorized));
                        }
                        else
                        {
                            resp.AppendLine("Тіркеліңіз, қақпақ астындағы кодтарды белсендіріңіз және сыйлықтар ұтып алу мүмкіндігіне ие болыңыз!");
                            resp.AppendLine("Регистрируйтесь, активируйте коды из под крышек и получите шанс выиграть призы!");
                            resp.AppendLine();
                            resp.AppendLine("Елді таңдаңыз:");
                            resp.AppendLine("Выберите страну:");

                            List<List<KeyboardButton>> countries = new List<List<KeyboardButton>>();
                            countries.Add(new List<KeyboardButton>() { new KeyboardButton("Қазақстан") { Text = "Қазақстан" } });//KAZ
                            countries.Add(new List<KeyboardButton>() { new KeyboardButton("Кыргызстан") { Text = "Кыргызстан" } });//KGZ
                            var country = new ReplyKeyboardMarkup(countries);

                            await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), ParseMode.Markdown, replyMarkup: country);
                            await _sessionService.PepsiUpdate(session, OperationType.start);
                        }
                        #endregion
                        break;
                    #endregion
                    #region Құпия сөзді қалпына келтіру
                    case "Құпия сөзді қалпына келтіру":
                    case "Сырсөздү калыбына келтирүү":
                    case "Восстановить пароль":
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
                            await _sessionService.PepsiUpdate(session, OperationType.start);
                        }
                        else
                        {
                            await _sessionService.PepsiUpdate(session, OperationType.restore);
                            await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regTelNumber, session.language),
                                ParseMode.Markdown, replyMarkup: _languageText.GetPhoneMarkup(session.language));
                            break;
                        }
                        break;
                    #endregion
                    #region Default
                    default:
                        if (update.Message.Contact != null)
                        {
                            switch (session.Type)
                            {
                                case OperationType.auth:
                                    var checkAuth = await _service.CheckPhonePepsi(update.Message.Contact.PhoneNumber.Replace("+", ""));
                                    if (checkAuth.status == 200)
                                    {
                                        //if (checkAuth.success)
                                        //{
                                        //    await _botClient.SendTextMessageAsync(ChatId, "Вы уже зарегистрированы. Вам необходимо пройти авторизацию!", ParseMode.Markdown, replyMarkup: mainMenu);
                                        //}
                                        //else
                                        {
                                            await _authService.PepsiGetOrCreate(ChatId, update.Message.Contact.PhoneNumber.Replace("+", ""));
                                            await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.auth, session.language),
                                                ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                            await _sessionService.PepsiUpdate(session, OperationType.telNumber);
                                        }
                                    }
                                    else
                                    {
                                        await _botClient.SendTextMessageAsync(ChatId, "Произошла ошибка, повторите попытку ещё раз!", ParseMode.Markdown, replyMarkup: mainMenu);
                                    }
                                    break;
                                case OperationType.restore:
                                    var rest = await _restoreService.GetOrCreate(ChatId);
                                    await _restoreService.Update(rest, phone: update.Message.Contact.PhoneNumber.Replace("+", ""));
                                    var _sms = await _service.SendRestoreSMS(phone: update.Message.Contact.PhoneNumber.Replace("+", ""));
                                    if (_sms.success)
                                    {
                                        await _sessionService.PepsiUpdate(session, OperationType.restoreSMS);
                                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regSMS, session.language),
                                            ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                    }
                                    else
                                    {
                                        await _sessionService.PepsiUpdate(session, OperationType.menu);
                                        await _botClient.SendTextMessageAsync(ChatId, _sms.data.FirstOrDefault().message,
                                            ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                    }
                                    break;
                                default:
                                    if (session.Type == OperationType.regTelNumber)
                                    {
                                        var call = await _service.CheckPhonePepsi(update.Message.Contact.PhoneNumber.Replace("+", ""));
                                        if (call.status == 200)
                                        {
                                            if (!call.success)
                                            {
                                                var datareg1 = await _authService.PepsiGetOrCreate(ChatId);
                                                datareg1.phone = update.Message.Contact.PhoneNumber.Replace("+", "");
                                                await _authService.PepsiUpdate(datareg1, phone: update.Message.Contact.PhoneNumber.Replace("+", ""));

                                                await _regService.PepsiGetOrCreate(ChatId, update.Message.Contact.PhoneNumber.Replace("+", ""));

                                                await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regPass, session.language), ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                                await _sessionService.PepsiUpdate(session, OperationType.regPass);
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
                                    break;
                            }
                        }
                        else
                        {
                            switch (session.Type)
                            {
                                case OperationType.restoreSMS:
                                    var rest = await _restoreService.GetOrCreate(ChatId);
                                    await _restoreService.Update(rest, sms: text);
                                    await _sessionService.PepsiUpdate(session, OperationType.restoreNewPass);
                                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.restoreSMS, session.language),
                                        ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                    break;
                                case OperationType.restoreNewPass:
                                    var restsms = await _restoreService.GetOrCreate(ChatId);
                                    restsms = await _restoreService.Update(restsms, passwod: text);
                                    var reset = await _service.ResetPassword(restsms);
                                    if (reset.success)
                                    {
                                        await _sessionService.PepsiUpdate(session, OperationType.menu);
                                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.restoreNewPass, session.language),
                                            ParseMode.Markdown, replyMarkup: mainMenu);
                                    }
                                    else
                                    {
                                        await _sessionService.PepsiUpdate(session, OperationType.menu);
                                        await _botClient.SendTextMessageAsync(ChatId, reset.data.FirstOrDefault().message,
                                            ParseMode.Markdown, replyMarkup: mainMenu);
                                    }
                                    break;
                                case OperationType.auth:
                                case OperationType.regTelNumber:
                                case OperationType.restore:
                                    await _botClient.SendTextMessageAsync(ChatId, "Нельзя отправить номер телефона в виде текста. Пожалуйста, воспользуйтесь с кнопкой!", replyMarkup: mainMenu);
                                    break;
                                case OperationType.country:
                                    //if (session.Adult.HasValue && !session.Adult.Value)
                                    var lan = text == "Қазақ тілі" ? Language.KAZ : (text == "Кыргызский" ? Language.KGZ : Language.Rus);
                                    await _sessionService.PepsiUpdate(session, OperationType.adult, language: lan);
                                    resp.AppendLine(_languageText.GetAdultText(session.country, lan));
                                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: _languageText.GetAdultKeyboard(session));
                                    break;
                                case OperationType.adult:
                                    switch (text)
                                    {
                                        case "да":
                                        case "ооба":
                                        case "иә":
                                            await _sessionService.PepsiUpdate(session, OperationType.start, adult: true);
                                            resp.AppendLine(await _languageText.GetTextFromLanguage(OperationType.country, session.language));
                                            await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: _languageText.GetKeyboard(session));
                                            break;
                                        case "нет":
                                        case "жок":
                                        case "жоқ":
                                            await _sessionService.PepsiUpdate(session, OperationType.start, adult: false);
                                            resp.AppendLine(await _languageText.GetTextFromLanguage(OperationType.adultNot, session.language));
                                            await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: new ReplyKeyboardRemove());
                                            break;
                                    }
                                    break;
                                case OperationType.Promocode:
                                    #region Активировать промокод
                                    var auth = await _authService.PepsiGetOrCreate(ChatId);
                                    if (session.expire.HasValue)
                                    {
                                        var expirePromo = await _service.TokenExpire(session.expire.Value);
                                        if (!expirePromo)
                                        {
                                            var dataPromo = await _service.AuthByPassword(auth);
                                            if (dataPromo.success)
                                            {
                                                await _sessionService.PepsiUpdate(session, OperationType.menu, token: dataPromo.data.FirstOrDefault().message, expire: double.Parse(dataPromo.data.FirstOrDefault().field));
                                            }
                                            else
                                            {
                                                if (dataPromo.status == 422)
                                                    if (dataPromo.data.FirstOrDefault().message == "Неверный логин или пароль")
                                                    {
                                                        await _sessionService.PepsiUpdate(session, OperationType.start, authorised: false);
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
                                            await _sessionService.PepsiUpdate(session, OperationType.menu, token: dataPromo.data.FirstOrDefault().message, expire: double.Parse(dataPromo.data.FirstOrDefault().field));
                                        }
                                        else
                                        {
                                            if (dataPromo.status == 422)
                                                if (dataPromo.data.FirstOrDefault().message == "Неверный логин или пароль")
                                                {
                                                    await _sessionService.PepsiUpdate(session, OperationType.start, authorised: false);
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
                                    var promoResult = await _service.PromocodeActivationPepsi(promo, session.Token, session.language);
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
                                        await _sessionService.PepsiUpdate(session, OperationType.Promotion);
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
                                        registration = await _regService.PepsiGetOrCreate(ChatId);
                                        await _regService.PepsiUpdate(registration, ChatId, birth_day: text);
                                        await _sessionService.PepsiUpdate(session, OperationType.gender);
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
                                    registration = await _regService.PepsiGetOrCreate(ChatId);
                                    await _regService.PepsiUpdate(registration, ChatId, first_name: text);
                                    await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Введи свое имя:" : "Атыңды киргиз:", ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                    await _sessionService.PepsiUpdate(session, OperationType.last_name);
                                    break;
                                case OperationType.last_name:
                                    registration = await _regService.PepsiGetOrCreate(ChatId);
                                    await _regService.PepsiUpdate(registration, ChatId, last_name: text);
                                    var skip = new InlineKeyboardMarkup(new[]
                                              {
                                                new[]{ new InlineKeyboardButton(session.language== Language.Rus? "Пропустить": "Өткөрүп жибер")
                                                { Text = session.language == Language.Rus ? "Пропустить" : "Өткөрүп жибер", CallbackData = "Skip" } }
                                            });
                                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.last_name, session.language), ParseMode.Markdown, replyMarkup: skip);
                                    await _sessionService.PepsiUpdate(session, OperationType.middlename);
                                    break;
                                case OperationType.middlename:
                                    registration = await _regService.PepsiGetOrCreate(ChatId);
                                    await _regService.PepsiUpdate(registration, ChatId, middlename: text);
                                    await _sessionService.PepsiUpdate(session, OperationType.birth_day);
                                    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.birth_day, session.language), ParseMode.Markdown);
                                    break;
                                case OperationType.telNumber:
                                    #region Keyboards
                                    var data = await _authService.PepsiGetOrCreate(ChatId);
                                    data.password = text;
                                    await _authService.PepsiUpdate(data, passwod: text);
                                    var call = await _service.AuthByPassword(data);
                                    if (call.success)
                                    {
                                        await _sessionService.PepsiUpdate(session, OperationType.menu, token: call.data.FirstOrDefault().message, authorised: true,
                                            expire: double.Parse(call.data.FirstOrDefault().field));
                                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.menu, session.language),
                                            ParseMode.Markdown, replyMarkup: _languageText.GetMainMenuPepsi(session.language, true));
                                    }
                                    else
                                    {
                                        await _botClient.SendTextMessageAsync(ChatId, call.data.FirstOrDefault().message + ". Попробуйте заново ввести пароль", ParseMode.Markdown);
                                    }
                                    #endregion
                                    break;
                                //case OperationType.regTelNumber:
                                //    await _sessionService.PepsiUpdate(session, OperationType.regPass);
                                //    await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regPass, session.language), ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                                //    break;
                                case OperationType.regPass:
                                    var regions = await _service.GetRegions(((int)session.country));
                                    if (regions.status == 200)
                                    {
                                        var datareg = await _authService.PepsiGetOrCreate(ChatId);
                                        datareg.password = text;
                                        await _authService.PepsiUpdate(datareg, passwod: text);

                                        registration = await _regService.PepsiGetOrCreate(ChatId);
                                        await _regService.PepsiUpdate(registration, ChatId, password: text);
                                        List<List<InlineKeyboardButton>> Buttons1 = new List<List<InlineKeyboardButton>>();
                                        regions.Items.ForEach(rr =>
                                        {
                                            Buttons1.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton(rr.name) { Text = rr.name, CallbackData = rr.id.ToString() } });
                                        });
                                        var regs = new InlineKeyboardMarkup(Buttons1);
                                        await _sessionService.PepsiUpdate(session, OperationType.regregion);
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
                                        registration = await _regService.PepsiGetOrCreate(ChatId);
                                        await _regService.PepsiUpdate(registration, ChatId, iin: text);
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
                                    }
                                    else
                                    {
                                        var iin = session.language == Language.KAZ ? "Сіз қате ЖСН енгіздіңіз. Дұрыс ЖСН енгізіңіз немесе Негізгі бетке оралыңыз" : "Вы ввели некорректный ИИН. Введите корректный ИИН либо вернитесь на Главную страницу";
                                        await _botClient.SendTextMessageAsync(ChatId, iin, ParseMode.Markdown, replyMarkup: mainMenu);
                                    }
                                    break;
                                case OperationType.regSMSConfirm:
                                    registration = await _regService.PepsiGetOrCreate(ChatId);
                                    var confirmed = await _service.SignUpConfirmPepsi(registration.phone, text);
                                    if (confirmed.success)
                                    {
                                        await _botClient.SendTextMessageAsync(ChatId, await _languageText.GetTextFromLanguage(OperationType.regSMSConfirm, session.language),
                                                                                ParseMode.Markdown, replyMarkup: _languageText.GetMainMenuPepsi(session.language, true));
                                        await _sessionService.PepsiUpdate(session, OperationType.menu, authorised: true);
                                    }
                                    else
                                    {
                                        await _botClient.SendTextMessageAsync(ChatId, confirmed.data.First().message + ". " + confirmed.data.First().field, ParseMode.Markdown, replyMarkup: mainMenu);
                                    }
                                    break;
                                case OperationType.languageChange:
                                    var lanCh = text == "Қазақ тілі" ? Language.KAZ : (text == "Кыргызский" ? Language.KGZ : Language.Rus);
                                    await _sessionService.PepsiUpdate(session, OperationType.menu, language: lanCh);
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
            }
            catch (Exception e)
            {
                await _botClient.SendTextMessageAsync(359972761, ChatId.ToString() + " " + e.Message);
            }
        }
    }
}
