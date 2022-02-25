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
        Task<ErrorData> AuthByPassword(AuthData auth);
        Task<Bundles> GetBundles(string token);
        Task<ErrorData> PromocodeActivation(PromoCode promo, string token, Language language);
        Task<SignUp> Register(Registration registration);
        Task<ErrorData> SignUpConfirm(string phone, string sms_password);
        Task<ErrorData> CheckPhone(string phone);
        Task<Family> FamilyStatuses();
        Task<Family> Terms(int countryId, Language language);
        Task<Family> About(int countryId, Language language);
        Task<Family> GetRegions(int countryId);
        Task<Family> GetCities(int regionId);
        Task<bool> CorrectIIN(string IIN);
    }
}
