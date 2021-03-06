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
    [Route("api/pepsi/update")]
    public class PepsiBotController : ControllerBase
    {
        private readonly IPepsiCommExecutor _commandExecutor;
        private readonly TelegramBotClient _botClient;
        public PepsiBotController(IPepsiCommExecutor commandExecutor, PepsiBot pepsiBot)
        {
            _botClient = pepsiBot.GetPepsiBot().Result;
            _commandExecutor = commandExecutor;
        }
        //
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
                await _commandExecutor.Execute(upd);
            }
            catch (Exception e)
            {
                
                await _botClient.SendTextMessageAsync(359972761, e.Message);
                return Ok();
            }

            return Ok();
        }
    }
}
