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
                //sesion.dateTime = DateTime.UtcNow;
                //_context.UserSessions.Update(sesion);
                //await _context.SaveChangesAsync();
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
        public async Task<PepsiSession> PepsiGetOrCreate(Update update)
        {
            var user = await _userService.PepsiGetOrCreate(update);
            var sesion = await _context.PepsiSessions.FirstOrDefaultAsync(x => x.User == user);
            if (sesion != null)
            {
                //sesion.dateTime = DateTime.UtcNow;
                //_context.UserSessions.Update(sesion);
                //await _context.SaveChangesAsync();
                return sesion;
            }

            var newSession = update.Type switch
            {
                UpdateType.CallbackQuery => new PepsiSession
                {
                    User = user,
                    UserId = update.CallbackQuery.Message.Chat.Id
                },
                UpdateType.Message => new PepsiSession
                {
                    User = user,
                    UserId = update.Message.Chat.Id,
                    dateTime = DateTime.UtcNow
                }
            };
            await _context.PepsiSessions.AddAsync(newSession);
            await _context.SaveChangesAsync();
            return newSession;
        }
        public async Task<UZSession> UZGetOrCreate(Update update)
        {
            var user = await _userService.GetUZUser(update);
            var sesion = await _context.UZSessions.FirstOrDefaultAsync(x => x.User == user);
            if (sesion != null)
            {
                return sesion;
            }

            var newSession = update.Type switch
            {
                UpdateType.CallbackQuery => new UZSession
                {
                    User = user,
                    UserId = update.CallbackQuery.Message.Chat.Id
                },
                UpdateType.Message => new UZSession
                {
                    User = user,
                    UserId = update.Message.Chat.Id,
                    dateTime = DateTime.UtcNow
                }
            };
            await _context.UZSessions.AddAsync(newSession);
            await _context.SaveChangesAsync();
            return newSession;
        }
        public async Task Delete(UserSession session)
        {
            _context.UserSessions.Remove(session);
            await _context.SaveChangesAsync();
        }
        public async Task PepsiDelete(PepsiSession session)
        {
            _context.PepsiSessions.Remove(session);
            await _context.SaveChangesAsync();
        }
        public async Task UZDelete(UZSession session)
        {
            _context.UZSessions.Remove(session);
            await _context.SaveChangesAsync();
        }
        public async Task Update(UserSession session, OperationType operation,string token=null, bool? authorised =null,Country? country = null, Language? language = null, double? expire = null)
        {
            session.dateTime = DateTime.UtcNow;
            session.Type = operation;
            if (!string.IsNullOrEmpty(token))
                session.Token = token;
            if (authorised.HasValue)
                session.Authorized = authorised.Value;
            if (country.HasValue)
                session.country = country.Value;
            if (language.HasValue)
                session.language = language.Value;
            if (expire.HasValue)
                session.expire = expire;
            _context.UserSessions.Update(session);
            await _context.SaveChangesAsync();
        }
        public async Task PepsiUpdate(PepsiSession session, OperationType operation, string token = null, bool? authorised = null, Country? country = null, Language? language = null, double? expire = null)
        {
            session.dateTime = DateTime.UtcNow;
            session.Type = operation;
            if (!string.IsNullOrEmpty(token))
                session.Token = token;
            if (authorised.HasValue)
                session.Authorized = authorised.Value;
            if (country.HasValue)
                session.country = country.Value;
            if (language.HasValue)
                session.language = language.Value;
            if (expire.HasValue)
                session.expire = expire;
            _context.PepsiSessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task UZUpdate(UZSession session, UZOperType operation, Language? language = null)
        {
            session.dateTime = DateTime.UtcNow;
            session.Type = operation;           
            _context.UZSessions.Update(session);
            if (language.HasValue)
                session.language = language.Value;
            await _context.SaveChangesAsync();
        }

    }
}
