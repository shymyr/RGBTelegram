using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Services
{
    public interface IRestoreService
    {
        Task<RestorePassword> GetOrCreate(long ChatID);
        Task<RestorePassword> Update(RestorePassword restore, string phone = null, string sms = null, string passwod = null);
    }
}
