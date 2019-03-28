using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenCartTestMladenVarbev
{
    [TestClass]
    public class AdminPageTests
    {
        IWebDriver driver;

        [TestInitialize]
        public void TestSetup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            LoginAsAdmin();
        }

        [TestCleanup]
        public void TestTeardown()
        {
            driver.Quit();
        }

        [TestCategory("AdminTests")]
        [TestMethod]
        public void Test01LoginAsAdmin()
        {
            var loggedUserName = driver.FindElement(By.XPath("//a[contains(text(),'demo demo ')]"));

            Assert.AreEqual("demo demo", loggedUserName.Text);
        }

        [TestCategory("AdminTests")]
        [TestMethod]
        public void Test02LogoutAsAdmin()
        {
            var logoutButton = driver.FindElement(By.XPath("//*[@id='header']/div/ul/li[2]/a/span"));
            logoutButton.Click();

            Thread.Sleep(1000); ;

            var logoutPanelTitle = driver.FindElement(By.XPath("//*[@id='content']/div/div/div/div/div[1]/h1"));

            Assert.AreEqual("Please enter your login details.", logoutPanelTitle.Text);
        }

        [TestCategory("AdminTests")]
        [TestMethod]
        public void Test03DeclineNameChange()
        {
            var profileDropdown = driver.FindElement(By.XPath("//*[@id='header']/div/ul/li[1]/a"));

            profileDropdown.Click();

            Thread.Sleep(1000);

            var profileSettings = driver.FindElement(By.XPath("//*[@id='header']/div/ul/li[1]/ul/li[1]/a"));

            profileSettings.Click();

            Thread.Sleep(1000);

            var firstNameField = driver.FindElement(By.XPath("//*[@id='input-firstname']"));
            var lastNameField = driver.FindElement(By.XPath("//*[@id='input-lastname']"));

            firstNameField.Clear();
            firstNameField.SendKeys("Mladen");

            lastNameField.Clear();
            lastNameField.SendKeys("Varbev");

            Thread.Sleep(1000);

            var saveChanges = driver.FindElement(By.XPath("//*[@id='content']/div[1]/div/div/button"));
            saveChanges.Click();

            Thread.Sleep(1000);

            var throwWarningMessage = driver.FindElement(By.XPath("//*[@id='content']/div[2]/div[1]"));
            var expectedMessage = @"Warning: You do not have permission to modify your profile!
×";

            Assert.AreEqual(expectedMessage, throwWarningMessage.Text);
        }

        [TestCategory("AdminTests")]
        [TestMethod]
        public void Test04FilterOrdersByName()
        {
            var totalOrdersMenu = driver.FindElement(By.XPath("//*[@id='content']/div[2]/div[1]/div[1]/div/div[3]/a"));
            totalOrdersMenu.Click();

            Thread.Sleep(1000);

            var customerFilter = driver.FindElement(By.Id("input-customer"));

            Thread.Sleep(1000);

            customerFilter.Clear();
            customerFilter.SendKeys("Bob Smith");

            var filterButton = driver.FindElement(By.XPath("//*[@id='button-filter']/i"));

            filterButton.Click();

            Thread.Sleep(1000);

            var filteredCustomer = driver.FindElement(By.XPath("//*[@id='form-order']/table/tbody/tr[1]/td[3]"));

            Assert.AreEqual("Bob Smith", filteredCustomer.Text);
        }

        [TestCategory("AdminTests")]
        [TestMethod]
        public void Test05OpenSupportForum()
        {
            var profileDropDown = driver.FindElement(By.XPath("//*[@id='header']/div/ul/li[1]/a"));

            profileDropDown.Click();

            Thread.Sleep(1000);

            var supportForumRef = driver.FindElement(By.XPath("//*[@id='header']/div/ul/li[1]/ul/li[9]/a"));

            supportForumRef.Click();

            Thread.Sleep(1000);

            ReadOnlyCollection<string> windowHandles = driver.WindowHandles;

            string firstTab = windowHandles.First();
            string lastTab = windowHandles.Last();

            driver.SwitchTo().Window(lastTab);

            Thread.Sleep(1000);

            var forumBanner = driver.FindElement(By.XPath("//*[@id='forum']/div[1]/div/h1"));
            var expectedBanner = "Community Forum";

            Assert.AreEqual(expectedBanner, forumBanner.Text);
        }

        private void LoginAsAdmin()
        {
            driver.Navigate().GoToUrl(@"https://demo.opencart.com/admin/");

            var userName = driver.FindElement(By.Id("input-username"));
            var password = driver.FindElement(By.Id("input-password"));
            var loginButton = driver.FindElement(By.CssSelector("button.btn"));

            userName.Clear();
            userName.SendKeys("demo");

            password.Clear();
            password.SendKeys("demo");

            loginButton.Click();

            Thread.Sleep(2000);
        }
    }
}

//[TestCategory("OtherTests")]
//[TestMethod]
//public void Test00NavigateToDemoPage()
//{
//    driver.Navigate().GoToUrl(@"https://www.opencart.com");
//
//    Thread.Sleep(1000);
//
//    var viewDemoButton = driver.FindElement(By.XPath("//*[@id='hero']/div[1]/div[1]/div/p[2]/a[2]"));
//
//    Thread.Sleep(1000);
//
//    viewDemoButton.Click();
//
//    Thread.Sleep(1000);
//
//    var demoPageHeading = driver.FindElement(By.XPath("//*[@id='cms-demo']/div[1]/div/h1"));
//    var expectedPageHeadingText = "OpenCart Demonstration";
//
//    Assert.AreEqual(expectedPageHeadingText, demoPageHeading.Text);
//}