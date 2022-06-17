using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace ContactBookSeleniumUITests
{
    public class SeleniumUITests
    {
        private const string url = "http://localhost:8080";
        private WebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void Test_ListOfContacts_CheckFirstContact()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            var contactsLink = driver.FindElement(By.LinkText("Contacts"));

            // Act
            contactsLink.Click();

            var firstName = driver.FindElement(By.CssSelector("a:nth-of-type(1) > .contact-entry  .fname > td")).Text; // $("#contact1 > tbody > tr.fname > td")
            var lastName = driver.FindElement(By.CssSelector("a:nth-of-type(1) > .contact-entry  .lname > td")).Text; // $("#contact1 > tbody > tr.lname > td")
            
            var actualResult = firstName + " " + lastName;
            var expectedResult = "Steve Jobs";

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_SearchForAlbert_CheckFirstContact()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            // Act
            var fieldKeyword = driver.FindElement(By.Id("keyword"));
            fieldKeyword.Clear();
            fieldKeyword.SendKeys("albert");

            driver.FindElement(By.Id("search")).Click();

            var firstName = driver.FindElement(By.CssSelector(".fname > td")).Text; 
            var lastName = driver.FindElement(By.CssSelector(".lname > td")).Text; 

            var actualResult = firstName + " " + lastName;
            var expectedResult = "Albert Einstein";

            // Assert
            //Assert.AreEqual("1 contacts found.", driver.FindElement(By.Id("searchResult")).Text);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_SearchForMissingContact_CheckResultIsEmpty()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            // Act
            var fieldKeyword = driver.FindElement(By.Id("keyword"));
            fieldKeyword.Clear();
            fieldKeyword.SendKeys("invalid2635");

            driver.FindElement(By.Id("search")).Click();
            
            // Assert
            Assert.AreEqual("No contacts found.", driver.FindElement(By.Id("searchResult")).Text);
        }

        [Test]
        public void Test_CreateContactWithInvalidData_CheckErrorIsShown()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            // Act
            var fieldFirstName = driver.FindElement(By.Id("firstName"));
            fieldFirstName.Clear();
            fieldFirstName.SendKeys("Konstantin");

            driver.FindElement(By.Id("create")).Click();

            // Assert
            Assert.AreEqual("Error: Last name cannot be empty!", driver.FindElement(By.CssSelector(".err")).Text);
        }

        [Test]
        public void Test_CreateContactWithValidData_CheckContactInContactList()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var firstName = "Konstantin" + DateTime.Now.Ticks;
            var lastName = "Benev" + DateTime.Now.Ticks;

            // Act
            var fieldFirstName = driver.FindElement(By.Id("firstName"));
            fieldFirstName.Clear();
            fieldFirstName.SendKeys(firstName);

            var fieldLastName = driver.FindElement(By.Id("lastName"));
            fieldLastName.Clear();
            fieldLastName.SendKeys(lastName);

            var fieldEmail = driver.FindElement(By.Id("email"));
            fieldEmail.Clear();
            fieldEmail.SendKeys(DateTime.Now.Ticks + "benev@abv.bg");

            var fieldPhone = driver.FindElement(By.Id("phone"));
            fieldPhone.Clear();
            fieldPhone.SendKeys("123456" + DateTime.Now.Ticks);

            var fieldComments = driver.FindElement(By.Id("comments"));
            fieldComments.Clear();
            fieldComments.SendKeys("Comment " + DateTime.Now.Ticks);

            driver.FindElement(By.Id("create")).Click();

            var contacts = driver.FindElements(By.CssSelector("body > main > div > a")).ToList();
            var actualResult = contacts.Last().FindElement(By.CssSelector("tr.fname > td")).Text + " " + contacts.Last().FindElement(By.CssSelector("tr.lname > td")).Text;
            var expectedResult = firstName + " " + lastName;

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}