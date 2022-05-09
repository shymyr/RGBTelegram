using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RGBTelegram.Commands
{
    public abstract class PepsiBaseCommand
    {
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(Update update, UserSession session);
    }
}
