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
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetOrCreate(Update update)
        {
            var newUser = update.Type switch
            {
                UpdateType.CallbackQuery => new AppUser
                {
                    Username = update.CallbackQuery.From.Username,
                    ChatId = update.CallbackQuery.Message.Chat.Id,
                    FirstName = update.CallbackQuery.Message.From.FirstName,
                    LastName = update.CallbackQuery.Message.From.LastName
                },
                UpdateType.Message => new AppUser
                {
                    Username = update.Message.Chat.Username,
                    ChatId = update.Message.Chat.Id,
                    FirstName = update.Message.Chat.FirstName,
                    LastName = update.Message.Chat.LastName
                }
            };

            var user = await _context.Users.FirstOrDefaultAsync(x => x.ChatId == newUser.ChatId);

            if (user != null) return user;

            var result = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<UZUser> GetUZUser(Update update)
        {
            var newUser = update.Type switch
            {
                UpdateType.CallbackQuery => new UZUser
                {
                    Username = update.CallbackQuery.From.Username,
                    ChatId = update.CallbackQuery.Message.Chat.Id,
                    FirstName = update.CallbackQuery.Message.From.FirstName,
                    LastName = update.CallbackQuery.Message.From.LastName
                },
                UpdateType.Message => new UZUser
                {
                    Username = update.Message.Chat.Username,
                    ChatId = update.Message.Chat.Id,
                    FirstName = update.Message.Chat.FirstName,
                    LastName = update.Message.Chat.LastName
                }
            };
            var user = await _context.UZUsers.FirstOrDefaultAsync(x => x.ChatId == newUser.ChatId);
            if (user != null) return user;
            var result = await _context.UZUsers.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
