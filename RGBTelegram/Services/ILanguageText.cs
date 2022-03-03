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
        Task<string> GetTextFromUZ(UZOperType operation, Language language);
        Task<string> GetTextFromLanguage(OperationType operation, Language language);
        ReplyKeyboardMarkup GetKeyboard(UserSession session);
        ReplyKeyboardMarkup GetLanguage(Country country);
        ReplyKeyboardMarkup GetMainMenu(Language language, bool Authorized);
        ReplyKeyboardMarkup GetPhoneMarkup(Language language);
        string GetCallCenter(Country country, Language language);
        string GetWinnerList(Country country, Language language);
    }
}
