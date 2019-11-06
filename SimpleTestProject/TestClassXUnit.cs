using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace SimpleTestProject
{
    public class SetupClass : IDisposable
    {
        public IWebDriver Driver { get; }

        public SetupClass()
        {
            var options = new ChromeOptions();
            options.AddArguments("--lang=en-GB");

            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
        }

        public void Dispose()
        {
            Driver.Quit();
        }
    }

    public class TestClassXUnit : IClassFixture<SetupClass>
    {
        public SetupClass setup;

        public TestClassXUnit(SetupClass setup)
        {
            this.setup = setup;
        }
 
        private const string URL = "https://www.google.com/";

        private const string SEARCH_FIELD_XPATH = "//input[@class = 'gLFyf gsfi']";
        private const string SEARCH_FIELD_CSS = "input[class = 'gLFyf gsfi']";

        private const string SEARCH_BUTTON_XPATH = "//div[@class = 'VlcLAe']//input[@class = 'gNO89b']";
        private const string SEARCH_BUTTON_CSS = "div[class = 'VlcLAe'] input[class = 'gNO89b']";

        private const string WIKI_RESULT_XPATH = "//h3[text()='XPath - Wikipedia']/..";
        private const string WIKI_RESULT_CSS = "div[class = 'r'] a[href $= 'wiki/XPath']";

        private const string CONTEXT_LIST_XPATH = "//div[@id= 'toc']/ul//a";
        private const string CONTEXT_LIST_CSS = "div[id= 'toc']>ul a";

        [Fact]
        public void Test()
        {
            setup.Driver.Navigate().GoToUrl(URL);

            var wait = new WebDriverWait(setup.Driver, new TimeSpan(0, 0, 5));
            wait.Until(drv => drv.FindElement(By.XPath(SEARCH_FIELD_XPATH)));

            setup.Driver.FindElement(By.CssSelector(SEARCH_FIELD_CSS)).SendKeys("xpath");

            var searchButton = setup.Driver.FindElement(By.CssSelector(SEARCH_BUTTON_CSS));
            var actions = new Actions(setup.Driver);
            actions.MoveToElement(searchButton).Perform();

            setup.Driver.FindElement(By.XPath(SEARCH_BUTTON_XPATH)).Click();

            wait.Until(drv => drv.FindElement(By.XPath(WIKI_RESULT_XPATH)));

            setup.Driver.FindElement(By.CssSelector(WIKI_RESULT_CSS)).Click();

            wait.Until(drv => drv.FindElement(By.ClassName("toctitle")));

            var allContextElements = setup.Driver.FindElements(By.CssSelector(CONTEXT_LIST_CSS));

            var contextListLength = allContextElements.Count;
            Assert.True(contextListLength == 36, $"Number of elements in context should be 36, but it is {contextListLength}");
        }
    }
}