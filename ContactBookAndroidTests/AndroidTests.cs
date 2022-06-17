using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace ContactBookAndroidTests
{
    public class AndroidTests
    {
        private const string appiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string contactBookUrl = "https://contactbook.nakov.repl.co/api";
        private const string appLocation = @"C:\work\contactbook-androidclient.apk";

        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new AndroidDriver<AndroidElement>(new Uri(appiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void Test_SearchContact_CheckFirstResult()
        {
            // Arrange/Act
            var urlField = driver.FindElement(By.Id("contactbook.androidclient:id/editTextApiUrl"));
            urlField.Clear();
            urlField.SendKeys(contactBookUrl);

            driver.FindElement(By.Id("contactbook.androidclient:id/buttonConnect")).Click();

            var searchField = driver.FindElement(By.Id("contactbook.androidclient:id/editTextKeyword"));
            searchField.Clear();
            searchField.SendKeys("steve");

            var searchButton = driver.FindElement(By.Id("contactbook.androidclient:id/buttonSearch"));
            searchButton.Click();

            var actualFirstName = driver.FindElement(By.XPath(@"//android.widget.TableRow[3]/android.widget.TextView[2]")).Text;
            var actualLastName = driver.FindElement(By.XPath(@"//android.widget.TableRow[4]/android.widget.TextView[2]")).Text;

            // Assert
            Assert.AreEqual("Steve", actualFirstName);
            Assert.AreEqual("Jobs", actualLastName);
        }


    }
}