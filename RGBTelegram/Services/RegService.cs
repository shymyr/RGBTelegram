using Microsoft.EntityFrameworkCore;
using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Services
{
    public class RegService : IRegService
    {
        private readonly DataContext _context;
        public RegService(DataContext context)
        {
            _context = context;
        }

        public async Task<Registration> GetOrCreate(long ChatId, string phone = null)
        {
            var reg = await _context.Registrations.FirstOrDefaultAsync(x => x.ChatId == ChatId);

            if (reg != null) return reg;

            var result = await _context.Registrations.AddAsync(new Registration() { ChatId = ChatId, phone = phone });
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<UZRegistration> UZGetOrCreate(long ChatId, string phone = null)
        {
            var reg = await _context.UZRegistrations.FirstOrDefaultAsync(x => x.ChatId == ChatId);

            if (reg != null) return reg;

            var result = await _context.UZRegistrations.AddAsync(new UZRegistration() { ChatId = ChatId, phone = phone });
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Registration> Update(Registration registration, long ChatId, string phone = null, string password = null, string first_name = null,
            string last_name = null, string middlename = null, string gender = null, string family_stat = null, string birth_day = null,
            string email = null, int? city_id = null, int? region_id = null, string iin = null)
        {
            if (!string.IsNullOrEmpty(phone))
                registration.phone = phone;
            if (!string.IsNullOrEmpty(password))
                registration.password = password;
            if (!string.IsNullOrEmpty(first_name))
                registration.first_name = first_name;
            if (!string.IsNullOrEmpty(last_name))
                registration.last_name = last_name;
            if (!string.IsNullOrEmpty(middlename))
                registration.middlename = middlename;
            if (!string.IsNullOrEmpty(gender))
                registration.gender = gender;
            if (!string.IsNullOrEmpty(family_stat))
                registration.family_stat = family_stat;

            if (!string.IsNullOrEmpty(birth_day))
            {
                registration.birth_day = DateTime.Parse(birth_day).ToUniversalTime().ToString("s");
            }
            if (!string.IsNullOrEmpty(email))
                registration.email = email;
            if (!string.IsNullOrEmpty(phone))
                registration.phone = phone;
            if (city_id.HasValue)
                registration.city_id = city_id.Value;
            if (region_id.HasValue)
                registration.region_id = region_id.Value;
            if (!string.IsNullOrEmpty(iin))
                registration.iin = iin;

            _context.Registrations.Update(registration);
            await _context.SaveChangesAsync();

            return registration;
        }
        public async Task<UZRegistration> UZUpdate(UZRegistration registration, long ChatId, string phone = null, int? city_id = null, string name = null,
            string surname = null, string middle_name = null, string birthdate = null)
        {
            if (!string.IsNullOrEmpty(phone))
                registration.phone = phone;
            if (city_id.HasValue)
                registration.city_id = city_id.Value;
            if (!string.IsNullOrEmpty(name))
                registration.name = name;
            if (!string.IsNullOrEmpty(surname))
                registration.surname = surname;
            if (!string.IsNullOrEmpty(middle_name))
                registration.middle_name = middle_name;
            if (!string.IsNullOrEmpty(birthdate))
            {
                registration.birthdate = DateTime.Parse(birthdate).ToShortDateString();
            }

            _context.UZRegistrations.Update(registration);
            await _context.SaveChangesAsync();

            return registration;
        }
    }
}
