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
    public class UZCallbackCommands : UZBaseCommand
    {
        private readonly ISessionService _sessionService;
        private readonly IServiceCall _service;
        private readonly IRegService _regService;
        private readonly ILanguageText _languageText;
        public UZCallbackCommands(ISessionService sessionService, IServiceCall service, IRegService regService,ILanguageText languageText)
        {
            _sessionService = sessionService;
            _service = service;
            _regService = regService;
            _languageText = languageText;
        }
        public override string Name => "callback";

        public override async Task ExecuteAsync(Update update, UZSession session, TelegramBotClient _botClient)
        {
            var text = update.CallbackQuery.Data;
            UZRegistration registration = new UZRegistration();

            StringBuilder resp = new StringBuilder();
            var ChatId = update.CallbackQuery.Message.Chat.Id;
            switch (session.Type)
            {
                case UZOperType.middle_name:
                    resp.AppendLine(await _languageText.GetTextFromUZ(UZOperType.middle_name, session.language));
                    await _sessionService.UZUpdate(session, UZOperType.birthdate);
                    await _botClient.SendTextMessageAsync(ChatId, resp.ToString());
                    break;
                case UZOperType.region:
                    registration = await _regService.UZGetOrCreate(ChatId);
                    await _regService.UZUpdate(registration, ChatId, region_id: int.Parse(text));
                    var cities = await _service.GetCities(int.Parse(text));
                    if (cities.status == 200)
                    {
                        List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();
                        cities.Items.ForEach(rr =>
                        {
                            Buttons.Add(new List<InlineKeyboardButton>() { new InlineKeyboardButton(rr.name) { Text = rr.name, CallbackData = rr.id.ToString() } });
                        });
                        var city = new InlineKeyboardMarkup(Buttons);
                        await _botClient.SendTextMessageAsync(ChatId, session.language== Language.Rus? "Выберите город:" : "Shaharni tanlang:", replyMarkup: city);
                        await _sessionService.UZUpdate(session, UZOperType.city);
                    }
                    break;
                case UZOperType.city:
                    registration = await _regService.UZGetOrCreate(ChatId);
                    await _regService.UZUpdate(registration, ChatId, city_id: int.Parse(text));
                    await _botClient.SendTextMessageAsync(ChatId, session.language == Language.Rus ? "Укажите ваше имя:" : "Ismingizni kiriting:", replyMarkup: new ReplyKeyboardRemove());
                    await _sessionService.UZUpdate(session, UZOperType.name);
                    break;
            }
            await _botClient.DeleteMessageAsync(ChatId, update.CallbackQuery.Message.MessageId);
        }
    }
}
