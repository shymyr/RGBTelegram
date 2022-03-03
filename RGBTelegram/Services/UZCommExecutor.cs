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
    public class UZCommExecutor: IUZCommExecutor
    {
        private readonly ISessionService _sessionService;
        private readonly List<UZBaseCommand> _commands;
        //private readonly TelegramBotClient _botClient;
        private UZBaseCommand _lastCommand;
        public UZCommExecutor(ISessionService sessionService,  IServiceProvider serviceProvider)
        {
            _sessionService = sessionService;
            //_botClient = telegramBot.GetBot().Result;
            _commands = serviceProvider.GetServices<UZBaseCommand>().ToList();
        }
        public async Task Execute(Update update, TelegramBotClient _botClient)
        {
            var session = await _sessionService.UZGetOrCreate(update);
            if (update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;
            switch (update.Type)
            {
                case UpdateType.Message:
                    await ExecuteCommand("message", update, session, _botClient);
                    break;
                case UpdateType.CallbackQuery:
                    await ExecuteCommand("callback", update, session, _botClient);
                    break;
            }
        }
        private async Task ExecuteCommand(string commandName, Update update, UZSession session, TelegramBotClient _botClient)
        {
            _lastCommand = _commands.First(x => x.Name == commandName);
            await _lastCommand.ExecuteAsync(update, session,_botClient);
        }
    }
}
