﻿using Microsoft.EntityFrameworkCore;
using RGBTelegram.Entities;
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

        public AuthService(DataContext context)
        {
            _context = context;
        }

        public async Task<AuthData> GetOrCreate(long ChatId)
        {
            var Auth = await _context.AuthDatas.FirstOrDefaultAsync(x => x.ChatId == ChatId);

            if (Auth != null) return Auth;

            var result = await _context.AuthDatas.AddAsync(new AuthData() { ChatId = ChatId});
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<AuthData> Update(AuthData auth, string phone = null, string passwod = null)
        {
            if(!string.IsNullOrEmpty(phone))
                auth.phone =phone;
            if (!string.IsNullOrEmpty(passwod))
                auth.password = passwod;
            _context.AuthDatas.Update(auth);
            await _context.SaveChangesAsync();

            return auth;
        }
    }
}