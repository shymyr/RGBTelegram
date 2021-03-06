using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RGBTelegram.Services
{
    public interface IUserService
    {
        Task<AppUser> GetOrCreate(Update update);
        Task<PepsiUser> PepsiGetOrCreate(Update update);
        Task<UZUser> GetUZUser(Update update);
    }
}
