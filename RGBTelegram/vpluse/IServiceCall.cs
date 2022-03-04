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
        Task<ErrorData> UZCheckRegistration(string phone);
        Task<ErrorData> AuthByPassword(AuthData auth);
        Task<Bundles> GetBundles(string token);
        Task<Promo> PromocodeActivation(PromoCode promo, string token, Language language);
        Task<SignUp> Register(Registration registration);
        Task<bool> TokenExpire(double expire);
        Task<ErrorData> SignUpConfirm(string phone, string sms_password);
        Task<ErrorData> CheckPhone(string phone);
        Task<Family> FamilyStatuses();
        Task<Family> Terms(int countryId, Language language);
        Task<Family> TermsPiala(Language language);
        Task<Family> TermsASU(Language language);
        Task<Family> AboutASU(Language language);
        Task<Family> AboutPiala(Language language);
        Task<Family> About(int countryId, Language language);
        Task<Family> GetRegions(int countryId);
        Task<Family> GetCities(int regionId);
        Task<bool> CorrectIIN(string IIN);
    }
}
