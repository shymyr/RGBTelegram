using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RGBTelegram.Services
{
    interface IAuthService
    {
        Task<AuthData> GetOrCreate(long ChatId);

        Task<AuthData> Update(AuthData auth, string phone = null, string passwod = null);
    }
}
