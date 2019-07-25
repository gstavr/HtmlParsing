using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Net.Http;
using System.Collections.Generic;
using System.Data;
using HtmlParserProgram.Models;
using System.Linq;
namespace HtmlParserProgram
{
    class Program
    {
        static void Main(string[] args)
        {

            //GetData();

            //WebClient client1 = new WebClient();
            //client1.DownloadFile("http://www.nzherald.co.nz/", "C://Users//g.stavrou//Downloads//tes111t.html");
            
            

            //! Go to Home Page Pame Stoi
            IWebDriver driver1 = new ChromeDriver(@"C:\Users\Mpoumpos\Source\Repos\HtmlParsing\HtmlParserProgram\bin\Debug\netcoreapp2.1");
            
            PameStoixima pameStoixima = new PameStoixima("https://www.pamestoixima.gr/desktop/home", driver1);
            string companyUrl = string.Empty;
            DataBase cmp = new DataBase();

            //foreach (DataRow row in cmp.companiesDataTable.Rows)
            //{
            //    string company = row["Descr"].ToString();

            //    switch (company)
            //    {
            //        case "OPAP":
            //            //PameStoixima pameStoixima = new PameStoixima(row["Link"].ToString() , driver1);
            //            PameStoixima pameStoixima = new PameStoixima("https://www.pamestoixima.gr/EN/1/sports#action=sports", driver1);
            //            companyUrl = pameStoixima.url;
            //            break;
            //    }
            //}
            

            
            
            //IJavaScriptExecutor js = (IJavaScriptExecutor)driver1;
            //string title = (string)js.ExecuteScript("return document.body.innerHTML");
            //string title1 = (string)js.ExecuteScript("return document.body.innerText");

            //driver1.FindElement(By.CssSelector("dynamic-root")).Click();
            //    SendKeys(Keys.Control + "S" + Keys.Control);
            System.Threading.Thread.Sleep(2000);

            //! Download Page and parse it to HtmlDocument
            //HtmlNodeCollection headlineTextDiv = pageDocument.DocumentNode.SelectNodes("(//div[contains(@id,'DynamicContentComponent31-group-48823.1')])");
            //foreach (HtmlNode node in headlineTextDiv)
            //{
            //    Console.WriteLine(node.OuterHtml);
            //}
            ////! Find All Matches
            //var headlineText = pageDocument.DocumentNode.SelectNodes("(//span[contains(@behavior.id,'ShowEvent')])");
            //foreach (HtmlNode node in headlineText)
            //{
            //    Console.WriteLine(node.OuterHtml);
            //    //! Step 2
            //    string behaviorID = node.GetAttributeValue("behavior.showevent.idfoevent", "Cant Fetch");
            //    Console.WriteLine(behaviorID);
            //    string onmorebetsclickedString = $"(//a[contains(@behavior.onmorebetsclicked.idfoevent,'{behaviorID}')])";
            //    var onmorebetsclicked = pageDocument.DocumentNode.SelectNodes(onmorebetsclickedString);
            //    foreach (HtmlNode nodeStep2 in onmorebetsclicked)
            //    {
            //        Console.WriteLine(nodeStep2.OuterHtml);
            //        //Step 3
            //        string nodesStep3String = $"(//input[contains(@behavior.selectionwithbetdatesclick.mtag,'E{behaviorID}|SCORE')])";
            //        var nodesStep3 = pageDocument.DocumentNode.SelectNodes(nodesStep3String);
            //        foreach (HtmlNode nodeStep3 in nodesStep3)
            //        {
            //            IWebDriver driver2 = new ChromeDriver();
            //            driver2.Navigate().GoToUrl("https://www.pamestoixima.gr/EN/1/sports#bo-navigation=16405.1&action=market-group-list&dynamic=17099.1");
            //            //string pageSource = driver2.PageSource;
            //            //Console.WriteLine(pageSource);
            //            Console.Read();

            //        }

            //    }
            //    Console.WriteLine("=======================================================");
            //}

            //Console.WriteLine(pageDocument.DocumentNode.InnerHtml);
            //MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

            //using (var context = new OddsContext())
            //{
            //    Companies cmp1 = context.Companies.FirstOrDefault(x => x.Id == 1);
            //}
        }

        #region Not Used


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

        async static void GetData()
        {
            //We will make a GET request to a really cool website...

            string baseUrl = "https://www.pamestoixima.gr/EN/1/sports#bo-navigation=16405.1&action=market-group-list&dynamic=16815.1";
            //The 'using' will help to prevent memory leaks.
            //Create a new instance of HttpClient
            using (HttpClient client = new HttpClient())

            //Setting up the response...         

            using (HttpResponseMessage res = await client.GetAsync(baseUrl))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {
                    Console.WriteLine(data);
                }
            }
        }
#endregion
    }
}
