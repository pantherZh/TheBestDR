using System;
using ClassLibrary;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;

namespace std
{
    class Program
    {
        static void Main(string[] agrs)
        {
            Bank nb = new Bank("Nb rb", "https://www.nbrb.by/api/exrates/rates/145", @"""Cur_OfficialRate"":(.*?)}", 1, 1, true);
            Console.WriteLine($"\t\t-----Курс национального банка РБ: {nb.USD_in}-----");

            
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Users\alesy\source\repos\TheBestDollarRate\tasks.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            Bank[] banks = new Bank[xRoot.ChildNodes.Count];

            Stopwatch stopwatch = Stopwatch.StartNew();

            Console.WriteLine("Start, всего банков " + xRoot.ChildNodes.Count);



            banks = StartParallelFor(xRoot);
            //banks = StartFor(xRoot);
            int i = 0;
            foreach (Bank b in banks)
            {
                Console.WriteLine($"{i}:{b.Name} -> {b.USD_in}/{b.USD_out}");
                i++;
            }

            Console.WriteLine("--------Покупка--------");
            Array.Sort(banks, Bank.SortUsdInDescending());
            foreach (Bank b in banks)
            {
                Console.WriteLine($"{b.USD_in} - {b.Name}");
            }
            Array.Sort(banks);
            Console.WriteLine("--------Продажа--------");
            foreach (Bank b in banks)
            {
                Console.WriteLine($"{b.USD_out} - {b.Name}");
            }

            stopwatch.Stop();
            Console.WriteLine("End tasks, elapsed time(ms) ->" + stopwatch.ElapsedMilliseconds);

            Console.WriteLine("\n\n\n");
        }

        public static Bank[] StartFor(XmlElement xRoot)
        {
            List<Bank> banks = new List<Bank>();
            string name = "", expression = "", address = "";
            int group1 = 0, group2 = 0; 

            foreach (XmlElement xnode in xRoot)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("name");

                if (attr != null)
                    name = attr.Value;

                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "address")
                        address = childnode.InnerText;
                    if (childnode.Name == "expression")
                        expression = childnode.InnerText;
                    if (childnode.Name == "group1")
                        group1 = int.Parse(childnode.InnerText);
                    if (childnode.Name == "group2")
                        group2 = int.Parse(childnode.InnerText);
                }

                banks.Add(new Bank(name, address, expression, group1, group2));
            }
            return banks.ToArray();
        }


        public static Bank[] StartParallelFor(XmlElement xRoot)
        {
            List<Bank> banks = new List<Bank>();
            object locker = new object();
            string name = "", expression = "", address = "";
            int group1 = 0, group2 = 0;

            ParallelLoopResult result = Parallel.For(0, xRoot.ChildNodes.Count, _ =>
            {
                
                lock (locker)
                {
                    Console.WriteLine("Добавление банка " + _);
                    name = xRoot.ChildNodes[_].Attributes["name"].Value;
                
                    foreach (XmlNode childnode in xRoot.ChildNodes[_].ChildNodes)
                    {
                        if (childnode.Name == "address")
                            address = childnode.InnerText;
                        if (childnode.Name == "expression")
                            expression = childnode.InnerText;
                        if (childnode.Name == "group1")
                            group1 = int.Parse(childnode.InnerText);
                        if (childnode.Name == "group2")
                            group2 = int.Parse(childnode.InnerText);
                    } 
                }
                banks.Add(new Bank(name, address, expression, group1, group2));
            });

            return banks.ToArray();
        }
    }
}