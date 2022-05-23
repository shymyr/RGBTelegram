using Microsoft.Extensions.DependencyInjection;
using RGBTelegram.Commands;
using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RGBTelegram.Services
{
    public class PepsiCommExecutor : IPepsiCommExecutor
    {
        private readonly ISessionService _sessionService;
        private readonly List<PepsiBaseCommand> _commands;
        private readonly TelegramBotClient _botClient;
        private PepsiBaseCommand _lastCommand;
        public PepsiCommExecutor(ISessionService sessionService, PepsiBot telegramBot, IServiceProvider serviceProvider)
        {
            _sessionService = sessionService;
            _botClient = telegramBot.GetPepsiBot().Result;
            _commands = serviceProvider.GetServices<PepsiBaseCommand>().ToList();
        }
        public async Task Execute(Update update)
        {
            var session = await _sessionService.PepsiGetOrCreate(update);
            if (update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;
            switch (update.Type)
            {
                case UpdateType.Message:
                    await ExecuteCommand("message", update, session);
                    break;
                case UpdateType.CallbackQuery:
                    await ExecuteCommand("callback", update, session);
                    break;
            }
        }

        private async Task ExecuteCommand(string commandName, Update update, PepsiSession session)
        {
            _lastCommand = _commands.First(x => x.Name == commandName);
            await _lastCommand.ExecuteAsync(update, session);
        }
    }
}
