using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RGBTelegram.Services
{
    public class LanguageText : ILanguageText
    {
        public async Task<string> GetTextFromUZ(UZOperType operation, Language language)
        {
            StringBuilder result = new StringBuilder();
            switch (operation)
            {
                case UZOperType.middle_name:
                    switch (language)
                    {
                        case Language.Rus:
                            result.AppendLine("Укажите день рождения в формате ДД.ММ.ГГГГ (пример 25.01.1991):");
                            break;
                        case Language.UZB:
                            result.AppendLine("Tug'ilgan kuningizni DD.MM.YYYY formatida kiriting (misol 25.01.1991):");
                            break;
                    }
                    break;
                    break;
                case UZOperType.region:
                    switch (language)
                    {
                        case Language.Rus:
                            result.AppendLine("Укажите регион:");
                            break;
                        case Language.UZB:
                            result.AppendLine("Hududni belgilang:");
                            break;
                    }
                    break;
                case UZOperType.language:
                    switch (language)
                    {
                        case Language.Rus:
                            result.AppendLine(@"Вы успешно сменили язык обслуживания. Выберите необходимую операцию!");
                            break;
                        case Language.UZB:
                            result.AppendLine("Siz xizmat tilini muvaffaqiyatli o'zgartirdingiz. Kerakli operatsiyani tanlang!");
                            break;
                    }
                    break;
            }
            return result.ToString();
        }
        public async Task<string> GetTextFromLanguage(OperationType operation, Language language)
        {
            StringBuilder result = new StringBuilder();
            switch (operation)
            {
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
                            result.AppendLine(@"Сіз сәтті тіркелдіңіз. Қажетті операцияны таңдаңыз!");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Вы успешно зарегистрировались. Выберите необходимую операцию!");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Сиз ийгиликтүү катталдыңыз. Керектүү операцияны тандаңыз!");
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
                case OperationType.regSMS:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Сіздің нөміріңізге SMS растау коды жіберілді. Кодты енгізіңіз:");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"На ваш номер отправлен СМС код подтверждения. Пожалуйста, введите код:");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Сиздин номериңизге тастыктоо коду SMS жөнөтүлдү. Сураныч, кодду киргизиңиз:");
                            break;
                    }
                    break;
                case OperationType.regcity:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Қаланы таңдаңыз:");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Выберите город:");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Шаар тандоо:");
                            break;
                    }
                    break;
                case OperationType.regregion:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Аймақты таңдаңыз:");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Выберите регион:");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Аймакты тандоо:");
                            break;
                    }
                    break;
                case OperationType.regPass:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Құпия сөз енгізіңіз, құпия сөзде әріптер (латын тілінде) және сандар болуы керек. Кемінде 6 таңба");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Придумайте пароль, пароль должен содержать буквы(на латинице) и цифры. Минимум 6 символов");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Сырсөз түзүңүз, сырсөз тамгаларды (латынча) жана сандарды камтышы керек. Минималдуу 6 белги.");
                            break;
                    }
                    break;
                case OperationType.menu:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Төмендегі батырмалардың біреуін басыңыз");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Нажми на одну из кнопок ниже");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Төмөндөгү баскычтардын бирин басыңыз.");
                            break;
                    }
                    break;
                case OperationType.auth:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Құпиясөзді енгізіңіз");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Введите пароль");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Сырсөздү киргизиңиз");
                            break;
                    }
                    break;
                case OperationType.country:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Салем! Ең алдымен авторизациядан өту қажет");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Привет! Для начала необходимо авторизоваться");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Салам! Алгач, өздүгүңүздү ырастаңыз.");
                            break;
                    }
                    break;
                case OperationType.regTelNumber1:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"Нөмір тіркелген. Жүйеге кіріңіз.");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Номер зарегистрирован. Пройдите авторизацию, пожалуйста.");
                            break;
                        case Language.KGZ:
                            result.AppendLine("Номер катталган. Сураныч, кириңиз.");
                            break;
                    }
                    break;
                case OperationType.regTelNumber:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"«Телефонмен бөлісу» батырмасы арқылы маған өз нөміріңізді жіберіңіз");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Отправьте мне свой номер телефона с помощью кнопки «Поделиться телефоном»");
                            break;
                        case Language.KGZ:
                            result.AppendLine("\"Телефонду бөлүшүү\" баскычын колдонуп, телефон номериңизди мага жөнөтүңүз");
                            break;
                    }
                    break;
            }
            return result.ToString();
        }
        public ReplyKeyboardMarkup GetUZKeyboard(UZOperType operation, Language language)
        {
            List<List<KeyboardButton>> Buttons = new List<List<KeyboardButton>>();
            switch (operation)
            {
                case UZOperType.menu:
                    switch (language)
                    {
                        case Language.Rus:
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Регистрация") { Text = "Регистрация" } });
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции") { Text = "Об Акции" } });
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Правила акции") { Text = "Правила акции" } });
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Список ЦВП") { Text = "Список ЦВП" } });
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Вопросы и ответы") { Text = "Вопросы и ответы" } });
                            break;
                        case Language.UZB:
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Ro‘yxatdan o‘tish") { Text = "Ro‘yxatdan o‘tish" } });//Регистрация
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Aksiya haqida") { Text = "Aksiya haqida" } });
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Aksiya Qoidalari") { Text = "Aksiya Qoidalari" } });
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("SBM ro‘yxati") { Text = "SBM ro‘yxati" } });
                            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Savollar va javoblar") { Text = "Savollar va javoblar" } });
                            break;
                    }
                    break;
            }
            return new ReplyKeyboardMarkup(Buttons);
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
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Кыргызский") { Text = "Кыргызский" } });
                    break;
                default:
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Русский") { Text = "Русский" } });
                    Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("O'zbek tili") { Text = "O'zbek tili" } });
                    break;
            }
            return new ReplyKeyboardMarkup(Buttons);
        }

        public ReplyKeyboardMarkup GetKeyboard(UserSession session)
        {
            List<List<KeyboardButton>> Buttons = new List<List<KeyboardButton>>();
            //switch (session.Type)
            //{
            //case OperationType.language:
            //    #region Language
            //    switch (session.language)
            //    {
            //        case Language.KAZ:
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Науқан туралы") { Text = "Науқан туралы" } });//Promotion
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тіркеу") { Text = "Тіркеу" } });//, CallbackData = "Registration" }
            //            break;
            //        case Language.KGZ:
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө" } });//, CallbackData = "Promotion" }
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Каттоо") { Text = "Каттоо" } });//, CallbackData = "Registration" }
            //            break;
            //        case Language.Rus:
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции") { Text = "Об Акции" } });//, CallbackData = "Promotion" }
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
            //            Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Регистрация") { Text = "Регистрация" } });//, CallbackData = "Registration" }
            //            break;
            //    }
            //    #endregion
            //    break;
            //case OperationType.menu:
            #region MainMenu
            if (session.Authorized)
            {
                switch (session.language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Науқан туралы") { Text = "Науқан туралы" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Кодты іске қосу") { Text = "Кодты іске қосу" } });//Promocode
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Науқан ережелері") { Text = "Науқан ережелері" } });//ProRule
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңімпаздардың тізімі") { Text = "Жеңімпаздардың тізімі" } });//WinnerList
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жедел желі телефоны") { Text = "Жедел желі телефоны" } });//CallCenter
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокодтар тарихы") { Text = "Промокодтар тарихы" } });//History
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции") { Text = "Об Акции" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Активировать код") { Text = "Активировать код" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Правила акции") { Text = "Правила акции" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Список победителей") { Text = "Список победителей" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Телефон горячей линии") { Text = "Телефон горячей линии" } });//, CallbackData = "CallCenter" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("История промокодов") { Text = "История промокодов" } });//, CallbackData = "History" }
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Кодду активдештирүү") { Text = "Кодду активдештирүү" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акциянын эрежелери") { Text = "Акциянын эрежелери" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңүүчүлөрдүн тизмеги") { Text = "Жеңүүчүлөрдүн тизмеги" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тынымсыз байланыш телефону") { Text = "Тынымсыз байланыш телефону" } });//, CallbackData = "CallCenter" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокоддордун тарыхы") { Text = "Промокоддордун тарыхы" } });//, CallbackData = "History" }
                        break;
                }
            }
            else
            {
                switch (session.language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Науқан туралы") { Text = "Науқан туралы" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тіркеу") { Text = "Тіркеу" } });//, CallbackData = "Registration" }
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Каттоо") { Text = "Каттоо" } });//, CallbackData = "Registration" }
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции") { Text = "Об Акции" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Регистрация") { Text = "Регистрация" } });//, CallbackData = "Registration" }
                        break;
                }
            }
            #endregion
            //        break;
            //}

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
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Науқан туралы") { Text = "Науқан туралы" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Кодты іске қосу") { Text = "Кодты іске қосу" } });//Promocode
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Науқан ережелері") { Text = "Науқан ережелері" } });//ProRule
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңімпаздардың тізімі") { Text = "Жеңімпаздардың тізімі" } });//WinnerList
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жедел желі телефоны") { Text = "Жедел желі телефоны" } });//CallCenter
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокодтар тарихы") { Text = "Промокодтар тарихы" } });//History
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции") { Text = "Об Акции" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Активировать код") { Text = "Активировать код" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Правила акции") { Text = "Правила акции" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Список победителей") { Text = "Список победителей" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Телефон горячей линии") { Text = "Телефон горячей линии" } });//, CallbackData = "CallCenter" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("История промокодов") { Text = "История промокодов" } });//, CallbackData = "History" }
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Кодду активдештирүү") { Text = "Кодду активдештирүү" } });//, CallbackData = "Promocode" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акциянын эрежелери") { Text = "Акциянын эрежелери" } });//, CallbackData = "ProRule" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Жеңүүчүлөрдүн тизмеги") { Text = "Жеңүүчүлөрдүн тизмеги" } });//, CallbackData = "WinnerList" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тынымсыз байланыш телефону") { Text = "Тынымсыз байланыш телефону" } });//, CallbackData = "CallCenter" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Промокоддордун тарыхы") { Text = "Промокоддордун тарыхы" } });//, CallbackData = "History" }
                        break;
                }
            }
            else
            {
                switch (language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Науқан туралы") { Text = "Науқан туралы" } });//Promotion
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Тіркеу") { Text = "Тіркеу" } });//, CallbackData = "Registration" }
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Каттоо") { Text = "Каттоо" } });//, CallbackData = "Registration" }
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Об Акции") { Text = "Об Акции" } });//, CallbackData = "Promotion" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Авторизация") { Text = "Авторизация" } });//, CallbackData = "Authorization" }
                        Buttons.Add(new List<KeyboardButton>() { new KeyboardButton("Регистрация") { Text = "Регистрация" } });//, CallbackData = "Registration" }
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
                    text = "Телефонду бөлүшүү";
                    break;
                case Language.Rus:
                    text = "Поделиться телефоном";
                    break;
                case Language.KAZ:
                    text = "Телефонмен бөлісу";
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
                            result = "Қосымша сұрақтар бойынша 3775 қысқа нөміріне хабарласыңыз";
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
                            result = "Бардык кошумча суроолор боюнча 3775 кыска номерине кайрылыңыз";
                            break;
                        case Language.Rus:
                            result = "По всем дополнительным вопросам обратитесь по короткому номеру 3775";
                            break;
                    }
                    break;
            }
            return result;
        }

        public string GetWinnerList(Country country, Language language)
        {
            string result = "";
            switch (country)
            {
                case Country.KAZ:
                    switch (language)
                    {
                        case Language.KAZ:
                            result = "Жеңімпаздардың толық тізімімен Nauryzpromo.kz сайтында таныса аласыз";
                            break;
                        case Language.Rus:
                            result = "С полным списком победителей можете ознакомится на сайте Nauryzpromo.kz";
                            break;
                    }
                    break;
                case Country.KGZ:
                    switch (language)
                    {
                        case Language.KGZ:
                            result = "Жеңүүчүлөрдүн толук тизмеги менен Nauryzpromo.kz сайтында таанышууга болот";
                            break;
                        case Language.Rus:
                            result = "С полным списком победителей можете ознакомится на сайте Nauryzpromo.kz";
                            break;
                    }
                    break;
            }
            return result;
        }
    }
}
