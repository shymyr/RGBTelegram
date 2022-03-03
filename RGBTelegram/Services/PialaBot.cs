using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RGBTelegram.Services
{
    public class PialaBot
    {
        private readonly IConfiguration _configuration;
        private TelegramBotClient _botClient;
        public PialaBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TelegramBotClient> GetPialaBot()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _botClient = new TelegramBotClient(_configuration["Piala_Token"]);

            var hook = $"{_configuration["Url"]}api/piala/update";
            await _botClient.SetWebhookAsync(hook);
            var me =  _botClient.GetMeAsync().Result;
            return _botClient;
        }
    }
}
