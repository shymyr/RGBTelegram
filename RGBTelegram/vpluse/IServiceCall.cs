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
        Task<ErrorData> UZCheckRegistration(string phone, string token);
        Task<ErrorData> AuthByPassword(AuthData auth);
        Task<ErrorData> AuthByPassword(PepsiAuthData auth);
        Task<Bundles> GetBundles(string token);
        Task<Bundles> GetBundlesPepsi(string token);
        Task<Promo> PromocodeActivation(PromoCode promo, string token, Language language);
        Task<Promo> PromocodeActivationPepsi(PromoCode promo, string token, Language language);
        Task<SignUp> Register(Registration registration);
        Task<SignUp> Register(PepsiRegistration registration);
        Task<bool> TokenExpire(double expire);
        Task<ErrorData> SignUpConfirm(string phone, string sms_password);
        Task<ErrorData> SendRestoreSMS(string phone);
        Task<ErrorData> ResetPassword(RestorePassword restore);
        Task<ErrorData> SignUpConfirmPepsi(string phone, string sms_password);
        Task<ErrorData> CheckPhone(string phone);
        Task<ErrorData> CheckPhonePepsi(string phone);
        Task<Family> FamilyStatuses();
        Task<Family> Terms(int countryId, Language language);
        Task<Family> TermsPepsi(int countryId, Language language);
        Task<Family> TermsPiala(Language language);
        Task<Family> TermsASU(Language language);
        Task<Family> FaqsAsu(Language language);
        Task<Family> FaqsPiala(Language language);
        Task<Family> AboutASU(Language language);
        Task<Family> AboutPiala(Language language);
        Task<Family> About(int countryId, Language language);
        Task<Family> AboutPepsi(int countryId, Language language);
        Task<Family> GetRegions(int countryId);
        Task<Family> GetCities(int regionId);
        Task<bool> CorrectIIN(string IIN);
        Task<Family> CheckpointsAsu(Language language);
        Task<Family> CheckpointsPiala(Language language);
        Task<string> GetToken(bool testConnect);
        Task<ErrorData> RegUZ(UZRegistration reg, string token, bool Piala);
        Task<ErrorData> regPiala(string phone, string token);
        Task<ErrorData> regAsu(string phone, string token);
    }
}
