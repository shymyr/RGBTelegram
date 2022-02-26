using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public enum OperationType
    {
        start = 0,
        country =10,
        language=11,
        menu = 1,
        //Authorization
        auth = 2,
        telNumber = 21,
        Passw = 22,
        //Registration
        registration = 3,
        regTelNumber = 31,
        regTelNumber1 = 311,
        regPass = 32,
        regfamily_stat = 33, //Семейный статус(число от 0 до 4), 0 - Не выбраны, 1 - В браке не состою и тд.
        regcity = 34,
        regregion = 35,
        regIIN = 36,
        regSMS = 37,
        regSMSConfirm=38,
        Promotion = 4,//Об акции
        Promocode = 5,//Ввести код
        ProRule = 6,//Правила акции
        MyPromocodes = 7,//Мои промокоды и призы
        Questions = 8, //Вопросы и ответы
        first_name = 9,
        last_name=91,
        middlename =92,
        birth_day=93,
        birth_month=94,
        birth_year=95,
        gender=96,
        languageChange = 99


    }
}
