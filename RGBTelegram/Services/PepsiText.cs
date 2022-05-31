using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RGBTelegram.Services
{
    public class PepsiText : IPepsiText
    {
        public async Task<string> GetTextFromLanguage(OperationType operation, Language language)
        {
            StringBuilder result = new StringBuilder();
            switch (operation)
            {
                case OperationType.adult:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Сен 18 жасқа толдың ба?");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Тебе исполнилось 14 лет? в случае, если ваш возраст с 14 до 18 лет, необходимо получить разрешение от своего родителя или опекуна, прежде чем оставлять заявку на участие в акции");
                            break;
                        case Language.KGZ:
                            result.AppendLine("14 жашка толдуңбу? Эгер жашың 14-18 жаш аралыгында болсо, акцияга катышууга өтүнмө берүүдөн мурун ата-энеңден же камкорчуңдан уруксат алышың керек");
                            break;
                    }
                    break;
                case OperationType.adultNot:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Кешірерсің, акцияға тек 18 жасқа толған тұлғалар ғана қатыстырылады.");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Извините, к участию в акции допускаются лица с 18 лет");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Кечиресиң, сен акцияга катыша албайсың");
                            break;
                    }
                    break;
                case OperationType.languageChange:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Сіз қызмет көрсету тілін сәтті ауыстырдыңыз. Қажетті операцияны таңдаңыз!");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Вы успешно сменили язык обслуживания. Выберите необходимую операцию!");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Сизге кызмат көрсөтүүнү ийгиликтүү алмаштырдыңыз. Керектүү операцияны тандаңыз!");
                            break;
                    }
                    break;
                case OperationType.regSMSConfirm:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Құттықтаймын! Енді сен тіркелдің.🎉");
                            result.AppendLine(@"Мәзірден қажетті операцияны таңда!");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Поздравляю! Теперь ты зарегистрирован.🎉");
                            result.AppendLine(@"Выбери необходимую операцию из Меню!");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Куттуктайбыз! Сен эми катталдың.🎉");
                            result.AppendLine("Менюдан керектүү операцияны танда!");
                            break;
                    }
                    break;
                case OperationType.gender:
                    switch (language)
                    {
                        case Language.Rus:
                            result.AppendLine(@"Статус пола:");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Гендердик статус:");
                            break;
                    }
                    break;
                case OperationType.birth_day:
                    switch (language)
                    {
                        case Language.Rus:
                            result.AppendLine(@"Укажите день рождения в формате ДД.ММ.ГГГГ (пример 25.01.1991):");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Туулган күнүңүздү ДД.ММ.ГГГГ форматында киргизиңиз (мисалы 25.01.1991):");
                            break;
                    }
                    break;
                case OperationType.last_name:
                    switch (language)
                    {
                        case Language.Rus:
                            result.AppendLine(@"Отчество(при наличии, если отсутствует пропустите):");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Атасынын аты(бар болсо, жок болсо, өткөрүп жибериңиз):");
                            break;
                    }
                    break;
                case OperationType.restoreSMS:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Өзің ұмытпайтындай құпиясөз ойлап тап! 🧐 Құпиясөзде кемінде алты символ болуы тиіс, ол латын алфавитінің әріптері мен цифрлардан құралуы тиіс☝");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Придумай себе такой пароль, чтобы не забыть! 🧐 Пароль должен содержать не менее шести символов, состоять из букв латинского алфавита и цифр☝️");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Өзүң унутпай кала турган сырсөз ойлоп тап! 🧐 Сырсөз кеминде алты белгиден, латын алфавитинин тамгаларынан жана сандардан турушу керек");
                            break;
                    }
                    break;
                case OperationType.restoreNewPass:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Құттықтаймыз, құпия сөзіңізді сәтті қалпына келтірдіңіз.");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Поздравляем, вы успешно восстановили пароль.");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Куттуктайбыз, сырсөзүңүздү ийгиликтүү калыбына келтирдиңиз.");
                            break;
                    }
                    break;
                case OperationType.regSMS:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Сенің нөміріңе растауға арналған СМС-код келеді. Оны мұнда енгіз:");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"На твой номер придет СМС-код для подтверждения. Введите его тут:");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Телефон номериңе тастыктоо үчүн СМС-код келет. Аны бул жерге киргиз:");
                            break;
                    }
                    break;
                case OperationType.regcity:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Тізімнен өз қалаңды таңда:");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Выбери свой город из списка:");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Тизмеден өзүңдүн шаарыңды танда:");
                            break;
                    }
                    break;
                case OperationType.regregion:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Тізімнен өз аймағыңды таңда:");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Выбери свой регион из списка:");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Тизмеден өзүңдүн аймагыңды танда:");
                            break;
                    }
                    break;
                case OperationType.regPass:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Өзің ұмытпайтындай құпиясөз ойлап тап! 🧐 Құпиясөзде кемінде алты символ болуы тиіс, ол латын алфавитінің әріптері мен цифрлардан құралуы тиіс☝");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Придумай себе такой пароль, чтобы не забыть! 🧐 Пароль должен содержать не менее шести символов, состоять из букв латинского алфавита и цифр☝️");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Өзүң унутпай кала турган сырсөз ойлоп тап! 🧐 Сырсөз кеминде алты белгиден, латын алфавитинин тамгаларынан жана сандардан турушу керек");
                            break;
                    }
                    break;
                case OperationType.menu:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"🎉Сен авторландырудан ойдағыдай өттің!");
                            result.AppendLine(@"Мәзірден операцияны таңда");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"🎉Ты успешно авторизован! Выбирай операцию из Меню");
                            break;
                        case Language.KGZ:
                            result.AppendLine("🎉Сен ийгиликтүү авторизацияландың!");
                            result.AppendLine("Менюдан операцияны танда");
                            break;
                    }
                    break;
                case OperationType.auth:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Құпиясөзді енгіз");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Введи пароль");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Сырсөздү киргиз");
                            break;
                    }
                    break;
                case OperationType.country:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Сәлем! Бастау үшін авторландырудан немесе тіркеуден өт.");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Привет! Чтобы начать авторизуйся или пройди регистрацию.");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Салам! Баштоо үчүн авторизациядан же каттоодон өт.");
                            break;
                    }
                    break;
                case OperationType.regTelNumber1:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"😲 Сен тіркеліп қойған екенсің! Авторландырудан өту ғана қалды");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"😲 Оказывается ты УЖЕ зарегистрирован! Осталось только пройти авторизацию");
                            break;
                        case Language.KGZ:
                            result.AppendLine("😲 Сен БУГА ЧЕЙИН катталган экенсиң! Авторизациядан өтүү гана калды");
                            break;
                    }
                    break;
                case OperationType.regTelNumber:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"«Телефон нөмірін бөлісу» батырмасының көмегімен телефон нөмірін жібер 📱");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Отправь свой номер телефона с помощью кнопки «Поделиться номером телефона»📱");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Өз телефон номериңди «Телефон номерди бөлүшүү» баскычын колдонуп жөнөт📱");
                            break;
                    }
                    break;
            }
            return result.ToString();
        }
        public ReplyKeyboardMarkup GetLanguage(Country country)
        {
            List<List<KeyboardButton>> Buttons = new List<List<KeyboardButton>>();
            switch (country)
            {
                case Country.KAZ:
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Қазақ тілі") { Text = "Қазақ тілі" } });
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Русский") { Text = "Русский" } });
                    break;
                case Country.KGZ:
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Русский") { Text = "Русский" } });
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Кыргызча") { Text = "Кыргызча" } });
                    break;
            }
            return new ReplyKeyboardMarkup(Buttons);
        }
        public ReplyKeyboardMarkup GetKeyboard(PepsiSession session)
        {
            List<List<KeyboardButton>> Buttons = new List<List<KeyboardButton>>();
            #region MainMenu
            if (session.Authorized)
            {
                switch (session.language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокодты белсенді ету 📲") { Text = "Промокодты белсенді ету 📲" } });//Promocode
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Менің промокодтарым мен жүлделерім 👀") { Text = "Менің промокодтарым мен жүлделерім 👀" } });//History
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция туралы 🗣") { Text = "Акция туралы 🗣" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция ережелері 📄") { Text = "Акция ережелері 📄" } });//ProRule
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңімпаздар тізімі 🥇") { Text = "Жеңімпаздар тізімі 🥇" } });//WinnerList
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Қауырт желі телефоны 📞") { Text = "Қауырт желі телефоны 📞" } });//CallCenter
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Активировать промокод 📲") { Text = "Активировать промокод 📲" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Мои промокоды и призы 👀") { Text = "Мои промокоды и призы 👀" } });//, CallbackData = "History" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции 🗣") { Text = "Об Акции 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Правила Акции 📄") { Text = "Правила Акции 📄" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Список победителей 🥇") { Text = "Список победителей 🥇" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Телефон горячей линии 📞") { Text = "Телефон горячей линии 📞" } });//, CallbackData = "CallCenter" }
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокодду активдештирүү 📲") { Text = "Промокодду активдештирүү 📲" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Менин промокоддорум жана байгелерим 👀") { Text = "Менин промокоддорум жана байгелерим 👀" } });//, CallbackData = "History" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө 🗣") { Text = "Акция жөнүндө 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акциянын эрежелери 📄") { Text = "Акциянын эрежелери 📄" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңүүчүлөрдүн тизмеги 🥇") { Text = "Жеңүүчүлөрдүн тизмеги 🥇" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тынымсыз байланыш телефону 📞") { Text = "Тынымсыз байланыш телефону 📞" } });//, CallbackData = "CallCenter" }
                        break;
                }
            }
            else
            {
                switch (session.language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция туралы 🗣") { Text = "Акция туралы 🗣" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторландырылу") { Text = "Авторландырылу" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тіркелу") { Text = "Тіркелу" } });//, CallbackData = "Registration" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Құпия сөзді қалпына келтіру") { Text = "Құпия сөзді қалпына келтіру" } });
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө 🗣") { Text = "Акция жөнүндө 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Катталуу") { Text = "Катталуу" } });//, CallbackData = "Registration" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Сырсөздү калыбына келтирүү") { Text = "Сырсөздү калыбына келтирүү" } });
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции 🗣") { Text = "Об Акции 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Регистрация") { Text = "Регистрация" } });//, CallbackData = "Registration" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Восстановить пароль") { Text = "Восстановить пароль" } });
                        break;
                }
            }

            #endregion

            return new ReplyKeyboardMarkup(Buttons);
        }
        public ReplyKeyboardMarkup GetAdultKeyboard(PepsiSession session)
        {
            List<List<KeyboardButton>> Buttons = new List<List<KeyboardButton>>();

            switch (session.language)
            {
                case Language.KAZ:
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("иә") { Text = "иә" } });
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("жоқ") { Text = "жоқ" } });
                    break;
                case Language.Rus:
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("да") { Text = "да" } });
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("нет") { Text = "нет" } });
                    break;
                case Language.KGZ:
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("ооба") { Text = "ооба" } });
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("жок") { Text = "жок" } });
                    break;
            }


            return new ReplyKeyboardMarkup(Buttons);
        }
        public ReplyKeyboardMarkup GetMainMenu(Language language, bool Authorized)
        {
            List<List<KeyboardButton>> Buttons = new List<List<KeyboardButton>>();
            #region MainMenu
            if (Authorized)
            {
                switch (language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокодты белсенді ету 📲") { Text = "Промокодты белсенді ету 📲" } });//Promocode
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Менің промокодтарым мен жүлделерім 👀") { Text = "Менің промокодтарым мен жүлделерім 👀" } });//History
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция туралы 🗣") { Text = "Акция туралы 🗣" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция ережелері 📄") { Text = "Акция ережелері 📄" } });//ProRule
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңімпаздар тізімі 🥇") { Text = "Жеңімпаздар тізімі 🥇" } });//WinnerList
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Қауырт желі телефоны 📞") { Text = "Қауырт желі телефоны 📞" } });//CallCenter
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Активировать промокод 📲") { Text = "Активировать промокод 📲" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Мои промокоды и призы 👀") { Text = "Мои промокоды и призы 👀" } });//, CallbackData = "History" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции 🗣") { Text = "Об Акции 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Правила Акции 📄") { Text = "Правила Акции 📄" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Список победителей 🥇") { Text = "Список победителей 🥇" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Телефон горячей линии 📞") { Text = "Телефон горячей линии 📞" } });//, CallbackData = "CallCenter" }
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокодду активдештирүү 📲") { Text = "Промокодду активдештирүү 📲" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Менин промокоддорум жана байгелерим 👀") { Text = "Менин промокоддорум жана байгелерим 👀" } });//, CallbackData = "History" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө 🗣") { Text = "Акция жөнүндө 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акциянын эрежелери 📄") { Text = "Акциянын эрежелери 📄" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңүүчүлөрдүн тизмеги 🥇") { Text = "Жеңүүчүлөрдүн тизмеги 🥇" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тынымсыз байланыш телефону 📞") { Text = "Тынымсыз байланыш телефону 📞" } });//, CallbackData = "CallCenter" }
                        break;
                }
            }
            else
            {
                switch (language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция туралы 🗣") { Text = "Акция туралы 🗣" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторландырылу") { Text = "Авторландырылу" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тіркелу") { Text = "Тіркелу" } });//, CallbackData = "Registration" }
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө 🗣") { Text = "Акция жөнүндө 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Катталуу") { Text = "Катталуу" } });//, CallbackData = "Registration" }
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции 🗣") { Text = "Об Акции 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Регистрация") { Text = "Регистрация" } });//, CallbackData = "Registration" }
                        break;
                }
            }
            #endregion
            return new ReplyKeyboardMarkup(Buttons);
        }
        public ReplyKeyboardMarkup GetMainMenuPepsi(Language language, bool Authorized)
        {
            List<List<KeyboardButton>> Buttons = new List<List<KeyboardButton>>();
            #region MainMenu
            if (Authorized)
            {
                switch (language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокодты белсенді ету 📲") { Text = "Промокодты белсенді ету 📲" } });//Promocode
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Менің промокодтарым мен жүлделерім 👀") { Text = "Менің промокодтарым мен жүлделерім 👀" } });//History
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция туралы 🗣") { Text = "Акция туралы 🗣" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция ережелері 📄") { Text = "Акция ережелері 📄" } });//ProRule
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңімпаздар тізімі 🥇") { Text = "Жеңімпаздар тізімі 🥇" } });//WinnerList
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Қауырт желі телефоны 📞") { Text = "Қауырт желі телефоны 📞" } });//CallCenter
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Активировать промокод 📲") { Text = "Активировать промокод 📲" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Мои промокоды и призы 👀") { Text = "Мои промокоды и призы 👀" } });//, CallbackData = "History" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции 🗣") { Text = "Об Акции 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Правила Акции 📄") { Text = "Правила Акции 📄" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Список победителей 🥇") { Text = "Список победителей 🥇" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Телефон горячей линии 📞") { Text = "Телефон горячей линии 📞" } });//, CallbackData = "CallCenter" }
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокодду активдештирүү 📲") { Text = "Промокодду активдештирүү 📲" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Менин промокоддорум жана байгелерим 👀") { Text = "Менин промокоддорум жана байгелерим 👀" } });//, CallbackData = "History" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө 🗣") { Text = "Акция жөнүндө 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акциянын эрежелери 📄") { Text = "Акциянын эрежелери 📄" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңүүчүлөрдүн тизмеги 🥇") { Text = "Жеңүүчүлөрдүн тизмеги 🥇" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тынымсыз байланыш телефону 📞") { Text = "Тынымсыз байланыш телефону 📞" } });//, CallbackData = "CallCenter" }
                        break;
                }
            }
            else
            {
                switch (language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция туралы") { Text = "Акция туралы" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тіркелу") { Text = "Тіркелу" } });//, CallbackData = "Registration" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Құпия сөзді қалпына келтіру") { Text = "Құпия сөзді қалпына келтіру" } });
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции 🗣") { Text = "Об Акции 🗣" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Катталуу") { Text = "Катталуу" } });//, CallbackData = "Registration" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Сырсөздү калыбына келтирүү") { Text = "Сырсөздү калыбына келтирүү" } });
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции") { Text = "Об Акции" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Регистрация") { Text = "Регистрация" } });//, CallbackData = "Registration" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Восстановить пароль") { Text = "Восстановить пароль" } });
                        break;
                }
            }
            #endregion
            return new ReplyKeyboardMarkup(Buttons);
        }

        public ReplyKeyboardMarkup GetPhoneMarkup(Language language)
        {
            string text = "";
            switch (language)
            {
                case Language.KGZ:
                    text = "Телефон номерди бөлүшүү";
                    break;
                case Language.Rus:
                    text = "Поделиться номером телефона";
                    break;
                case Language.KAZ:
                    text = "Телефон нөмірін бөлісу";
                    break;
            }
            return new ReplyKeyboardMarkup(new[]
                                             {
                                                    new KeyboardButton(text)
                                                     {
                                                        RequestContact = true
                                                     }
                                                });
        }
        public string GetCallCenter(Country country, Language language)
        {
            string result = "";
            switch (country)
            {
                case Country.KAZ:
                    switch (language)
                    {
                        case Language.KAZ:
                            result = "Барлық қосымша сұрақтар бойынша 3775 қысқа нөмірі арқылы хабарлас";
                            break;
                        case Language.Rus:
                            result = "По всем дополнительным вопросам обратитесь по короткому номеру 3775";
                            break;
                    }
                    break;
                case Country.KGZ:
                    switch (language)
                    {
                        case Language.KGZ:
                            result = "Бардык кошумча суроолор боюнча кыска номерге кайрыл 3775";
                            break;
                        case Language.Rus:
                            result = "По всем дополнительным вопросам обратитесь по короткому номеру 3775";
                            break;
                    }
                    break;
            }
            return result;
        }
        public string GetAdultText(Country country, Language language)
        {
            string result = "";

            switch (country)
            {
                case Country.KGZ:
                    switch (language)
                    {
                        case Language.Rus://Тебе исполнилось 18 лет?
                            result = (@"Тебе исполнилось 14 лет? в случае, если ваш возраст с 14 до 18 лет, необходимо получить разрешение от своего родителя или опекуна, прежде чем оставлять заявку на участие в акции");
                            break;
                        case Language.KGZ:
                            result = ("14 жашка толдуңбу? Эгер жашың 14-18 жаш аралыгында болсо, акцияга катышууга өтүнмө берүүдөн мурун ата-энеңден же камкорчуңдан уруксат алышың керек");
                            break;
                    }

                    break;
                case Country.KAZ:
                    switch (language)
                    {
                        case Language.KAZ:
                            result = (@"Сен 18 жасқа толдың ба?");
                            break;
                        case Language.Rus://Тебе исполнилось 18 лет?
                            result = (@"Тебе исполнилось 18 лет?");
                            break;
                    }
                    break;
            }
            return result;
        }
        public string GetWinnerListPepsi(Country country, Language language)
        {
            string result = "";
            switch (country)
            {
                case Country.KAZ:
                    switch (language)
                    {
                        case Language.KAZ:
                            result = "Жеңімпаздардың толық тізімін  біздің сайттан ізде ➡️ pepsi.kz";
                            break;
                        case Language.Rus:
                            result = "Полный список победителей ищи на нашем сайте ➡️ pepsi.kz";
                            break;
                    }
                    break;
                case Country.KGZ:
                    switch (language)
                    {
                        case Language.KGZ:
                            result = "Жеңүүчүлөрдүн толук тизмегин биздин ➡️ pepsi.kg сайттарыбызда изде";
                            break;
                        case Language.Rus:
                            result = "Полный список победителей ищи на нашем сайте ➡️ pepsi.kg";
                            break;
                    }
                    break;
            }
            return result;
        }
    }
}
