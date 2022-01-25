using RGBTelegram.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RGBTelegram.vpluse
{
    public interface IServiceCall
    {
        //Task<HttpResponseMessage> CallService(StringContent content, string action, string methodType);

        Task<ErrorData> AuthByPassword(AuthData auth);
    }
}
