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
        public async Task<string> GetTextFromLanguage(OperationType operation, Language language)
        {
            StringBuilder result = new StringBuilder();
            switch (operation)
            {
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
                case OperationType.regTelNumber:
                    switch (language)
                    {
                        case Language.KAZ:
                            result.AppendLine(@"«Телефонмен бөлісу» батырмасы арқылы маған өз нөміріңізді жіберіңіз");
                            break;
                        case Language.Rus:
                            result.AppendLine(@"Отправьте мнесвой номер телефона с помощью кнопки «Поделиться телефоном»");
                            break;
                        case Language.KGZ:
                            result.AppendLine("\"Телефонду бөлүшүү\" баскычын колдонуп, телефон номериңизди мага жөнөтүңүз");
                            break;
                    }
                    break;
            }
            return result.ToString();
        }

        public InlineKeyboardMarkup GetKeyboard(UserSession session)
        {
            List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();
            switch (session.Type)
            {
                case OperationType.language:
                    #region Language
                    switch (session.language)
                    {
                        case Language.KAZ:
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Науқан туралы") { Text = "Науқан туралы", CallbackData = "Promotion" } });
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Тіркеу") { Text = "Тіркеу", CallbackData = "Registration" } });
                            break;
                        case Language.KGZ:
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө", CallbackData = "Promotion" } });
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Каттоо") { Text = "Каттоо", CallbackData = "Registration" } });
                            break;
                        case Language.Rus:
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Об Акции") { Text = "Об Акции", CallbackData = "Promotion" } });
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Регистрация") { Text = "Регистрация", CallbackData = "Registration" } });
                            break;
                    }
                    #endregion
                    break;
                case OperationType.menu:
                    #region MainMenu
                    if (session.Authorized)
                    {
                        switch (session.language)
                        {
                            case Language.KAZ:
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Науқан туралы") { Text = "Науқан туралы", CallbackData = "Promotion" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Кодты іске қосу") { Text = "Кодты іске қосу", CallbackData = "Promocode" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Науқан ережелері") { Text = "Науқан ережелері", CallbackData = "ProRule" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Жеңімпаздардың тізімі") { Text = "Жеңімпаздардың тізімі", CallbackData = "WinnerList" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Жедел желі телефоны") { Text = "Жедел желі телефоны", CallbackData = "CallCenter" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Промокодтар тарихы") { Text = "Промокодтар тарихы", CallbackData = "History" } });
                                break;
                            case Language.Rus:
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Об акции") { Text = "Об акции", CallbackData = "Promotion" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Активировать код") { Text = "Активировать код", CallbackData = "Promocode" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Правила акции") { Text = "Правила акции", CallbackData = "ProRule" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Список победителей") { Text = "Список победителей", CallbackData = "WinnerList" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Телефон горячей линии") { Text = "Телефон горячей линии", CallbackData = "CallCenter" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("История промокодов") { Text = "История промокодов", CallbackData = "History" } });
                                break;
                            case Language.KGZ:
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө", CallbackData = "Promotion" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Кодду активдештирүү") { Text = "Активировать код", CallbackData = "Promocode" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Акциянын эрежелери") { Text = "Акциянын эрежелери", CallbackData = "ProRule" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Жеңүүчүлөрдүн тизмеги") { Text = "Жеңүүчүлөрдүн тизмеги", CallbackData = "WinnerList" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Тынымсыз байланыш телефону") { Text = "Тынымсыз байланыш телефону", CallbackData = "CallCenter" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Промокоддордун тарыхы") { Text = "Промокоддордун тарыхы", CallbackData = "History" } });
                                break;
                        }
                    }
                    else
                    {
                        switch (session.language)
                        {
                            case Language.KAZ:
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Науқан туралы") { Text = "Науқан туралы", CallbackData = "Promotion" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Тіркеу") { Text = "Тіркеу", CallbackData = "Registration" } });
                                break;
                            case Language.KGZ:
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө", CallbackData = "Promotion" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Каттоо") { Text = "Каттоо", CallbackData = "Registration" } });
                                break;
                            case Language.Rus:
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Об Акции") { Text = "Об Акции", CallbackData = "Promotion" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                                Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Регистрация") { Text = "Регистрация", CallbackData = "Registration" } });
                                break;
                        }
                    }
                    #endregion
                    break;
            }

            return new InlineKeyboardMarkup(Buttons); ;
        }

        public InlineKeyboardMarkup GetMainMenu(Language language, bool Authorized)
        {
            List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();
            #region MainMenu
            if (Authorized)
            {
                switch (language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Науқан туралы") { Text = "Науқан туралы", CallbackData = "Promotion" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Кодты іске қосу") { Text = "Кодты іске қосу", CallbackData = "Promocode" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Науқан ережелері") { Text = "Науқан ережелері", CallbackData = "ProRule" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Жеңімпаздардың тізімі") { Text = "Жеңімпаздардың тізімі", CallbackData = "WinnerList" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Жедел желі телефоны") { Text = "Жедел желі телефоны", CallbackData = "CallCenter" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Промокодтар тарихы") { Text = "Промокодтар тарихы", CallbackData = "History" } });
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Об акции") { Text = "Об акции", CallbackData = "Promotion" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Активировать код") { Text = "Активировать код", CallbackData = "Promocode" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Правила акции") { Text = "Правила акции", CallbackData = "ProRule" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Список победителей") { Text = "Список победителей", CallbackData = "WinnerList" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Телефон горячей линии") { Text = "Телефон горячей линии", CallbackData = "CallCenter" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("История промокодов") { Text = "История промокодов", CallbackData = "History" } });
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө", CallbackData = "Promotion" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Кодду активдештирүү") { Text = "Активировать код", CallbackData = "Promocode" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Акциянын эрежелери") { Text = "Акциянын эрежелери", CallbackData = "ProRule" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Жеңүүчүлөрдүн тизмеги") { Text = "Жеңүүчүлөрдүн тизмеги", CallbackData = "WinnerList" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Тынымсыз байланыш телефону") { Text = "Тынымсыз байланыш телефону", CallbackData = "CallCenter" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Промокоддордун тарыхы") { Text = "Промокоддордун тарыхы", CallbackData = "History" } });
                        break;
                }
            }
            else
            {
                switch (language)
                {
                    case Language.KAZ:
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Науқан туралы") { Text = "Науқан туралы", CallbackData = "Promotion" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Тіркеу") { Text = "Тіркеу", CallbackData = "Registration" } });
                        break;
                    case Language.KGZ:
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Акция жөнүндө") { Text = "Акция жөнүндө", CallbackData = "Promotion" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Каттоо") { Text = "Каттоо", CallbackData = "Registration" } });
                        break;
                    case Language.Rus:
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Об Акции") { Text = "Об Акции", CallbackData = "Promotion" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") { Text = "Авторизация", CallbackData = "Authorization" } });
                        Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Регистрация") { Text = "Регистрация", CallbackData = "Registration" } });
                        break;
                }
            }
            #endregion
            return new InlineKeyboardMarkup(Buttons);
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
    }
}
