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
        public async Task<ErrorData> AuthByPassword(AuthData auth)
        {
            ErrorData result = new ErrorData();
            StringContent content = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json");
            var Response = await CallService(content, "v2/client/action/auth-by-password", "POST");
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

        public async Task<Family> GetRegions()
        {
            Family result = new Family();
            var Response = await CallService(null, "v2/client/catalog/regions/1", "GET");
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
            var Response = await CallService(null, $"v2/client/catalog/cities/{regionId}", "GET") ;
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

        public class Phone
        {
            public string phone { get; set; }
        }

    }
}
