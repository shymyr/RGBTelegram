using Microsoft.EntityFrameworkCore;
using RGBTelegram.Entities;
using RGBTelegram.vpluse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RGBTelegram.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IServiceCall _service;

        public AuthService(DataContext context, IServiceCall service)
        {
            _context = context;
            _service = service;
        }

        public async Task<AuthData> GetOrCreate(long ChatId, string phone = null)
        {
            var Auth = await _context.AuthDatas.FirstOrDefaultAsync(x => x.ChatId == ChatId);

            if (Auth != null)
            {
                if (!string.IsNullOrEmpty(phone))
                {
                    Auth.phone = phone;
                    _context.AuthDatas.Update(Auth);
                    await _context.SaveChangesAsync();
                }
                return Auth;
            }

            var result = await _context.AuthDatas.AddAsync(new AuthData() { ChatId = ChatId, phone = phone });
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        public async Task<PepsiAuthData> PepsiGetOrCreate(long ChatId, string phone = null)
        {
            var Auth = await _context.AuthPepsi.FirstOrDefaultAsync(x => x.ChatId == ChatId);

            if (Auth != null)
            {
                if (!string.IsNullOrEmpty(phone))
                {
                    Auth.phone = phone;
                    _context.AuthPepsi.Update(Auth);
                    await _context.SaveChangesAsync();
                }
                return Auth;
            }

            var result = await _context.AuthPepsi.AddAsync(new PepsiAuthData() { ChatId = ChatId, phone = phone });
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<AuthData> Update(AuthData auth, string phone = null, string passwod = null)
        {
            if (!string.IsNullOrEmpty(phone))
                auth.phone = phone;
            if (!string.IsNullOrEmpty(passwod))
                auth.password = passwod;
            _context.AuthDatas.Update(auth);
            await _context.SaveChangesAsync();

            return auth;
        }
        public async Task<PepsiAuthData> PepsiUpdate(PepsiAuthData auth, string phone = null, string passwod = null)
        {
            if (!string.IsNullOrEmpty(phone))
                auth.phone = phone;
            if (!string.IsNullOrEmpty(passwod))
                auth.password = passwod;
            _context.AuthPepsi.Update(auth);
            await _context.SaveChangesAsync();

            return auth;
        }
        public async Task<Token> GetOrCreateToken()
        {
            var Auth = await _context.UZAuthToken.FirstOrDefaultAsync();
            string newToken = string.Empty;
            if (Auth != null)
            {
                if (Auth.Expired < DateTime.Now.AddSeconds(10))
                {
                    newToken = await _service.GetToken(true);
                    await UpdateToken(Auth, newToken);
                }
                return Auth;
            }
            newToken = await _service.GetToken(true);
            var result = await _context.UZAuthToken.AddAsync(new Token() { Expired = DateTime.Now.AddSeconds(3600), AuthToken = newToken });
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        public async Task<Token> UpdateToken(Token auth, string token)
        {
            auth.AuthToken = token;
            auth.Expired = DateTime.Now.AddSeconds(3600);
            _context.UZAuthToken.Update(auth);
            await _context.SaveChangesAsync();
            return auth;
        }
    }
}
