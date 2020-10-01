using System;
using ClassLibrary;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace std
{
    class Program
    {
        static void Main(string[] agrs)
        {



            //string date = DateTime.Now.ToShortDateString();
            //date = date.Replace(".", "/");


            Bank nb = new Bank("Nb rb", "https://www.nbrb.by/api/exrates/rates/145", @"""Cur_OfficialRate"":(.*?)}", 1, 1);//национальный банк - валюта вверху
            Console.WriteLine($"\t\t-----Курс национального банка РБ: {nb.USD_in}-----");

            Task<Bank>[] tasks = new Task<Bank>[]
               {
                 new Task<Bank>(()=> new Bank("AlfaBank","https://www.alfabank.by/currencys/",@"<span class=""informer-currencies_value-txt"">(.*?)</span>(.*?)<span class=""informer-currencies_value-txt"">(.*?)</span>",1,3)),
                 new Task<Bank>(()=> new Bank("IdeaBank", "https://www.ideabank.by/", @"<td>(.*?)</td>(.*?)<td>(.*?)</td>(.*?)<td>(.*?)</td>", 3, 5)),
                 new Task<Bank>(()=> new Bank("BelVeb","https://www.belveb.by/individual/",@"<td>Доллар США</td>(.*?)<td>(.*?)</td>(.*?)<td>(.*?)</td>",2,4)),
                 new Task<Bank>(()=> new Bank("Bank Dabrabyt","https://bankdabrabyt.by/currency_exchange/",@"<td>(.*?)</td>(.*?)<td>(.*?)</td><td>(.*?)</td>",3,4)),
                 new Task<Bank>(()=> new Bank("AbsolytBank","https://absolutbank.by/exchange-rates.xml",@"<buy>(.*?)</buy>(.*?)<sell>(.*?)</sell>",1,3)),
                 new Task<Bank>(()=> new Bank("Rbank","https://rbank.by/",@"<span class=""new-summ"">(.*?)</span>(.*?)<span class=""new-summ"">(.*?)</span>",1,3)),
                 new Task<Bank>(()=> new Bank("Bps-sberbank","https://www.bps-sberbank.by/",@"<div class=""BlockCurrencyExchangeRates__rate-item_buy-rate"">(.*?)</div>(.*?)<div class=""BlockCurrencyExchangeRates__rate-item_sell-rate"">(.*?)</div>",1,3)),
                 new Task<Bank>(()=> new Bank("BsbBank","https://www.bsb.by/",@"<span class=""exchange-scale"">(.*?)</span>(.*?)</th>(.*?)<td>(.*?)</td>(.*?)<td>(.*?)</td>",4,6) ),
                 new Task<Bank>(()=> new Bank("BtaBank","https://bta.by/currency-courses/",@"<div class=""result-sum--courses-sum"">(\s+.*?)</div>(.*?)<div class=""result-sum--courses-sum"">(\s+.*?)</div>",1,3) ),//нет инфо
                 new Task<Bank>(()=> new Bank("Vtb Bank","https://www.vtb-bank.by/sites/default/files/rates.xml",@"<code>usd</code><buy>(.*?)</buy><sell>(.*?)</sell>",1,2)),
                 new Task<Bank>(()=> new Bank("BelgazpromBank","https://belgazprombank.by/about/kursi_valjut/",@"<div class=""range-row"">(.*?)</div>(.*?)<div class=""range-row"">(.*?)</div>",1,3)),
                 new Task<Bank>(()=> new Bank("BelagropromBank","https://belapb.by/CashExRatesDaily.php?ondate=09/26/2020",@"<Name>Доллар США</Name>(.*?)<RateBuy>(.*?)</RateBuy>(.*?)<RateSell>(.*?)</RateSell>",2,4)),//нет инфо
                 new Task<Bank>(()=> new Bank("BelarusBank","https://belarusbank.by/api/kursExchange?city=Минск",@"USD_in"":""(.*?)"",""USD_out"":""(.*?)"",",1,2) ),
                 new Task<Bank>(()=> new Bank("BelinvestBank","https://www.belinvestbank.by/exchange-rates/courses-tab-cashless",@"<td class=""courses-table__td courses-table__td_buy"">(.*?)</td>(.*?)<td class=""courses-table__td courses-table__td_sell"">(.*?)</td>",1,3)),//пробелы
                 new Task<Bank>(()=> new Bank("Rrb","https://www.rrb.by/cursi",@"<span>USD/BYN</span>(.*?)<i>(.*?)(\s\|\s)([0-9]+\.[0-9]+)<\/i>",2,4)),
                 new Task<Bank>(()=> new Bank("ParitetBank","https://www.paritetbank.by/private/",@"<span>1 USD</span>(.*?)<span class=""(.*?)"">(.*?)</span>(.*?)<span class=""(.*?)"">(.*?)</span>",3,6) ),//down up
                 new Task<Bank>(()=> new Bank("Bnb","https://bnb.by/",@"<span class=""js-action_chtext"">(.*?)</span>(.*?)<span class=""js-action_chtext"">(.*?)</span>",1,3)),
              // new Task<Bank>(()=>Create_Bank_f("Priorbank","https://www.priorbank.by/offers/services/currency-exchange",@"<td class=""(.*?)"">1 Доллар США</td>(.*?)<td class=""(.*?)"">до 200.0</td>(.*?)<td class=""(.*?)"">(.*?)</td>(.*?)<td class=""(.*?)"">(.*?)</td>",6,9)),
                 new Task<Bank>(()=> new Bank("Mtb","https://www.mtbank.by/currxml.php?",@"<codeTo>USD</codeTo>(.*?)<purchase>(.*?)</purchase>(.*?)<sale>(.*?)</sale>",2,4)),
                 new Task<Bank>(()=> new Bank("Stbank","https://www.stbank.by/",@"<div class=""exchange-course-item__value"">(.*?)</div><div class=""exchange-course-item__value"">(.*?)</div>",1,2) ),
                 new Task<Bank>(()=> new Bank("FransaBank","https://fransabank.by/",@"<span class=""num"">(.*?)</span>(.*?)<span class=""num"">(.*?)</span>",1,3) ),
                 new Task<Bank>(()=> new Bank("TcBank","https://www.tcbank.by/",@"([0-9]+\.[0-9]+)(.*?)([0-9]+\.[0-9]+)(.*?)([0-9]+\.[0-9]+)(.*?)([0-9]+\.[0-9]+)",5,7) ),
                 new Task<Bank>(()=> new Bank("Technobank","https://www.tb.by/individuals/",@"<strong>USD</strong>(.*?)<span class=""currency-media-new-curr"">(.*?)</span>(.*?)<span class=""currency-media-new-curr"">(.*?)</span>",2,4)),
                 new Task<Bank>(()=> new Bank("Zepterbank","https://www.zepterbank.by/personal/services/currency/",@"<td> USD</td>(.*?)<td class=""rate_val "">(.*?)</td>(.*?)<td class=""rate_val "">(.*?)</td>",2,4))
           };
            foreach (var t in tasks)
            {
                t.Start();
            }
            Task.WaitAll(tasks);

            List<Currency> currency_in = new List<Currency> { };
            List<Currency> currency_out = new List<Currency> { };
            Bank b;
            foreach (var t in tasks)
            {
                b = t.Result;
                //Console.WriteLine("Name - " + b.Name + ":\t\t" + "\tПокупка - " + b.USD_in + "\tПродажа - " + b.USD_out);
                currency_in.Add(new Currency { Name = b.Name, USD_in = b.USD_in });
                currency_out.Add(new Currency { Name = b.Name, USD_out = b.USD_out });
            }

            var sortedCurrencyIn = from ci in currency_in
                                   orderby ci.USD_in descending
                                   select ci;
            Console.WriteLine("--------Покупка--------");
            foreach (Currency ci in sortedCurrencyIn)
            {
                Console.WriteLine($"{ci.USD_in} - {ci.Name}");
            }
            var sortedCurrencyOut = from co in currency_out
                                    orderby co.USD_out
                                    select co;
            Console.WriteLine("--------Продажа--------");
            foreach (Currency co in sortedCurrencyOut)
            {
                Console.WriteLine($"{co.USD_out} - {co.Name}");
            }


        }

    }
}