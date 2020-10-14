using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Currency
    {
        public string Name { get; set; }
        public decimal USD_in { get; set; }
        public decimal USD_out { get; set; }
    }

    public class Bank : IComparable<Bank>
    {
        public object locker = new object();
        public Exception error = null;

        public bool Yes { get { return this._Yes; } private set { this._Yes = value; } }
        private bool _Yes = false;

        public string Name { get {  return this._Name;  } private set { _Name = value; } }
        private string _Name;

        public string Address { get { return this._Address;  } private set { _Address = value; } }
        private string _Address;

        public decimal USD_in { get { lock (locker) { return this._USD_id; } } private set { _USD_id = value; } }
        private decimal _USD_id;

        public decimal USD_out { get { lock (locker) { return this._USD_out; } } private set { _USD_out = value; } }
        private decimal _USD_out;


        public Bank(string name, string address, string expression, int group1, int group2)
        {
            _Name = name;
            _Address = address;

            new Task(() =>
            {
                try
                {
                    lock (locker)
                    {
                        HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
                        request.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 70.0.3538.102 Safari / 537.36 Edge / 18.19041";

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        Match match = new Regex(expression, RegexOptions.Compiled | RegexOptions.Singleline).Match(reader.ReadToEnd().ToString());
                        reader.Close();

                        _USD_id = decimal.Parse((match.Groups[group1].Value).Replace(".", ","));
                        _USD_out = decimal.Parse((match.Groups[group2].Value).Replace(".", ","));

                    }
                }
                catch(Exception ex)
                {
                    this.error = ex;
                }
            }).Start();

            Yes = true;
        }

        public Bank(string name, string address, string expression, int group1, int group2, bool sync)
        {
            Name = name;
            Address = address;

            
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
                request.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 70.0.3538.102 Safari / 537.36 Edge / 18.19041";
                //request.CookieContainer = new CookieContainer { };
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());
                Match match = new Regex(expression, RegexOptions.Compiled | RegexOptions.Singleline).Match(reader.ReadToEnd().ToString());
                reader.Close();
                //match = Regex.Replace(Data, pattern, "");
                this.USD_in = decimal.Parse((match.Groups[group1].Value).Replace(".", ","));
                this.USD_out = decimal.Parse((match.Groups[group2].Value).Replace(".", ","));

                Yes = true;
            }
        }


        public int CompareTo(Bank b)
        {
            return this.USD_out.CompareTo(b.USD_out);
        }
        //public int CompareTo(Bank b)
        //{
        //    return b.USD_in.CompareTo(this.USD_in);
        //}

    }
    
}




