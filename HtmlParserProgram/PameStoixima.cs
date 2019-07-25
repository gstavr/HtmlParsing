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
using OpenQA.Selenium.Support.UI;

namespace HtmlParserProgram
{
    class PameStoixima
    {


        public string url = "https://www.pamestoixima.gr/EN/1/sports#action=sports";
        public IWebDriver driver1 = null;
        public GeneralMethods generalMethods = new GeneralMethods();
        public DataBase _dataBase = new DataBase();

        public HtmlNodeCollection htmlNodeCollection = null;

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

            pageSource = pageSource.Replace(Environment.NewLine, "").Replace("\t","");
            string getID = string.Empty;
            //! Parse HTML
            HttpClient client = new HttpClient();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument = generalMethods.ParseHtmlPageSource(pageSource);
                        
            while(htmlNodeCollection == null)
            {
                htmlNodeCollection = generalMethods.FindSpecificElements(pageDocument, "(//ul[contains(@class,'nodes')])");
            }

            //! Find FootBall Tab From HomePage
            foreach (HtmlNode node in htmlNodeCollection)
            {
                
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
            //generalMethods.setTimeOut(4);
            htmlNodeCollection = null;

            while(htmlNodeCollection == null)
            {
                pageSource = this.driver1.PageSource.Replace(System.Environment.NewLine, "").Replace("\t", "");
                htmlNodeCollection = generalMethods.FindSpecificElements(generalMethods.ParseHtmlPageSource(pageSource), "(//div[contains(@id,'DynamicContentComponent31-menu')])");
            }
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

                                                    //Competition competition = context.Competition.FirstOrDefault(x => x.Descr.Equals(childNode3.InnerText));
                                                    Competition competition = context.Competition.FirstOrDefault(x => x.DynamicId.Equals(nodeAttributeCollection["behavior.gotoleague.idfwbonavigation"].Value));

                                                    if (competition == default(Competition))
                                                    {
                                                        competition = new Competition();
                                                        context.Competition.Add(competition);
                                                        context.Database.CloseConnection();
                                                        competition.Id = _dataBase.X_getGID("Competition");
                                                        context.Database.OpenConnection();
                                                    }

                                                    competition.DateUpdated = DateTime.Now;
                                                    competition.SportId = 1;
                                                    competition.Descr = childNode3.InnerText;
                                                    competition.DynamicId = nodeAttributeCollection["behavior.gotoleague.idfwbonavigation"].Value;
                                                    
                                                    context.SaveChanges();

                                                    competitionGames(competition);
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

            //generalMethods.setTimeOut(2);
            //! Fill For Each Competition the Games
            
            
            
        }

        private void competitionGames(Competition competition)
        {
            using (OddsContext context = new OddsContext())
            {

                //generalMethods.setTimeOut(2);
                //! Go to Game URL (DYNAMIC)
                if (competition != default(Competition) && !string.IsNullOrWhiteSpace(competition.DynamicId))
                {
                    IWebElement liList = default(IWebElement);
                    
                    findElement(string.Format("//span[contains(@behavior.gotoleague.idfwbonavigation, '{0}')]", competition.DynamicId) , 0);
                    //WebDriverWait waitForElement = new WebDriverWait(driver1, TimeSpan.FromSeconds(5));
                    //waitForElement.Until(ExpectedConditions.ElementIsVisible(By.XPath(string.Format("//span[contains(@behavior.gotoleague.idfwbonavigation, '{0}')]", competition.DynamicId))));


                    while (liList == null)
                    {
                        liList = driver1.FindElement(By.XPath(string.Format("//span[contains(@behavior.gotoleague.idfwbonavigation, '{0}')]", competition.DynamicId)));
                    }

                    liList.Click();                   

                    htmlNodeCollection = null;
                    int counter = 0;
                    while (htmlNodeCollection == null)
                    {
                        string pgData = this.driver1.PageSource.Replace(System.Environment.NewLine, "").Replace("\t", "");
                        htmlNodeCollection = generalMethods.FindSpecificElements(generalMethods.ParseHtmlPageSource(pgData), "(//div[contains(@id,'DynamicContentComponent31-groups')])");
                        if(counter > 1000)
                            return;
                    }

                    HtmlDocument pageDocumentTable = new HtmlDocument();
                    HtmlNodeCollection htmlNodeCollection1 = null;
                    while (htmlNodeCollection1 == null)
                    {
                        pageDocumentTable = generalMethods.ParseHtmlPageSource(htmlNodeCollection[0].OuterHtml);
                        htmlNodeCollection1 = generalMethods.FindSpecificElements(pageDocumentTable, "(//table)");
                    }

                    foreach (HtmlNode collection in htmlNodeCollection1)
                    {
                        if (collection.ChildNodes.Count > 0)
                        {
                            foreach (HtmlNode childNode in collection.ChildNodes)
                            {
                                if (childNode.Name.Equals("tbody"))
                                {

                                    if (childNode.ChildNodes.Count > 0)
                                    {
                                        foreach (HtmlNode tr in childNode.ChildNodes)
                                        {
                                            // For Every Tr in tbody
                                            if (tr.Name.Equals("tr") && tr.ChildNodes.Count > 0)
                                            {

                                                Game game = new Game();
                                                game.CompetitionId = competition.Id;
                                                game.DateUpdated = DateTime.Now;
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

                                                        if (time.Contains("mins"))
                                                        {
                                                            game.MatchDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (DateTime.Now.Hour + 1), 0, 0);
                                                        }
                                                        else
                                                        {
                                                            game.MatchDate = new DateTime(DateTime.Now.Year, Convert.ToInt32(date.Split('/')[1]), Convert.ToInt32(date.Split('/')[0]), Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), 0);
                                                        }

                                                    }
                                                }

                                                string behaviorID = tr.LastChild.FirstChild.Attributes["behavior.more.id"].Value;
                                                string behaviorName = tr.LastChild.FirstChild.Attributes["behavior.more.id"].Name;

                                                game.DynamicId = behaviorID;

                                                //Game checkIfGameExists = context.Game.FirstOrDefault(x => x.CompetitionId == game.CompetitionId && x.MatchDate == game.MatchDate && x.HomeTeam.Equals(game.HomeTeam) && x.AwayTeam.Equals(game.AwayTeam));
                                                Game checkIfGameExists = context.Game.FirstOrDefault(x => x.CompetitionId == game.CompetitionId && x.DynamicId == behaviorID);
                                                if (checkIfGameExists == default(Game))
                                                {
                                                    game.Id = _dataBase.X_getGID("Game");
                                                    context.Game.Add(game);
                                                }
                                                else
                                                {
                                                    game.Id = checkIfGameExists.Id;
                                                }


                                                context.SaveChanges();

                                                IWebElement moreBetsPage = driver1.FindElement(By.XPath(string.Format("//span[@{1}='{0}' and @behavior.id ='More' and @title='More bets']", behaviorID, behaviorName)));
                                                while(moreBetsPage == null)
                                                {
                                                    moreBetsPage = driver1.FindElement(By.XPath(string.Format("//span[@{1}='{0}' and @behavior.id ='More' and @title='More bets']", behaviorID, behaviorName)));
                                                }

                                                moreBetsPage.Click();

                                                generalMethods.setTimeOut(2);
                                                Fill_GamePick(moreBetsPage, game);                                                                                              

                                                driver1.Navigate().Back();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                    driver1.Navigate().Back();
                }                             
                
            }
        }

        private void Fill_GamePick(IWebElement moreBetsPage, Game game)
        {   
            HtmlNodeCollection htmlNodeCollection = null;
            int countDiv = 0;
            while (htmlNodeCollection == null)
            {
                string pgData = this.driver1.PageSource.Replace(System.Environment.NewLine, "").Replace("\t", "");
                htmlNodeCollection = generalMethods.FindSpecificElements(generalMethods.ParseHtmlPageSource(pgData), string.Format("(//div[@id='MarketListContentComponent32-event-{0}-group-1'])", game.DynamicId));
            }

            if(htmlNodeCollection != null && htmlNodeCollection.Count > 0)
            {
                if(htmlNodeCollection[0].ChildNodes.Count > 0 && htmlNodeCollection[0].ChildNodes.Count == 1)
                {
                    countDiv = htmlNodeCollection[0].ChildNodes[0].ChildNodes.Count;
                    //behavior.id="ToggleMarket"
                    // \"toggler\" 
                    foreach (HtmlNode gamePick in htmlNodeCollection[0].ChildNodes[0].ChildNodes)
                    {

                        if(gamePick.ChildNodes.Count > 0)
                        {
                            if(gamePick.ChildNodes[0].ChildNodes.Count > 0)
                            {
                                using (OddsContext context = new OddsContext())
                                {
                                    GamePick gamePickRecord = new GamePick();
                                    // Foreach \"market\" class 
                                    foreach (HtmlNode market in gamePick.ChildNodes[0].ChildNodes)
                                    {   
                                        //HtmlNode lastNode = findLastChildNode(market);
                                        if (market.Name.Equals("h2"))
                                        {
                                            string GamePickDescr = market.FirstChild.InnerText;
                                            gamePickRecord = context.GamePick.FirstOrDefault(x => x.GameId == game.Id && x.Descr.Equals(GamePickDescr));
                                            if(gamePickRecord == default(GamePick))
                                            {
                                                gamePickRecord = new GamePick();
                                                context.GamePick.Add(gamePickRecord);
                                                gamePickRecord.Id = _dataBase.X_getGID("GamePick");
                                                gamePickRecord.GameId = game.Id;
                                            }                                         
                                           
                                            gamePickRecord.Descr = GamePickDescr;
                                            gamePickRecord.OddSumNum = 0;

                                            context.SaveChanges();
                                        }


                                        if (market.Name.Equals("table"))
                                        {
                                            foreach (HtmlNode tableNode in market.FirstChild.ChildNodes)
                                            {
                                                foreach (HtmlNode trNode in tableNode.ChildNodes)
                                                {
                                                    if(trNode.ChildNodes.Count > 0)
                                                    {
                                                        foreach (HtmlNode tdNode in trNode.FirstChild.ChildNodes)
                                                        {
                                                            gamePickRecord.OddSumNum++;
                                                            // Left Td String Value

                                                            string leftTd = tdNode.FirstChild.FirstChild.InnerText;
                                                            string rightTd = tdNode.FirstChild.LastChild.InnerText;
                                                            GamePickValue gamePickValue = context.GamePickValue.FirstOrDefault(x => x.Descr.Equals(leftTd) && x.GamePickId == gamePickRecord.Id);


                                                            if (gamePickValue == default(GamePickValue))
                                                            {
                                                                gamePickValue = new GamePickValue();
                                                                context.GamePickValue.Add(gamePickValue);
                                                                gamePickValue.Id = _dataBase.X_getGID("GamePickValue");
                                                                gamePickValue.PickValue = Convert.ToDouble(rightTd.Replace(',', '.'));
                                                            }
                                                            else
                                                            {
                                                                //gamePickValue.ChangedValue = Convert.ToDouble(rightTd.Replace(',', '.'));
                                                            }
                                                            gamePickValue.Descr = gamePickValue.AlternativeDescr = leftTd;
                                                            gamePickValue.OddsUpdated = DateTime.Now;
                                                            gamePickValue.GamePickId = gamePickRecord.Id;
                                                            gamePickValue.CompanyId = 1;
                                                            context.SaveChanges();
                                                        }
                                                    }
                                                    
                                                }
                                            }
                                        }                                        
                                    }
                                }
                            }
                        }

                        string t = gamePick.Name;
                    }
                }
            }

            using (OddsContext context = new OddsContext())
            {
                List<GamePick> gamePickRecord = context.GamePick.Where(x => x.GameId == game.Id).ToList();
                if (gamePickRecord.Count == countDiv)
                {
                    Console.WriteLine($"Game {game.Descr} has all records");
                }
            }
            
        }

        private IWebElement findElement(string xPath , int counter)
        {
            IWebElement liList = default(IWebElement);
            int _counter = counter;
            try
            {   
                liList = driver1.FindElement(By.XPath(xPath));
            }
            catch (Exception ex)
            {
                if(_counter < 3)
                {
                    generalMethods.setTimeOut(2);
                    findElement(xPath , ++_counter);
                }
                else
                {
                    generalMethods.setTimeOut(2);
                    liList = driver1.FindElement(By.XPath("(//span[contains(@id,'DynamicContentComponent31-tab-menu')])"));
                    liList.Click();                    
                    generalMethods.setTimeOut(5);
                    findElement(xPath,0);
                }
               
                
            }

            return liList;
        }


        private HtmlNode findLastChildNode(HtmlNode node)
        {

            if(node.ChildNodes.Count > 0 )
            {
                foreach(HtmlNode nodeInner in node.ChildNodes)
                {
                    return findLastChildNode(nodeInner);
                }
            }
            
            return node;
        }

    }
}
