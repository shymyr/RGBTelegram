using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RGBTelegram.Services
{
    public interface IUZCommExecutor
    {
        Task Execute(Update update, TelegramBotClient _botClient);
    }
}
