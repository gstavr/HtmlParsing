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


namespace HtmlParserProgram
{
    class Program
    {
        static void Main(string[] args)
        {

            WebClient client1 = new WebClient();
            //File.Delete("C://Users//g.stavrou//Downloads//test.html");
            //client1.DownloadString("https://www.pamestoixima.gr/EN/1/sports", "C://Users//g.stavrou//Downloads//testAfterDL.html");
            //client1.DownloadFile("https://www.pamestoixima.gr/EN/1/sports", "C://Users//g.stavrou//Downloads//testAfterDL.html");

            IWebDriver driver1 = new ChromeDriver(@"C:\Users\g.stavrou\Source\Repos\HtmlParserProgram\HtmlParserProgram\bin\Debug\netcoreapp2.1");
            driver1.Navigate().GoToUrl("https://www.pamestoixima.gr/EN/1/sports#bo-navigation=16405.1&action=market-group-list&dynamic=17099.1");
            string t = driver1.PageSource;
            System.Threading.Thread.Sleep(4000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver1;
            string title = (string)js.ExecuteScript("return document.body.innerHTML");
            string title1 = (string)js.ExecuteScript("return document.body.innerText");
            
            driver1.FindElement(By.CssSelector("body")).Click();
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

            //string source = Clipboard.GetText(TextDataFormat.UnicodeText);
            //File.WriteAllText(@"PathToSaveTheSource", source);
            //
            System.Threading.Thread.Sleep(10000);
            //client1.DownloadFile("http://www.nzherald.co.nz/", "C://Users//g.stavrou//Downloads//tes111t.html");
            //string path = File.ReadAllText("C://Users//g.stavrou//Downloads//tes111t.html");
            string path = File.ReadAllText("C://Users//g.stavrou//Downloads//test.html");

            //! Download Page and parse it to HtmlDocument
            HttpClient client = new HttpClient();
            HtmlDocument pageDocument = new HtmlDocument();
            //pageDocument.LoadHtml(path);
            pageDocument.LoadHtml(title);

            var headlineTextDiv = pageDocument.DocumentNode.SelectNodes("(//div[contains(@id,'DynamicContentComponent31-group-48823.1')])");
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
                        string pageSource = driver2.PageSource;
                        Console.WriteLine(pageSource);
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
