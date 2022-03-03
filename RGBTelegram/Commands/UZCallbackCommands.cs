using RGBTelegram.Entities;
using RGBTelegram.Services;
using RGBTelegram.vpluse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RGBTelegram.Commands
{
    public class UZCallbackCommands : UZBaseCommand
    {
        private readonly ISessionService _sessionService;
        private readonly IServiceCall _service;
        private readonly IRegService _regService;
        public UZCallbackCommands(ISessionService sessionService, IServiceCall service, IRegService regService)
        {
            _sessionService = sessionService;
            _service = service;
            _regService = regService;
        }
        public override string Name => "callback";

        public override async Task ExecuteAsync(Update update, UZSession session, TelegramBotClient _botClient)
        {

        }
    }
}
