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
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Авторизация") {Text= "Авторизация", CallbackData = "Authorization"}});
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton("Тіркеу") {Text= "Тіркеу", CallbackData = "Registration"}});
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
            }

            return  new InlineKeyboardMarkup(Buttons); ;
        }
    }
}
