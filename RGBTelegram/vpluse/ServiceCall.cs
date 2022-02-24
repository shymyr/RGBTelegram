using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RGBTelegram.Entities;
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
            string apiBaseUrl = "https://staging-gateway.vpluse.me/";
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
            string apiBaseUrl = "https://staging-gateway.vpluse.me/";
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
                    result.data.Add(new Data() { field = "token", message = token });
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
        public async Task<ErrorData> PromocodeActivation(PromoCode promo, string token)
        {
            ErrorData result = new ErrorData();
            StringContent content = new StringContent(JsonConvert.SerializeObject(promo), Encoding.UTF8, "application/json");
            var Response = await CallServiceAuthorize(content, "v2/client/promo/codes", "POST", token);
            var resp = await Response.Content.ReadAsStringAsync();
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    result.data = new List<Data>();
                    JObject details = JObject.Parse(resp);
                    result.success = true;
                    var mesage = details["data"]["message"].ToString();
                    result.data.Add(new Data() { field = "message", message = mesage });
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                case System.Net.HttpStatusCode.InternalServerError:
                    result = JsonConvert.DeserializeObject<ErrorData>(resp);
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
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:

                    JObject details = JObject.Parse(resp);
                    result.Items = new List<Item>();
                    foreach (var item in details["data"].ToArray())
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
