using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RGBTelegram.Services
{
    public interface ISessionService
    {
        Task<UserSession> GetOrCreate(Update update);
        Task<PepsiSession> PepsiGetOrCreate(Update update);
        Task<UZSession> UZGetOrCreate(Update update);
        Task Delete(UserSession session);
        Task PepsiDelete(PepsiSession session);
        Task UZDelete(UZSession session);
        Task Update(UserSession session, OperationType operation, string token = null, bool? authorised = null, Country? country = null, Language? language = null, double? expire = null);
        Task PepsiUpdate(PepsiSession session, OperationType operation, string token = null, bool? adult = null, bool? authorised = null, Country? country = null, Language? language = null, double? expire = null);
        Task UZUpdate(UZSession session, UZOperType operation, Language? language = null);
    }
}
