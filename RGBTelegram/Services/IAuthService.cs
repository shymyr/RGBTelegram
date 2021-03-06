using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RGBTelegram.Services
{
    public interface IAuthService
    {
        Task<AuthData> GetOrCreate(long ChatId,string phone = null);
        Task<PepsiAuthData> PepsiGetOrCreate(long ChatId, string phone = null);
        Task<AuthData> Update(AuthData auth, string phone = null, string passwod = null);
        Task<PepsiAuthData> PepsiUpdate(PepsiAuthData auth, string phone = null, string passwod = null);
        Task<Token> GetOrCreateToken();
        Task<Token> UpdateToken(Token auth, string token);
    }
}
