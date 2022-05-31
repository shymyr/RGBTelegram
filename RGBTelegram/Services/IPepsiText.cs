using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace RGBTelegram.Services
{
    public interface IPepsiText
    {
        Task<string> GetTextFromLanguage(OperationType operation, Language language);
        ReplyKeyboardMarkup GetKeyboard(PepsiSession session);
        ReplyKeyboardMarkup GetAdultKeyboard(PepsiSession session);
        ReplyKeyboardMarkup GetLanguage(Country country);
        ReplyKeyboardMarkup GetMainMenu(Language language, bool Authorized);
        ReplyKeyboardMarkup GetMainMenuPepsi(Language language, bool Authorized);
        ReplyKeyboardMarkup GetPhoneMarkup(Language language);
        string GetCallCenter(Country country, Language language);
        string GetAdultText(Country country, Language language);
        string GetWinnerListPepsi(Country country, Language language);
    }
}
