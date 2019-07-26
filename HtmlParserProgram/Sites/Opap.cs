using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using OpenQA.Selenium;

namespace HtmlParserProgram.Sites
{
    class Opap
    {
        private IWebDriver driver1;
        private string link;
        public GeneralMethods generalMethods;
        public HtmlNodeCollection htmlNodeCollection = null;
        public Opap(IWebDriver driver1, string link)
        {
            this.driver1 = driver1;
            this.link = link;
            this.generalMethods = new GeneralMethods();
            this.LoadSite();
        }



        private void LoadSite()
        {
            driver1.Navigate().GoToUrl(this.link);
            generalMethods.setTimeOut(2);


            TimeSpan tm = new TimeSpan(0, 0, 8);
            driver1.Manage().Timeouts().ImplicitWait = tm;


            //string pageSource
            IWebElement table = this.driver1.FindElement(By.Id("coupon-table"));
            IList<IWebElement> row = table.FindElements(By.TagName("tr"));
            //int rowcount = row.Count();


            ReadOnlyCollection<IWebElement> e = table.FindElements(By.TagName("tr"));


            //! Parse HTML
            HttpClient client = new HttpClient();
            HtmlDocument pageDocument = new HtmlDocument();
            string pageSource = this.driver1.PageSource;
            pageSource = pageSource.Replace(Environment.NewLine, "").Replace("\t", "");
            pageDocument = generalMethods.ParseHtmlPageSource(pageSource);
            

            while (htmlNodeCollection == null)
            {
                htmlNodeCollection = generalMethods.FindSpecificElements(pageDocument, "//*[@id='coupon-table']");
            }




            //List<IWebElements> row = table.FindElements(By.TagName("tr"));
            //int rowcount = row.Count();

        }
    }
}
