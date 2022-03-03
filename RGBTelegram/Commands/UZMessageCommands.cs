using RGBTelegram.Entities;
using RGBTelegram.Services;
using RGBTelegram.vpluse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RGBTelegram.Commands
{
    public class UZMessageCommands : UZBaseCommand
    {
        private readonly ISessionService _sessionService;
        private readonly IServiceCall _service;
        private readonly IRegService _regService;
        public UZMessageCommands(ISessionService sessionService, IServiceCall service, IRegService regService)
        {
            _sessionService = sessionService;
            _service = service;
            _regService = regService;
        }
        public override string Name => "message";

        public override async Task ExecuteAsync(Update update, UZSession session, TelegramBotClient _botClient)
        {
            var text = update.Message.Text;
            var ChatId = update.Message.Chat.Id;
            await _botClient.SendTextMessageAsync(ChatId, text);
            StringBuilder resp = new StringBuilder();
            switch (text)
            {
                case "/start":
                    resp.AppendLine("Привет я бот, тут вы можете регистрироваться. Для регистрации отправьте мне свой номер телефона с помощью кнопки «Поделиться телефоном» ");
                    var phone = new ReplyKeyboardMarkup(new KeyboardButton("Поделиться телефоном") { Text = "Поделиться телефоном", RequestContact = true });
                    phone.ResizeKeyboard = true;
                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString(), replyMarkup: phone);
                    await _sessionService.UZUpdate(session, UZOperType.start);
                    break;
                default:
                    if (update.Message.Contact != null)
                    {
                        var uzPhone = update.Message.Contact.PhoneNumber.Replace("+", "");
                        var check = await _service.UZCheckRegistration(uzPhone);
                        if (check.status == 200)
                        {
                            if (check.success)
                            {
                               await _botClient.SendTextMessageAsync(ChatId, "Вы уже регистрированы", replyMarkup: new ReplyKeyboardRemove());
                            }
                        }
                    }
                    break;
            }
        }
    }
}
