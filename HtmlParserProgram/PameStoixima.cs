using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
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
using Microsoft.EntityFrameworkCore;

namespace HtmlParserProgram
{
    class PameStoixima
    {


        public string url = "https://www.pamestoixima.gr/EN/1/sports#action=sports";
        public IWebDriver driver1 = null;
        public GeneralMethods generalMethods = new GeneralMethods();
        public DataBase _dataBase = new DataBase();
        public PameStoixima(string _pageUrl , IWebDriver _driver1)
        {
            this.url = _pageUrl;
            this.driver1 = _driver1;
            ParsePageData();
        }

        private void ParsePageData()
        {
            this.driver1.Navigate().GoToUrl(this.url);
            generalMethods.setTimeOut(2);
            TimeSpan tm = new TimeSpan(0, 0, 8);
            driver1.Manage().Timeouts().ImplicitWait = tm;

            //! Get HTML Code
            string pageSource = this.driver1.PageSource;

            pageSource = pageSource.Replace(System.Environment.NewLine, "").Replace("\t","");
            string getID = string.Empty;
            //! Parse HTML
            HttpClient client = new HttpClient();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument = generalMethods.ParseHtmlPageSource(pageSource);
            HtmlNodeCollection htmlNodeCollection = generalMethods.FindSpecificElements(pageDocument, "(//ul[contains(@class,'nodes')])");
            
            //! Find FootBall Tab From HomePage
            foreach (HtmlNode node in htmlNodeCollection)
            {
                Console.WriteLine(generalMethods.IterateThroughHtmlNodes(node));

                if (node.ChildNodes.Count > 0)
                {
                    foreach (HtmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name.Equals("li") && childNode.InnerText.Equals("Football"))
                        {

                            foreach(HtmlNode aref in childNode.ChildNodes)
                            {
                                if(aref.Name == "a")
                                {
                                    if (!string.IsNullOrWhiteSpace(aref.Attributes["behavior.node.id"].Value)){
                                        using (OddsContext context = new OddsContext())
                                        {

                                            Companies companyFootball = context.Companies.FirstOrDefault(x => x.Link.Equals(this.url));

                                            if (companyFootball != default(Companies))
                                            {
                                                companyFootball.DynamicParam = aref.Attributes["behavior.node.id"].Value;
                                            }
                                            context.SaveChanges();
                                        }
                                    }
                                }
                            }

                            //! Find Football Link and Get ID
                            HtmlAttributeCollection nodeAttributeCollection = childNode.Attributes;
                            foreach (HtmlAttribute attribute in nodeAttributeCollection)
                            {
                                Console.WriteLine(childNode.InnerHtml);
                                if (attribute.Name.Equals("id") && attribute.Value.Contains("DynamicRootComponent"))
                                {   
                                    IWebElement li = driver1.FindElement(By.Id(attribute.Value));
                                    IWebElement liList = li.FindElement(By.XPath(string.Format("//a[contains(@behavior.node.id, '{0}')]", attribute.Value.Split('-')[1])));
                                    liList.Click();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //! Get Competition Page and Fill
            generalMethods.setTimeOut(4);
            pageSource = this.driver1.PageSource.Replace(System.Environment.NewLine, "").Replace("\t", "");  
            htmlNodeCollection = generalMethods.FindSpecificElements(generalMethods.ParseHtmlPageSource(pageSource), "(//div[contains(@id,'DynamicContentComponent31-menu')])");
            HtmlNode runningNode = null;
            foreach (HtmlNode node in htmlNodeCollection)
            {
                if(node.ChildNodes.Count > 0)
                {
                    foreach(HtmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name.Equals("div"))
                        {
                            HtmlNode ul = childNode.FirstChild;
                            foreach (HtmlNode childNode2 in ul.ChildNodes)
                            {
                                if (childNode2.Name.Equals("li"))
                                {
                                    //! Find Li Input and Spanm
                                    if(childNode2.ChildNodes.Count > 0)
                                    {
                                        foreach(HtmlNode childNode3 in childNode2.ChildNodes)
                                        {
                                            if (childNode3.Name.Equals("span"))
                                            {
                                                HtmlAttributeCollection nodeAttributeCollection = childNode3.Attributes;

                                                using (OddsContext context = new OddsContext())
                                                {

                                                    Competition competition = context.Competition.FirstOrDefault(x => x.Descr.Equals(childNode3.InnerText));

                                                    if(competition == default(Competition))
                                                    {
                                                        competition = new Competition();
                                                        context.Competition.Add(competition);
                                                        context.Database.CloseConnection();
                                                        competition.Id = _dataBase.X_getGID("Competition");
                                                        context.Database.OpenConnection();
                                                    }
                                                    
                                                    competition.SportId = 1;
                                                    competition.Descr = childNode3.InnerText;
                                                    competition.DynamicId = nodeAttributeCollection["behavior.gotoleague.idfwbonavigation"].Value;
                                                    
                                                    context.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //IWebElement li = driver1.FindElement(By.Id(attribute.Value));
                            //Console.WriteLine(childNode.InnerHtml);
                        }
                    }
                }

            }

            generalMethods.setTimeOut(2);
            //! Fill For Each Competition the Games
            
            
            using (OddsContext context = new OddsContext())
            {

                List<Competition> companyFootball = context.Competition.ToList();

                string CompetitionURL = this.driver1.Url;
                foreach (Competition comp in companyFootball)
                {
                    generalMethods.setTimeOut(2);
                    //! Go to Game URL (DYNAMIC)
                    if (!string.IsNullOrWhiteSpace(comp.DynamicId))
                    {
                        IWebElement liList = driver1.FindElement(By.XPath(string.Format("//span[contains(@behavior.gotoleague.idfwbonavigation, '{0}')]", comp.DynamicId)));
                        liList.Click();
                        //string gameURl = string.Format(CompetitionURL + "&dynamic={0}", comp.DynamicId);
                        //this.driver1.Navigate().GoToUrl(this.url);
                        generalMethods.setTimeOut(2);
                        string pgData = this.driver1.PageSource.Replace(System.Environment.NewLine, "").Replace("\t", "");

                        htmlNodeCollection = generalMethods.FindSpecificElements(generalMethods.ParseHtmlPageSource(pgData), "(//div[contains(@id,'DynamicContentComponent31-groups')])");
                        runningNode = null;
                        HtmlDocument pageDocumentTable = new HtmlDocument();
                        pageDocumentTable = generalMethods.ParseHtmlPageSource(htmlNodeCollection[0].OuterHtml);
                        HtmlNodeCollection htmlNodeCollection1 = generalMethods.FindSpecificElements(pageDocumentTable, "(//table)");

                        foreach(HtmlNode collection in htmlNodeCollection1)
                        {
                            if(collection.ChildNodes.Count > 0)
                            {
                                foreach(HtmlNode childNode in collection.ChildNodes)
                                {
                                    if(childNode.Name.Equals("tbody"))
                                    {

                                        if(childNode.ChildNodes.Count > 0)
                                        {
                                            foreach(HtmlNode tr in childNode.ChildNodes)
                                            {
                                                // For Every Tr in tbody
                                                if (tr.Name.Equals("tr") && tr.ChildNodes.Count > 0)
                                                {

                                                    Game game = new Game();
                                                    game.CompetitionId = comp.Id;
                                                    foreach (HtmlNode td in tr.ChildNodes)
                                                    {
                                                        if (td.HasClass("eventname"))
                                                        {
                                                            string date = td.FirstChild.FirstChild.FirstChild.InnerText;
                                                            string time = td.FirstChild.FirstChild.LastChild.InnerText;

                                                            string MatchHomeTeam = td.LastChild.FirstChild.FirstChild.InnerText;
                                                            string AwayTeam = td.LastChild.LastChild.LastChild.InnerText;

                                                            game.Descr = string.Format("{0} - {1}", MatchHomeTeam, AwayTeam);
                                                            game.HomeTeam = MatchHomeTeam;
                                                            game.AwayTeam = AwayTeam;
                                                            game.MatchDate = new DateTime(DateTime.Now.Year, Convert.ToInt32(date.Split('/')[1]), Convert.ToInt32(date.Split('/')[0]), Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), 0);
                                                        }
                                                    }

                                                    Game checkIfGameExists = context.Game.FirstOrDefault(x => x.CompetitionId == game.CompetitionId && x.MatchDate == game.MatchDate && x.HomeTeam.Equals(game.HomeTeam) && x.AwayTeam.Equals(game.AwayTeam));
                                                    if (checkIfGameExists == default(Game))
                                                    {
                                                        game.Id = _dataBase.X_getGID("Game");
                                                        context.Game.Add(game);
                                                    }
                                                    else
                                                    {
                                                        checkIfGameExists = game;
                                                    }

                                                    context.SaveChanges();

                                                    string getXPath = tr.LastChild.FirstChild.XPath;
                                                    string behaviorID = tr.LastChild.FirstChild.Attributes["behavior.more.id"].Value;
                                                    string behaviorName = tr.LastChild.FirstChild.Attributes["behavior.more.id"].Name;
                                                    IWebElement moreBetsPage = driver1.FindElement(By.XPath(string.Format("//span[contains(@{1}, '{0}')]", behaviorID , behaviorName)));
                                                    //liList.Click();
                                                    generalMethods.setTimeOut(2);

                                                    //driver1.Navigate().Back();
                                                }
                                            }
                                        }


                                        
                                    }
                                }
                            }
                        }

                        driver1.Navigate().Back();

                    }
                }
                context.SaveChanges();
            }
        }

    }
}
