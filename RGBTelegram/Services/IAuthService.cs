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

        Task<AuthData> Update(AuthData auth, string phone = null, string passwod = null);
    }
}
