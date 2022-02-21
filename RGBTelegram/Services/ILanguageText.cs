using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RGBTelegram.Services
{
    public interface ILanguageText
    {
        Task<string> GetTextFromLanguage(OperationType operation, Language language);
        InlineKeyboardMarkup GetKeyboard(UserSession session);
        InlineKeyboardMarkup GetMainMenu(Language language, bool Authorized);
        ReplyKeyboardMarkup GetPhoneMarkup(Language language);
    }
}
