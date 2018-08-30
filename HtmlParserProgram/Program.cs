using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace HtmlParserProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            
            WebClient client1 = new WebClient();            
            //File.Delete("C://Users//g.stavrou//Downloads//test.html");
            //client1.DownloadFile("https://www.pamestoixima.gr/EN/1/sports", "C://Users//g.stavrou//Downloads//test.html");

            //client1.DownloadFile("http://www.nzherald.co.nz/", "C://Users//g.stavrou//Downloads//tes111t.html");
            //string path = File.ReadAllText("C://Users//g.stavrou//Downloads//tes111t.html");
            string path = File.ReadAllText("C://Users//g.stavrou//Downloads//test.html");

            HttpClient client = new HttpClient();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(path);
            //var headlineText = pageDocument.DocumentNode.SelectNodes("(//span[contains(@behavior.id,'ShowEvent')])");
            //foreach(HtmlNode node in headlineText)
            //{
            //    Console.WriteLine(node.OuterHtml);
            //}
            //Console.WriteLine(headlineText);



            var headlineTextTest = pageDocument.DocumentNode.SelectNodes("(//a[contains(@behavior.onmorebetsclicked.idfoevent,'970424.1')])");

            foreach (HtmlNode node in headlineTextTest)
            {
                Console.WriteLine(node.InnerHtml);
            }

            //Console.WriteLine(headlineTextTest);


            ///


            var headlineTextTestAfter = pageDocument.DocumentNode.SelectNodes("(//input[contains(@behavior.selectionwithbetdatesclick.idfoevent,'970424.1')])");
            foreach (HtmlNode node in headlineTextTestAfter)
            {
                Console.WriteLine(node.InnerHtml);
            }

            //Console.WriteLine(headlineTextTest);

            
            //Console.WriteLine(pageDocument.DocumentNode.InnerHtml);
            //MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        async static Task MainAsync(string[] args)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://www.nzherald.co.nz/");
            var pageContents = await response.Content.ReadAsStringAsync();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);
            var headlineText = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'pb-f-homepage-hero')]//h3)[1]").InnerText;
            Console.WriteLine(headlineText);
            Console.ReadLine();
        }
    }
}
