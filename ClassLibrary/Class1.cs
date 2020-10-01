using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace ClassLibrary
{
    public class Currency
    {
        public string Name { get; set; }
        public decimal USD_in { get; set; }
        public decimal USD_out { get; set; }
    }

    public class Bank
    {
        public string Name { get; set; }
        string Address { get; set; }
        public decimal USD_in { get; set; }
        public decimal USD_out { get; set; }
        public Bank(string name, string address, string expression, int group1, int group2)
        {
            Name = name;
            Address = address;
            HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
            request.UserAgent = "User - Agent: Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 70.0.3538.102 Safari / 537.36 Edge / 18.19041";
            //request.CookieContainer = new CookieContainer { };
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string Data = reader.ReadToEnd();
            reader.Close();


            string pattern = expression;
            RegexOptions options = RegexOptions.Compiled | RegexOptions.Singleline;
            Regex regex = new Regex(pattern, options);
            Match match = regex.Match(Data.ToString());
            //match = Regex.Replace(Data, pattern, "");
            this.USD_in = decimal.Parse((match.Groups[group1].Value).Replace(".", ","));
            this.USD_out = decimal.Parse((match.Groups[group2].Value).Replace(".", ","));
        }

    }
}




