﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RGBTelegram.Services
{
    public interface IPepsiCommExecutor
    {
        Task Execute(Update update);
    }
}
