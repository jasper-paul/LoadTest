using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using LoadTestConsole.Model;

namespace LoadTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start now...");
            Console.ReadLine();
            ParseUrlSequenceRecorder();
            string xmlFile = "LoadTest.xml";
            if(args.Length > 0)
            {
                xmlFile = args[0];
            }

            if (!File.Exists(xmlFile))
            {
                Console.WriteLine("File specified is not found");
                Console.Read();

                return;
            }

            XElement elementLoad = XElement.Load(xmlFile);

            try
            {
                Parallel.ForEach(from xe in elementLoad.Elements("Test")
                                 select
                                     new UrlLoadGenerator(int.Parse(xe.Attribute("Time").Value),
                                     int.Parse(xe.Attribute("Count").Value),
                                     int.Parse(xe.Attribute("ConcurrentUsers").Value),
                                     xe.Attribute("Url").Value), model => model.RunTest());
             }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            Console.WriteLine("Test completed");
            Console.Read();
        }

        private static void ParseUrlSequenceRecorder()
        {
            string xmlFile = "logxml.xml";
            if (File.Exists(xmlFile))
            {
                XElement elementLoad = XElement.Load(xmlFile);
                var ele = from xe in elementLoad.Elements("Url") select xe.Attribute("address");
                XElement loadTestElement = new XElement("LoadTests", 
                    from e in ele select new XElement("Test",
                    new XAttribute("Time", 300000),
                    new XAttribute("Count", 100),
                    new XAttribute("ConcurrentUsers", 10),
                    new XAttribute("Url", e.Value.Replace("%26amp;","&"))));

                loadTestElement.Save("LoadTest.xml");
            }
        }
    }
}
