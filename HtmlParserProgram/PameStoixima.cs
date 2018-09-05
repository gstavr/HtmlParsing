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

namespace HtmlParserProgram
{
    class PameStoixima
    {


        public string url = "https://www.pamestoixima.gr/EN/1/sports#action=sports";
        public IWebDriver driver1 = null;
        public GeneralMethods generalMethods = new GeneralMethods();
        public PameStoixima(string _pageUrl , IWebDriver _driver1)
        {
            this.url = _pageUrl;
            this.driver1 = _driver1;
            ParsePageData();
        }

        private void ParsePageData()
        {
            this.driver1.Navigate().GoToUrl(this.url);
            generalMethods.setTimeOut(4);
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
                        if (childNode.Name.Equals("li") && childNode.InnerHtml.Contains("Football"))
                        {
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
                                            }
                                        }
                                    }

                                    //Console.WriteLine(childNode2.InnerHtml);
                                }
                            }
                            //IWebElement li = driver1.FindElement(By.Id(attribute.Value));
                            //Console.WriteLine(childNode.InnerHtml);
                        }
                    }
                }

            }

        }

    }
}
