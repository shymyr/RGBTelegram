﻿using Microsoft.Extensions.DependencyInjection;
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
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ISessionService _sessionService;
        private readonly List<BaseCommand> _commands;
        private readonly TelegramBotClient _botClient;
        private BaseCommand _lastCommand;
        public CommandExecutor(ISessionService sessionService, TelegramBot telegramBot, IServiceProvider serviceProvider)
        {
            _sessionService = sessionService;
            _botClient = telegramBot.GetBot().Result;
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
        }
        public async Task Execute(Update update)
        {
            var session = await _sessionService.GetOrCreate(update);
            if (update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;

            if ((DateTime.UtcNow - session.dateTime).Minutes < 15)
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        await ExecuteCommand("message", update, session);
                        break;
                }
            }
            else
            {
                await _botClient.SendTextMessageAsync(session.UserId, "Ваша сессия закрыто. Пожалуйста, авторизуйтесь и начните заново!", parseMode: ParseMode.Markdown);
                await _sessionService.Delete(session);

            }
        }

        private async Task ExecuteCommand(string commandName, Update update, UserSession session)
        {
            _lastCommand = _commands.First(x => x.Name == commandName);
            await _lastCommand.ExecuteAsync(update, session);
        }
    }
}
