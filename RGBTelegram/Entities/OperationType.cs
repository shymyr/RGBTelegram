using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RGBTelegram.Entities
{
    public enum OperationType
    {
        start = 0,
        menu = 1,
        //Authorization
        auth = 2,
        telNumber = 21,
        Passw = 22,
        //Registration
        registration = 3,
        regTelNumber = 31,
        regPass = 32,
        regfamily_stat = 33, //Семейный статус(число от 0 до 4), 0 - Не выбраны, 1 - В браке не состою и тд.
        regcity = 34,
        regregion = 35,
        regIIN = 36,
        regSMS = 37,
        Promotion = 4,//Об акции
        Promocode = 5,//Ввести код
        ProRule = 6,//Правила акции
        MyPromocodes = 7,//Мои промокоды и призы
        Questions = 8 //Вопросы и ответы

    }
}
