using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RGBTelegram.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RGBTelegram
{
    [ApiController]
    [Route("api/pepsi/update")]
    public class PepsiBotController : ControllerBase
    {
        private readonly IPepsiCommExecutor _commandExecutor;

        public PepsiBotController(IPepsiCommExecutor commandExecutor)
        {
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
                return Ok();
            }

            return Ok();
        }
    }
}
