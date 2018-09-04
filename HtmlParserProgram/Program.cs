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

namespace HtmlParserProgram
{
    class Program
    {
        static void Main(string[] args)
        {

            //GetData();

            //WebClient client1 = new WebClient();
            //client1.DownloadFile("http://www.nzherald.co.nz/", "C://Users//g.stavrou//Downloads//tes111t.html");

            //! Go to Home Page
            IWebDriver driver1 = new ChromeDriver(@"C:\Users\g.stavrou\Source\Repos\HtmlParserProgram\HtmlParserProgram\bin\Debug\netcoreapp2.1");
            driver1.Navigate().GoToUrl("https://www.pamestoixima.gr/EN/1/sports#action=sports");
            System.Threading.Thread.Sleep(4000);
            //! Get HTML Code
            string pageSource =  driver1.PageSource;

            //! Parse HTML
            HttpClient client = new HttpClient();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument = ParseHtmlPageSource(pageSource);
            HtmlNodeCollection htmlNodeCollection = FindSpecificElements(pageDocument , "(//ul[contains(@class,'nodes')])");
            foreach(HtmlNode node in htmlNodeCollection)
            {
                Console.WriteLine(IterateThroughHtmlNodes(node));

                if(node.ChildNodes.Count > 0)
                {
                    foreach(HtmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name.Equals("li"))
                        {
                            Console.WriteLine(node.Name);
                            HtmlAttributeCollection nodeAttributeCollection = childNode.Attributes;
                            foreach(HtmlAttribute attribute in nodeAttributeCollection)
                            {
                                Console.WriteLine(attribute.Value);
                            }
                        }
                    }
                }
            }


            //IJavaScriptExecutor js = (IJavaScriptExecutor)driver1;
            //string title = (string)js.ExecuteScript("return document.body.innerHTML");
            //string title1 = (string)js.ExecuteScript("return document.body.innerText");

            driver1.FindElement(By.CssSelector("dynamic-root")).Click();
            //    SendKeys(Keys.Control + "S" + Keys.Control);
            System.Threading.Thread.Sleep(2000);

            //driver1.Manage().Window.Maximize();
            //driver1.Navigate().GoToUrl($"view-source:{driver1.Url}");

            Actions action = new Actions(driver1);
            action.KeyDown(Keys.LeftControl)
                  .SendKeys("P")
                  //.SendKeys("c")
                  .Build()
                  .Perform();

            System.Threading.Thread.Sleep(10000);

            //! Download Page and parse it to HtmlDocument
            

            HtmlNodeCollection headlineTextDiv = pageDocument.DocumentNode.SelectNodes("(//div[contains(@id,'DynamicContentComponent31-group-48823.1')])");
            foreach (HtmlNode node in headlineTextDiv)
            {
                Console.WriteLine(node.OuterHtml);
            }
            //! Find All Matches
            var headlineText = pageDocument.DocumentNode.SelectNodes("(//span[contains(@behavior.id,'ShowEvent')])");
            foreach (HtmlNode node in headlineText)
            {
                Console.WriteLine(node.OuterHtml);
                //! Step 2
                string behaviorID = node.GetAttributeValue("behavior.showevent.idfoevent", "Cant Fetch");
                Console.WriteLine(behaviorID);
                string onmorebetsclickedString = $"(//a[contains(@behavior.onmorebetsclicked.idfoevent,'{behaviorID}')])";
                var onmorebetsclicked = pageDocument.DocumentNode.SelectNodes(onmorebetsclickedString);
                foreach (HtmlNode nodeStep2 in onmorebetsclicked)
                {
                    Console.WriteLine(nodeStep2.OuterHtml);
                    //Step 3
                    string nodesStep3String = $"(//input[contains(@behavior.selectionwithbetdatesclick.mtag,'E{behaviorID}|SCORE')])";
                    var nodesStep3 = pageDocument.DocumentNode.SelectNodes(nodesStep3String);
                    foreach (HtmlNode nodeStep3 in nodesStep3)
                    {
                        IWebDriver driver2 = new ChromeDriver();
                        driver2.Navigate().GoToUrl("https://www.pamestoixima.gr/EN/1/sports#bo-navigation=16405.1&action=market-group-list&dynamic=17099.1");
                        //string pageSource = driver2.PageSource;
                        //Console.WriteLine(pageSource);
                        Console.Read();

                    }

                }
                Console.WriteLine("=======================================================");
            }

            //Console.WriteLine(headlineText);



            //var headlineTextTest = pageDocument.DocumentNode.SelectNodes("(//a[contains(@behavior.onmorebetsclicked.idfoevent,'970424.1')])");

            //foreach (HtmlNode node in headlineTextTest)
            //{
            //    //Console.WriteLine(node.InnerHtml);
            //}

            //Console.WriteLine(headlineTextTest);


            ///


            var headlineTextTestAfter = pageDocument.DocumentNode.SelectNodes("(//input[contains(@behavior.selectionwithbetdatesclick.idfoevent,'970424.1')])");
            foreach (HtmlNode node in headlineTextTestAfter)
            {
                //Console.WriteLine(node.InnerHtml);
            }

            //Console.WriteLine(headlineTextTest);


            //Console.WriteLine(pageDocument.DocumentNode.InnerHtml);
            //MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static HtmlDocument ParseHtmlPageSource(string pageSource)
        {
            HttpClient client = new HttpClient();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageSource);

            return pageDocument;
        }

        private static HtmlNodeCollection FindSpecificElements(HtmlDocument pageDocument, string XPath)
        {
            HtmlNodeCollection htmlNodeCollections = new HtmlNodeCollection(null);
            htmlNodeCollections = pageDocument.DocumentNode.SelectNodes(XPath);
            
            return htmlNodeCollections;
        }

        private static string IterateThroughHtmlNodes(HtmlNode element)
        {
            HtmlNode node = null;
            node = element;

            return node.OuterHtml;
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
    }
}
