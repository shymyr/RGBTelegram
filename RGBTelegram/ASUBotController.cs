using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RGBTelegram.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RGBTelegram
{
    [ApiController]
    [Route("api/asu/update")]
    public class ASUBotController: ControllerBase
    {
        private readonly IUZCommExecutor _commandExecutor;
        private readonly TelegramBotClient _botClient;
        public ASUBotController(IUZCommExecutor commandExecutor, AsuBot telegramBot)
        {
            _commandExecutor = commandExecutor;
            _botClient = telegramBot.GetASUBot().Result;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] object update)
        {
            var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

            if (upd?.Message?.Chat == null && upd?.CallbackQuery == null)
            {
                return Ok();
            }

            try
            {
                await _commandExecutor.Execute(upd, _botClient);
            }
            catch (Exception e)
            {
                return Ok();
            }

            return Ok();
        }
    }
}
