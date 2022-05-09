using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RGBTelegram.Services
{
    
    public class PepsiBot
    {
        private readonly IConfiguration _configuration;
        private TelegramBotClient _botClient;

        public PepsiBot(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TelegramBotClient> GetPepsiBot()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _botClient = new TelegramBotClient(_configuration["Pepsi_Token"]);

            var hook = $"{_configuration["Url"]}api/pepsi/update";
            await _botClient.SetWebhookAsync(hook);

            return _botClient;
        }

    }
}
