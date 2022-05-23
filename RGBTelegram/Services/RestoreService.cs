using Microsoft.EntityFrameworkCore;
using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Services
{
    public class RestoreService:IRestoreService
    {
        private readonly DataContext _context;
        public RestoreService(DataContext context)
        {
            _context = context;
        }

        public async Task<RestorePassword> GetOrCreate(long ChatID)
        {
            var rest = await _context.RestorePassword.FirstOrDefaultAsync(x => x.ChatID == ChatID);

            if (rest != null)
            {
                return rest;
            }

            var result = await _context.RestorePassword.AddAsync(new RestorePassword() { ChatID = ChatID });
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<RestorePassword> Update(RestorePassword restore, string phone = null,string sms=null, string passwod = null)
        {
            if (!string.IsNullOrEmpty(phone))
                restore.phone = phone;
            if (!string.IsNullOrEmpty(sms))
                restore.sms_code = sms;
            if (!string.IsNullOrEmpty(passwod))
                restore.new_password = passwod;
            _context.RestorePassword.Update(restore);
            await _context.SaveChangesAsync();

            return restore;
        }
    }
}
