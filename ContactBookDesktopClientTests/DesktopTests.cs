using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace ContactBookDesktopClientTests
{
    public class DesktopTests
    {
        private const string appiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string contactBookUrl = "https://contactbook.nakov.repl.co/api";
        private const string appLocation = @"C:\work\ContactBook-DesktopClient\ContactBook-DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(appiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void Test1()
        {
            // Arrange/Act
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(contactBookUrl);

            driver.FindElementByAccessibilityId("buttonConnect").Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var searchField = driver.FindElementByAccessibilityId("textBoxSearch");
            searchField.Clear();
            searchField.SendKeys("steve");

            var searchButton = driver.FindElementByAccessibilityId("buttonSearch");
            searchButton.Click();

            Thread.Sleep(5000);

            var actualFirstName = driver.FindElement(By.XPath("//Edit[@Name=\"FirstName Row 0, Not sorted.\"]")).Text;
            var actualLastName = driver.FindElement(By.XPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]")).Text;

            // Assert
            Assert.AreEqual("Steve", actualFirstName);
            Assert.AreEqual("Jobs", actualLastName);
        }
    }
}