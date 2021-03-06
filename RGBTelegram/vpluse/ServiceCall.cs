using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RGBTelegram.Entities;
using RGBTelegram.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RGBTelegram.vpluse
{
    public class ServiceCall : IServiceCall
    {
        public async Task<HttpResponseMessage> CallService(StringContent content, string action, string methodType)
        {
            string apiBaseUrl = "https://gateway.vpluse.me/";
            string endpoint = apiBaseUrl + action;
            HttpResponseMessage Response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                switch (methodType)
                {
                    case "POST":
                        Response = await client.PostAsync(endpoint, content);
                        break;
                    case "GET":
                        Response = await client.GetAsync(endpoint);
                        break;
                    case "PUT":
                        Response = await client.PutAsync(endpoint, content);
                        break;
                    case "DELETE":
                        Response = await client.DeleteAsync(endpoint);
                        break;
                }
            }
            return Response;
        }
        public async Task<HttpResponseMessage> CallService(StringContent content, string action, string methodType, bool testConnect)
        {
            string apiBaseUrl = testConnect ? "https://gateway.vpluse.me/" : "https://gateway.vpluse.me/";
            string endpoint = apiBaseUrl + action;
            HttpResponseMessage Response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                switch (methodType)
                {
                    case "POST":
                        Response = await client.PostAsync(endpoint, content);
                        break;
                    case "GET":
                        Response = await client.GetAsync(endpoint);
                        break;
                    case "PUT":
                        Response = await client.PutAsync(endpoint, content);
                        break;
                    case "DELETE":
                        Response = await client.DeleteAsync(endpoint);
                        break;
                }
            }
            return Response;
        }
        public async Task<HttpResponseMessage> CallServiceUZB(StringContent content, string action, string methodType)
        {
            string apiBaseUrl = "https://gateway.vpluse.me/";
            string endpoint = apiBaseUrl + action;
            HttpResponseMessage Response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                switch (methodType)
                {
                    case "POST":
                        Response = await client.PostAsync(endpoint, content);
                        break;
                    case "GET":
                        Response = await client.GetAsync(endpoint);
                        break;
                    case "PUT":
                        Response = await client.PutAsync(endpoint, content);
                        break;
                    case "DELETE":
                        Response = await client.DeleteAsync(endpoint);
                        break;
                }
            }
            return Response;
        }
        public async Task<HttpResponseMessage> CallServiceAuthorize(StringContent content, string action, string methodType, string token)
        {
            string apiBaseUrl = "https://gateway.vpluse.me/";
            string endpoint = apiBaseUrl + action;
            HttpResponseMessage Response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token + ":"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
                switch (methodType)
                {
                    case "POST":
                        Response = await client.PostAsync(endpoint, content);
                        break;
                    case "GET":
                        Response = await client.GetAsync(endpoint);
                        break;
                    case "PUT":
                        Response = await client.PutAsync(endpoint, content);
                        break;
                    case "DELETE":
                        Response = await client.DeleteAsync(endpoint);
                        break;
                }
            }
            return Response;
        }
        public async Task<HttpResponseMessage> CallServiceAuthorize(StringContent content, string action, string methodType, string token, bool testConnect)
        {
            string apiBaseUrl = testConnect ? "https://gateway.vpluse.me/" : "https://gateway.vpluse.me/"; //"https://gateway.vpluse.me/";
            string endpoint = apiBaseUrl + action;
            HttpResponseMessage Response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes(token + ":"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
                switch (methodType)
                {
                    case "POST":
                        Response = await client.PostAsync(endpoint, content);
                        break;
                    case "GET":
                        Response = await client.GetAsync(endpoint);
                        break;
                    case "PUT":
                        Response = await client.PutAsync(endpoint, content);
                        break;
                    case "DELETE":
                        Response = await client.DeleteAsync(endpoint);
                        break;
                }
            }
            return Response;
        }
        public async Task<ErrorData> UZCheckRegistration(string phone, string token)
        {
            ErrorData result = new ErrorData();
            var Response = await CallService(null, $"v2/nauryzpromo/uzb/check-registration/{phone}", "GET", token);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    result.status = 200;
                    JObject details = JObject.Parse(resp);
                    var isRegistred = details["data"]["is_registered"].ToString(); //is_registered
                    if (isRegistred.ToLower() == "true")
                        result.success = true;
                    else
                        result.success = false;
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    result.status = 401;
                    result.success = false;
                    result.data = new List<Data>();
                    result.data.Add(new Data() { message = "Не авторизован" });
                    break;
                default:
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }

            return result;
        }
        public async Task<ErrorData> AuthByPassword(AuthData auth)
        {
            ErrorData result = new ErrorData();
            StringContent content = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/auth-by-password", "POST");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    result.data = new List<Data>();
                    JObject details = JObject.Parse(resp);
                    var token = details["data"]["token"].ToString();
                    result.data.Add(new Data() { field = details["data"]["token_expired"].ToString(), message = token });
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }
            return result;
        }
        public async Task<ErrorData> AuthByPassword(PepsiAuthData auth)
        {
            ErrorData result = new ErrorData();
            StringContent content = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/auth-by-password", "POST", true);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    result.data = new List<Data>();
                    JObject details = JObject.Parse(resp);
                    var token = details["data"]["token"].ToString();
                    result.data.Add(new Data() { field = details["data"]["token_expired"].ToString(), message = token });
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }
            return result;
        }
        public async Task<Bundles> GetBundles(string token)
        {
            //
            Bundles result = new Bundles();
            var Response = await CallServiceAuthorize(null, "v2/nauryzpromo/attempts/bundles/1?count=10", "GET", token);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    result.gifts = new List<Gifts>();
                    JObject details = JObject.Parse(resp);
                    foreach (var items in details["data"]["items"].ToArray())
                    {
                        JObject gifts = JObject.Parse(items.ToString());
                        foreach (var gift in gifts["gifts"].ToArray())
                        {
                            Gifts gift1 = new Gifts();
                            gift1.created_at = gift["created_at"].ToString();
                            gift1.status = gift["status"].ToString();
                            gift1.namekz = gift["name"]["kz"].ToString();
                            gift1.nameru = gift["name"]["ru"].ToString();
                            gift1.namekg = gift["name"]["kg"].ToString();
                            gift1.description = gift["description"].ToString();
                            result.gifts.Add(gift1);
                        }
                        foreach (var attempts in gifts["attempts"].ToArray())
                        {
                            Attempts attempt = new Attempts();
                            attempt.createt_at = attempts["createt_at"].ToString();
                            attempt.promocode = attempts["promocode"].ToString();
                            attempt.product = attempts["add_info"]["product_id"].ToString();
                            attempt.brand = attempts["add_info"]["brand_id"].ToString();
                            result.attempts.Add(attempt);
                        }
                    }
                    result.status = 200;
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.error = err;
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Bundles> GetBundlesPepsi(string token)
        {
            Bundles result = new Bundles();
            var Response = await CallServiceAuthorize(null, "v2/pepsipromo/attempts/1?count=33", "GET", token,true);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    result.gifts = new List<Gifts>();
                    JObject details = JObject.Parse(resp);
                    foreach (var items in details["data"]["items"].ToArray())
                    {
                        Attempts attempt = new Attempts();
                        attempt.createt_at = items["createt_at"].ToString();
                        attempt.promocode = items["promocode"].ToString();

                        result.attempts.Add(attempt);
                        JObject gifts = JObject.Parse(items.ToString());
                        var gift_message = items["gift"].ToString();
                        if (!string.IsNullOrEmpty(gift_message))
                        {
                            JObject gMess = JObject.Parse(gift_message.ToString());

                            Gifts gift1 = new Gifts();
                            gift1.created_at = gMess["created_at"].ToString();
                            gift1.namekz = gMess["name"]["kz"].ToString();
                            gift1.nameru = gMess["name"]["ru"].ToString();
                            gift1.namekg = gMess["name"]["kg"].ToString();
                            gift1.description = gMess["description"].ToString();
                            result.gifts.Add(gift1);
                        }

                    }
                    result.status = 200;
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.error = err;
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Promo> PromocodeActivation(PromoCode promo, string token, Language language)
        {
            Promo result = new Promo();
            StringContent content = new StringContent(JsonConvert.SerializeObject(promo), Encoding.UTF8, "application/json");
            var Response = await CallServiceAuthorize(content, "v2/client/promo/codes", "POST", token);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    JObject details = JObject.Parse(resp);
                    var items = details["data"]["gift"].ToString();

                    JObject gifts = JObject.Parse(items.ToString());

                    foreach (var sms in gifts["messages"].ToArray())
                    {
                        Messages mess = new Messages();
                        mess.kz = sms["kz"].ToString();
                        mess.ru = sms["ru"].ToString();
                        mess.kg = sms["kg"].ToString();
                        result.messages.Add(mess);
                    }

                    foreach (var gift in gifts["gifts"].ToArray())
                    {
                        Gifts gift1 = new Gifts();
                        gift1.created_at = gift["created_at"].ToString();
                        gift1.status = gift["status"].ToString();
                        gift1.namekz = gift["name"]["kz"].ToString();
                        gift1.nameru = gift["name"]["ru"].ToString();
                        gift1.namekg = gift["name"]["kg"].ToString();
                        gift1.description = gift["description"].ToString();
                        result.gifts.Add(gift1);
                    }
                    foreach (var attempts in gifts["attempts"].ToArray())
                    {
                        Attempts attempt = new Attempts();
                        attempt.createt_at = attempts["createt_at"].ToString();
                        attempt.promocode = attempts["promocode"].ToString();
                        attempt.product = attempts["add_info"]["product_id"].ToString();
                        attempt.brand = attempts["add_info"]["brand_id"].ToString();
                        result.attempts.Add(attempt);
                    }
                    result.status = 200;
                    result.success = true;
                    string mesage = details["data"]["message"].ToString();
                    result.message = mesage;
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = language == Language.KAZ ? "Код табылмады.Кодтың дұрыс енгізілгенін тексеріңіз" :
                        (language == Language.KGZ ? "Код табылган жок. Коддунтуура киргизилгендигин текшериңиз" : "Код не найден. Проверьте правильно ли введён код");
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.error = err;
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Promo> PromocodeActivationPepsi(PromoCode promo, string token, Language language)
        {
            Promo result = new Promo();
            StringContent content = new StringContent(JsonConvert.SerializeObject(promo), Encoding.UTF8, "application/json");
            var Response = await CallServiceAuthorize(content, "v2/client/promo/codes", "POST", token,true);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    JObject details = JObject.Parse(resp);
                    var gift_message = details["data"]["gift_message"].ToString();
                    if (!string.IsNullOrEmpty(gift_message))
                    {
                        JObject gMess = JObject.Parse(gift_message.ToString());
                        Messages mess = new Messages();
                        mess.kz = gMess["kz"].ToString();
                        mess.ru = gMess["ru"].ToString();
                        mess.kg = gMess["kg"].ToString();
                        result.messages.Add(mess);
                    }
                    var gift = details["data"]["gift"].ToString();
                    if (!string.IsNullOrEmpty(gift))
                    {
                        JObject gifts = JObject.Parse(gift.ToString());
                        Gifts gift1 = new Gifts();
                        gift1.created_at = gifts["created_at"].ToString();
                        gift1.namekz = gifts["name"]["kz"].ToString();
                        gift1.nameru = gifts["name"]["ru"].ToString();
                        gift1.namekg = gifts["name"]["kg"].ToString();
                        gift1.description = gifts["description"].ToString();
                        result.gifts.Add(gift1);
                    }
                    result.status = 200;
                    result.success = true;
                    string mesage = details["data"]["message"].ToString();
                    result.message = mesage;
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = language == Language.KAZ ? "Код табылмады.Кодтың дұрыс енгізілгенін тексеріңіз" :
                        (language == Language.KGZ ? "Код табылган жок. Коддунтуура киргизилгендигин текшериңиз" : "Код не найден. Проверьте правильно ли введён код");
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.error = err;
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<SignUp> Register(Registration registration)
        {
            SignUp result = new SignUp();
            StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/phone-sign-up", "POST");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    var regData = JsonConvert.DeserializeObject<RegData>(resp);
                    result.success = true;
                    result.RegData = regData;
                    result.status = 201;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var error = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = error.data.First().message;
                    result.field = error.data.First().field;
                    break;
            }
            return result;
        }
        public async Task<SignUp> Register(PepsiRegistration registration)
        {
            SignUp result = new SignUp();
            StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/phone-sign-up", "POST", true);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    var regData = JsonConvert.DeserializeObject<RegData>(resp);
                    result.success = true;
                    result.RegData = regData;
                    result.status = 201;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var error = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = error.data.First().message;
                    result.field = error.data.First().field;
                    break;
            }
            return result;
        }
        public async Task<bool> TokenExpire(double expire)
        {
            var expTime = ConvertFromUnixTimestamp(expire);
            if (expTime >= DateTime.Now.AddSeconds(5))
                return true;
            else
                return false;
        }
        public DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp).ToLocalTime();
        }
        public async Task<ErrorData> SignUpConfirm(string phone, string sms_password)
        {
            ErrorData result = new ErrorData();
            SMSConfirm sms = new SMSConfirm() { phone = phone, sms_password = sms_password };
            StringContent content = new StringContent(JsonConvert.SerializeObject(sms), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/phone-sign-up-confirm", "POST");
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var resp = await Response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }
            return result;
        }

        public async Task<ErrorData> SendRestoreSMS(string phone)
        {
            ErrorData result = new ErrorData();
            SMSConfirm sms = new SMSConfirm() { phone = phone };
            StringContent content = new StringContent(JsonConvert.SerializeObject(sms), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/send-restore-sms", "POST",true);
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var resp = await Response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }
            return result;
        }

        public async Task<ErrorData> ResetPassword(RestorePassword restore)
        {
            ErrorData result = new ErrorData();
            StringContent content = new StringContent(JsonConvert.SerializeObject(restore), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/reset-password", "POST", true);
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var resp = await Response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }
            return result;
        }

        public async Task<ErrorData> SignUpConfirmPepsi(string phone, string sms_password)
        {
            ErrorData result = new ErrorData();
            SMSConfirm sms = new SMSConfirm() { phone = phone, sms_password = sms_password };
            StringContent content = new StringContent(JsonConvert.SerializeObject(sms), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/phone-sign-up-confirm", "POST", true);
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var resp = await Response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }
            return result;
        }
        public async Task<ErrorData> CheckPhone(string phone)
        {
            ErrorData result = new ErrorData();
            Phone contentphone = new Phone() { phone = phone };
            StringContent content = new StringContent(JsonConvert.SerializeObject(contentphone), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/check-phone", "POST");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var details = JObject.Parse(resp);
                    result.success = true;
                    result.success = bool.Parse(details["data"]["is_exist"].ToString());
                    result.status = int.Parse(details["status"].ToString());

                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }
            return result;
        }
        public async Task<ErrorData> CheckPhonePepsi(string phone)
        {
            ErrorData result = new ErrorData();
            Phone contentphone = new Phone() { phone = phone };
            StringContent content = new StringContent(JsonConvert.SerializeObject(contentphone), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/check-phone", "POST", true);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var details = JObject.Parse(resp);
                    result.success = true;
                    result.success = bool.Parse(details["data"]["is_exist"].ToString());
                    result.status = int.Parse(details["status"].ToString());

                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }
            return result;
        }
        public async Task<Family> FamilyStatuses()
        {
            Family result = new Family();
            var Response = await CallService(null, "v2/client/catalog/familyStatuses", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    result.Items = new List<Item>();
                    JObject details = JObject.Parse(resp);
                    foreach (var item in details["data"]["items"].ToArray())
                    {
                        result.Items.Add(new Item() { id = int.Parse(item["id"].ToString()), name = item["name"].ToString() });
                    }
                    result.status = 200;
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> Terms(int countryId, Language language)
        {
            Family result = new Family();
            var Response = await CallService(null, $"v2/nauryzpromo/terms/{countryId}", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        Item term = new Item();
                        term.id = 1;
                        Item file = new Item();
                        file.id = 2;
                        switch (language)
                        {
                            case Language.KAZ:
                                term.name = item["term"]["kz"].ToString();
                                file.name = item["file"]["kz"].ToString();
                                break;
                            case Language.KGZ:
                                term.name = item["term"]["kg"].ToString();
                                file.name = item["file"]["kg"].ToString();
                                break;
                            case Language.Rus:
                                term.name = item["term"]["ru"].ToString();
                                file.name = item["file"]["ru"].ToString();
                                break;
                        }
                        result.Items.Add(term);
                        result.Items.Add(file);
                    }
                    result.status = 200;
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> TermsPepsi(int countryId, Language language)
        {
            Family result = new Family();
            var Response = await CallService(null, $"v2/pepsipromo/terms/{countryId}", "GET", true);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        Item term = new Item();
                        term.id = 1;
                        Item file = new Item();
                        file.id = 2;
                        switch (language)
                        {
                            case Language.KAZ:
                                term.name = item["term"]["kz"].ToString();
                                file.name = item["file"]["kz"].ToString();
                                break;
                            case Language.KGZ:
                                term.name = item["term"]["kg"].ToString();
                                file.name = item["file"]["kg"].ToString();
                                break;
                            case Language.Rus:
                                term.name = item["term"]["ru"].ToString();
                                file.name = item["file"]["ru"].ToString();
                                break;
                        }
                        result.Items.Add(term);
                        result.Items.Add(file);
                    }
                    result.status = 200;
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> TermsASU(Language language)
        {
            Family result = new Family();
            var Response = await CallServiceUZB(null, "v2/nauryzpromo/uzb/asu/terms", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        Item term = new Item();
                        term.id = 1;
                        Item file = new Item();
                        file.id = 2;
                        switch (language)
                        {
                            case Language.UZB:
                                term.name = item["term"]["uz"].ToString();
                                file.name = item["file"]["uz"].ToString();
                                break;
                            case Language.Rus:
                                term.name = item["term"]["ru"].ToString();
                                file.name = item["file"]["ru"].ToString();
                                break;
                        }
                        result.Items.Add(term);
                        result.Items.Add(file);
                    }
                    result.status = 200;
                    result.success = true;
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> TermsPiala(Language language)
        {
            Family result = new Family();
            var Response = await CallServiceUZB(null, "v2/nauryzpromo/uzb/asu/terms", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        Item term = new Item();
                        term.id = 1;
                        Item file = new Item();
                        file.id = 2;
                        switch (language)
                        {
                            case Language.UZB:
                                term.name = item["term"]["uz"].ToString();
                                file.name = item["file"]["uz"].ToString();
                                break;
                            case Language.Rus:
                                term.name = item["term"]["ru"].ToString();
                                file.name = item["file"]["ru"].ToString();
                                break;
                        }
                        result.Items.Add(term);
                        result.Items.Add(file);
                    }
                    result.status = 200;
                    result.success = true;
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> FaqsAsu(Language language)
        {
            Family result = new Family();
            var Response = await CallServiceUZB(null, "v2/nauryzpromo/uzb/asu/faqs", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            StringBuilder builder = new StringBuilder();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        Item question = new Item();
                        question.id = int.Parse(item["id"].ToString());
                        Item answer = new Item();
                        answer.id = int.Parse(item["id"].ToString());
                        switch (language)
                        {
                            case Language.UZB:
                                question.name = item["question"]["uz"].ToString();
                                answer.name = item["answer"]["uz"].ToString();
                                break;
                            case Language.Rus:
                                question.name = item["question"]["ru"].ToString();
                                answer.name = item["answer"]["ru"].ToString();
                                break;
                        }
                        result.Items.Add(question);
                        builder.AppendLine(question.name);
                        //builder.AppendLine(answer.name);
                    }
                    result.status = 200;
                    result.success = true;
                    result.message = builder.ToString();
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> FaqsPiala(Language language)
        {
            Family result = new Family();
            var Response = await CallServiceUZB(null, "v2/nauryzpromo/uzb/piala/faqs", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            StringBuilder builder = new StringBuilder();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        Item question = new Item();
                        question.id = int.Parse(item["id"].ToString());
                        Item answer = new Item();
                        answer.id = int.Parse(item["id"].ToString());
                        switch (language)
                        {
                            case Language.UZB:
                                question.name = item["question"]["uz"].ToString();
                                answer.name = item["answer"]["uz"].ToString();
                                break;
                            case Language.Rus:
                                question.name = item["question"]["ru"].ToString();
                                answer.name = item["answer"]["ru"].ToString();
                                break;
                        }
                        result.Items.Add(question);
                        builder.AppendLine(question.name);
                        //builder.AppendLine(answer.name);
                    }
                    result.status = 200;
                    result.success = true;
                    result.message = builder.ToString();
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> AboutASU(Language language)
        {
            Family result = new Family();
            var Response = await CallServiceUZB(null, "v2/nauryzpromo/uzb/asu/about", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            StringBuilder builder = new StringBuilder();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        if (item["description"].ToString() != "{}")
                        {
                            Item item1 = new Item();
                            item1.id = int.Parse(item["id"].ToString());
                            switch (language)
                            {
                                case Language.UZB:
                                    item1.name = item["description"]["uz"].ToString();
                                    break;
                                case Language.Rus:
                                    item1.name = item["description"]["ru"].ToString();
                                    break;
                            }
                            result.Items.Add(item1);
                            builder.AppendLine(item1.name);
                        }
                    }
                    result.status = 200;
                    result.success = true;
                    result.message = builder.ToString();
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }

            return result;
        }
        public async Task<Family> AboutPiala(Language language)
        {
            Family result = new Family();
            var Response = await CallServiceUZB(null, "v2/nauryzpromo/uzb/piala/about", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            StringBuilder builder = new StringBuilder();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        if (item["description"].ToString() != "{}")
                        {
                            Item item1 = new Item();
                            item1.id = int.Parse(item["id"].ToString());
                            switch (language)
                            {
                                case Language.UZB:
                                    item1.name = item["description"]["uz"].ToString();
                                    break;
                                case Language.Rus:
                                    item1.name = item["description"]["ru"].ToString();
                                    break;
                            }
                            result.Items.Add(item1);
                            builder.AppendLine(item1.name);
                        }
                    }
                    result.status = 200;
                    result.success = true;
                    result.message = builder.ToString();
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }

            return result;
        }
        /// <summary>
        /// Получить описание акции
        /// </summary>
        /// <param name="countryId">Код страны 1 - Казахстан, 2 - Киргиз</param>
        /// <returns></returns>
        public async Task<Family> About(int countryId, Language language)
        {
            Family result = new Family();
            var Response = await CallService(null, $"v2/nauryzpromo/about/{countryId}", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            StringBuilder builder = new StringBuilder();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        if (item["description"].ToString() != "{}")
                        {
                            Item item1 = new Item();
                            item1.id = int.Parse(item["id"].ToString());
                            switch (language)
                            {
                                case Language.KAZ:
                                    item1.name = item["description"]["kz"].ToString();
                                    break;
                                case Language.KGZ:
                                    item1.name = item["description"]["kg"].ToString();
                                    break;
                                case Language.Rus:
                                    item1.name = item["description"]["ru"].ToString();
                                    break;
                            }
                            result.Items.Add(item1);
                            builder.AppendLine(item1.name);
                        }
                    }
                    result.status = 200;
                    result.success = true;
                    result.message = builder.ToString();
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }

        public async Task<Family> AboutPepsi(int countryId, Language language)
        {
            Family result = new Family();
            var Response = await CallService(null, $"v2/pepsipromo/about/{countryId}", "GET", true);
            var resp = await Response.Content.ReadAsStringAsync();
            StringBuilder builder = new StringBuilder();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
                    {
                        if (item["description"].ToString() != "{}")
                        {
                            Item item1 = new Item();
                            item1.id = int.Parse(item["id"].ToString());
                            switch (language)
                            {
                                case Language.KAZ:
                                    item1.name = item["description"]["kz"].ToString();
                                    break;
                                case Language.KGZ:
                                    item1.name = item["description"]["kg"].ToString();
                                    break;
                                case Language.Rus:
                                    item1.name = item["description"]["ru"].ToString();
                                    break;
                            }
                            result.Items.Add(item1);
                            builder.AppendLine(item1.name);
                        }
                    }
                    result.status = 200;
                    result.success = true;
                    result.message = builder.ToString();
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> GetRegions(int countryId)
        {
            Family result = new Family();
            var Response = await CallService(null, $"v2/client/catalog/regions/{countryId}", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    result.Items = new List<Item>();
                    JObject details = JObject.Parse(resp);
                    foreach (var item in details["data"]["items"].ToArray())
                    {
                        result.Items.Add(new Item() { id = int.Parse(item["id"].ToString()), name = item["name_ru"].ToString() });
                    }
                    result.status = 200;
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }

        public async Task<Family> GetCities(int regionId)
        {
            Family result = new Family();
            var Response = await CallService(null, $"v2/client/catalog/cities/{regionId}", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    result.Items = new List<Item>();
                    JObject details = JObject.Parse(resp);
                    foreach (var item in details["data"]["items"].ToArray())
                    {
                        result.Items.Add(new Item() { id = int.Parse(item["id"].ToString()), name = item["name_ru"].ToString() });
                    }
                    result.status = 200;
                    result.success = true;
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }

        public async Task<bool> CorrectIIN(string IIN)
        {
            int s = 0;
            for (int i = 0; i < 11; i++)
            {
                s = s + (i + 1) * int.Parse(IIN.ToCharArray()[i].ToString());
            }
            int k = s % 11;
            if (k == 10)
            {
                s = 0;
                for (int i = 0; i < 11; i++)
                {
                    int t = (i + 3) % 11;
                    if (t == 0)
                    {
                        t = 11;
                    }
                    s = s + t * int.Parse(IIN.ToCharArray()[i].ToString());
                }
                k = s % 11;
                if (k == 10)
                    return false;

                return (k == int.Parse(IIN.Substring(11, 1)));
            }
            return (k == int.Parse(IIN.Substring(11, 1)));
        }

        public async Task<Family> CheckpointsAsu(Language language)
        {
            Family result = new Family();
            var Response = await CallServiceUZB(null, "v2/nauryzpromo/uzb/asu/checkpoints", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            StringBuilder builder = new StringBuilder();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    var items = details["data"]["title"].ToString();

                    JObject titles = JObject.Parse(items.ToString());
                    var title = language == Language.UZB ? titles["uz"].ToString() : titles["ru"].ToString();
                    builder.AppendLine(title);
                    int k = 0;
                    foreach (var item in details["data"]["checkpoints"].ToArray())
                    {
                        builder.AppendLine();
                        if (item["city"].ToString() != "{}")
                            builder.AppendLine(item["city"]["name"].ToString());
                        if (item["name"].ToString() != "{}")
                            builder.AppendLine(language == Language.UZB ? item["name"]["uz"].ToString() : item["name"]["ru"].ToString());
                        if (item["address"].ToString() != "{}")
                            builder.AppendLine(language == Language.UZB ? item["address"]["uz"].ToString() : item["address"]["ru"].ToString());
                        if (item["working_hours"].ToString() != "{}")
                            builder.AppendLine(language == Language.UZB ? item["working_hours"]["uz"].ToString() : item["working_hours"]["ru"].ToString());
                        k++;
                        Item text = new Item();
                        text.id = k;
                        text.name = builder.ToString();
                        Item coord = new Item();
                        coord.id = k + 1;
                        coord.name = item["coordinates"].ToString();
                        result.Items.Add(text);
                        result.Items.Add(coord);
                    }
                    result.status = 200;
                    result.success = true;
                    result.message = builder.ToString();
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }
        public async Task<Family> CheckpointsPiala(Language language)
        {
            Family result = new Family();
            var Response = await CallServiceUZB(null, "v2/nauryzpromo/uzb/piala/checkpoints", "GET");
            var resp = await Response.Content.ReadAsStringAsync();
            StringBuilder builder = new StringBuilder();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    var items = details["data"]["title"].ToString();

                    JObject titles = JObject.Parse(items.ToString());
                    var title = language == Language.UZB ? titles["uz"].ToString() : titles["ru"].ToString();
                    builder.AppendLine(title);
                    int k = 0;
                    foreach (var item in details["data"]["checkpoints"].ToArray())
                    {
                        builder.AppendLine();
                        if (item["city"].ToString() != "{}")
                            builder.AppendLine(item["city"]["name"].ToString());
                        if (item["name"].ToString() != "{}")
                            builder.AppendLine(language == Language.UZB ? item["name"]["uz"].ToString() : item["name"]["ru"].ToString());
                        if (item["address"].ToString() != "{}")
                            builder.AppendLine(language == Language.UZB ? item["address"]["uz"].ToString() : item["address"]["ru"].ToString());
                        if (item["working_hours"].ToString() != "{}")
                            builder.AppendLine(language == Language.UZB ? item["working_hours"]["uz"].ToString() : item["working_hours"]["ru"].ToString());
                        k++;
                        Item text = new Item();
                        text.id = k;
                        text.name = builder.ToString();
                        Item coord = new Item();
                        coord.id = k + 1;
                        coord.name = item["coordinates"].ToString();
                        result.Items.Add(text);
                        result.Items.Add(coord);
                    }
                    result.status = 200;
                    result.success = true;
                    result.message = builder.ToString();
                    break;
                default:
                    var err = JsonConvert.DeserializeObject<ErrorData>(resp);
                    result.status = ((int)Response.StatusCode);
                    result.success = false;
                    result.message = err.data.First().message;
                    break;
            }
            return result;
        }

        public async Task<HttpResponseMessage> CallService(StringContent content, string action, string methodType, string token = null)
        {
            string apiBaseUrl = "https://gateway.vpluse.me/";
            string endpoint = apiBaseUrl + action;
            HttpResponseMessage Response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                switch (methodType)
                {
                    case "POST":
                        Response = await client.PostAsync(endpoint, content);
                        break;
                    case "GET":
                        Response = await client.GetAsync(endpoint);
                        break;
                    case "PUT":
                        Response = await client.PutAsync(endpoint, content);
                        break;
                    case "DELETE":
                        Response = await client.DeleteAsync(endpoint);
                        break;
                }
            }
            return Response;
        }
        public async Task<string> GetToken(bool testConnect)
        {
            string token = string.Empty;
            HttpResponseMessage Response = new HttpResponseMessage();
            using (var httpClient = new HttpClient())
            {
                var BaseAddress = new Uri(testConnect ? "https://identity.vpluse.me/" : "https://identity.vpluse.me/");
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "vpluse.pepsipromo2021.api.client"),
                new KeyValuePair<string, string>("client_secret", "4mgEr7AYWuBctZ78PqQafTnd"),
                new KeyValuePair<string, string>("scope","gateway.api")
            });

                Response = await httpClient.PostAsync(BaseAddress + "connect/token", content);
                var resp = await Response.Content.ReadAsStringAsync();
                switch (Response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        JObject details = JObject.Parse(resp);
                        token = details["access_token"].ToString();
                        break;
                }
            }
            return token;
        }
        public async Task<ErrorData> RegUZ(UZRegistration reg, string token, bool Piala)
        {
            ErrorData result = new ErrorData();
            StringContent content = new StringContent(JsonConvert.SerializeObject(reg), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/nauryzpromo/uzb/registration/no-password", "POST", token);
            var resp = await Response.Content.ReadAsStringAsync();
            //switch (Response.StatusCode)
            //{
            //    case System.Net.HttpStatusCode.OK:
            result = Piala ? await regPiala(reg.phone, token) : await regAsu(reg.phone, token);
            //        break;
            //    default:
            //        result = JsonConvert.DeserializeObject<ErrorData>(resp);
            //        break;
            //}

            return result;
        }
        public async Task<ErrorData> regPiala(string phone, string token)
        {
            Confirm confirm = new Confirm();
            confirm.phone = phone;
            ErrorData result = new ErrorData();
            StringContent content = new StringContent(JsonConvert.SerializeObject(confirm), Encoding.UTF8, "application/json");
            var Response = await CallService(content, $"v2/nauryzpromo/uzb/registration/{confirm.phone}/product/piala", "POST", token);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    result.status = 200;
                    result.success = true;
                    break;
                default:
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }

            return result;
        }
        public async Task<ErrorData> regAsu(string phone, string token)
        {
            Confirm confirm = new Confirm();
            confirm.phone = phone;
            ErrorData result = new ErrorData();
            StringContent content = new StringContent(JsonConvert.SerializeObject(confirm), Encoding.UTF8, "application/json");
            var Response = await CallService(content, $"v2/nauryzpromo/uzb/registration/{confirm.phone}/product/asu", "POST", token);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    result.status = 200;
                    result.success = true;
                    break;
                default:
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
                    break;
            }

            return result;
        }

        public class Confirm
        {
            public string phone { get; set; }
        }
        public class Phone
        {
            public string phone { get; set; }
        }
        public class Auth
        {
            public string phone { get; set; }
            public string password { get; set; }
        }
        public class SMSConfirm
        {
            public string phone { get; set; }
            public string sms_password { get; set; }
        }
    }
}
