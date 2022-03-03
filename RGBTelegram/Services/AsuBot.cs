using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RGBTelegram.Services
{
    public class AsuBot
    {
        private readonly IConfiguration _configuration;
        private TelegramBotClient _botClient;
        public AsuBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TelegramBotClient> GetASUBot()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _botClient = new TelegramBotClient(_configuration["ASU_Token"]);

            var hook = $"{_configuration["Url"]}api/asu/update";
            await _botClient.SetWebhookAsync(hook);

            return _botClient;
        }
    }
}
