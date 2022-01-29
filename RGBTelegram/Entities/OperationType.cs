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
        Passw=22,
        //Registration
        registration=3,
        regTelNumber=31,
        regPass = 32,
        regFName=33,
        regLName=34,
        regMName=35,
        reggender=36,//1 - Мужчина, 2 - Женщина
        regfamily_stat=37, //Семейный статус(число от 0 до 4), 0 - Не выбраны, 1 - В браке не состою и тд.
        regcity=38,
        regregion=39,
        regIIN=310,
        Promotion = 4,//Об акции
        Promocode = 5,//Ввести код
        ProRule = 6,//Правила акции
        MyPromocodes=7,//Мои промокоды и призы
        Questions = 8 //Вопросы и ответы

    }
}
