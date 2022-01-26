using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Services
{
    public interface IRegService
    {
        Task<Registration> GetOrCreate(long ChatId, string phone = null);
        Task<Registration> Update(Registration registration, long ChatId, string phone = null, string password = null, string first_name = null,
           string last_name = null, string middlename = null, string gender = null, string family_stat = null, string birth_day = null,
           string email = null, int? city_id = null, int? region_id = null, string iin = null);
    }
}
