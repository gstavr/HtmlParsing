using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IWebDriver driver = new ChromeDriver();
            Console.WriteLine("adada");
            driver.Navigate().GoToUrl("https://www.pamestoixima.gr/EN/1/sports#bo-navigation=16405.1&action=market-group-list&dynamic=17099.1");
            string pageSource = driver.PageSource;
            Console.WriteLine(pageSource);
            Console.Read();
        }
    }
}
