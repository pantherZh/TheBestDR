using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;
using System.Collections;

namespace ClassLibrary
{
    public class Bank : IComparable
    {
        public object locker = new object();
        public string Name { get { return this.name;  } private set { name = value; } }
        private string name;

        public string Address { get { return this.address;  } private set { address = value; } }
        private string address;

        public decimal USD_in { get { lock (locker) { return this.usd_id; } } private set { usd_id = value; } }
        private decimal usd_id;

        public decimal USD_out { get { lock (locker) { return this.usd_out; } } private set { usd_out = value; } }
        private decimal usd_out;


        public Bank(string name, string address, string expression, int group1, int group2)
        {
            Name = name;
            Address = address;

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

                        USD_in = decimal.Parse((match.Groups[group1].Value).Replace(".", ","));
                        USD_out = decimal.Parse((match.Groups[group2].Value).Replace(".", ","));
                    }
                }
                catch (WebException ex)
                {
                    WebExceptionStatus status = ex.Status;

                    if (status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)ex.Response;
                        Console.WriteLine("Статусный код ошибки: {0} - {1}",
                                (int)httpResponse.StatusCode, httpResponse.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }).Start();
        }
        public Bank(string name, string address, string expression, int group1, int group2, bool sync)
        {
            Name = name;
            Address = address;

            HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
            request.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 70.0.3538.102 Safari / 537.36 Edge / 18.19041";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            Match match = new Regex(expression, RegexOptions.Compiled | RegexOptions.Singleline).Match(reader.ReadToEnd().ToString());
            reader.Close();

            this.USD_in = decimal.Parse((match.Groups[group1].Value).Replace(".", ","));
            this.USD_out = decimal.Parse((match.Groups[group2].Value).Replace(".", ","));
        }

        public int CompareTo(object obj)
        { 
            Bank b = (Bank)obj;
            return Decimal.Compare(this.USD_out, b.USD_out);
        }

        private class SortUsdInDescendingHelper : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                Bank b1 = (Bank)x;
                Bank b2 = (Bank)y;
                return Decimal.Compare(b2.USD_in, b1.USD_in);
            }
        }
        public static IComparer SortUsdInDescending()
        {
            return (IComparer)new SortUsdInDescendingHelper();
        }
    }
    
}




