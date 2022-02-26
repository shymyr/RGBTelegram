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
        Task Delete(UserSession session);
        Task Update(UserSession session, OperationType operation, string token = null, bool? authorised = null, Country? country = null, Language? language = null, double? expire = null);
        
    }
}
