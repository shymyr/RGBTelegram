using Microsoft.EntityFrameworkCore;
using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RGBTelegram.Services
{
    public class SessionService : ISessionService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        public SessionService(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public async Task<UserSession> GetOrCreate(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            var sesion = await _context.UserSessions.FirstOrDefaultAsync(x => x.User == user);
            if (sesion != null)
            {
                sesion.dateTime = DateTime.UtcNow;
                _context.UserSessions.Update(sesion);
                await _context.SaveChangesAsync();
                return sesion;
            }

            var newSession = update.Type switch
            {
                UpdateType.CallbackQuery => new UserSession
                {
                    User = user,
                    UserId = update.CallbackQuery.Message.Chat.Id
                },
                UpdateType.Message => new UserSession
                {
                    User = user,
                    UserId = update.Message.Chat.Id,
                    dateTime = DateTime.UtcNow
                }
            };
            await _context.UserSessions.AddAsync(newSession);
            await _context.SaveChangesAsync();
            return newSession;
        }

        public async Task Delete(UserSession session)
        {
            _context.UserSessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task Update(UserSession session, OperationType operation)
        {
            session.dateTime = DateTime.UtcNow;
            session.Type = operation;
            _context.UserSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task Update(UserSession session, OperationType operation, bool authorised)
        {
            session.dateTime = DateTime.UtcNow;
            session.Type = operation;
            session.Authorized = authorised;
            _context.UserSessions.Update(session);
            await _context.SaveChangesAsync();
        }
    }
}
