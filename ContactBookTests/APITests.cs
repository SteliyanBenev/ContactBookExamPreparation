using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace ContactBookTests
{
    public class APITests
    {
        private const string url = "http://localhost:8080/api/contacts";
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            client = new RestClient();
        }

        [Test]
        public void Test_GetAllContacts_CheckFirstContact()
        {
            // Arrange
            request = new RestRequest(url);

            // Act 
            var response = client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            var expectedResult = "Steve Jobs";
            var actualResult = contacts[0].firstName + " " + contacts[0].lastName;

            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_SearchForContactAlbert_CheckFirstResult()
        {
            // Arrange
            request = new RestRequest(url + "/search/albert");

            // request = new RestRequest(url + "{keyword}");
            // request.AddUrlSegment("keyword", "albert");

            // Act 
            var response = client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            var expectedResult = "Albert Einstein";
            string actualResult = contacts[0].firstName + " " + contacts[0].lastName;

            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_SearchByMissingContact_CheckResultIsEmpty()
        {
            // Arrange

            request = new RestRequest(url + "/search/{keyword}");
            Random random = new Random();
            int number = random.Next();
            string keyword = "missing" + number;
            request.AddUrlSegment("keyword", keyword);

            // Act 
            var response = client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            // Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(contacts.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_CreateContactWithInvalidData()
        {
            // Arrange

            request = new RestRequest(url);
            var body = new 
            { 
                firstName = "Marie"
            };
            request.AddBody(body);

            // Act 
            var response = client.Execute(request, Method.Post);

            // Assert

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("{\"errMsg\":\"Last name cannot be empty!\"}", response.Content);
        }

        [Test]
        public void Test_CreateContactWithValidData_CheckContactExistsInContactList()
        {
            // Arrange

            request = new RestRequest(url);
            var body = new
            {
                firstName = "Konstantin" + DateTime.Now.Ticks.ToString(),
                lastName = "Benev" + DateTime.Now.Ticks.ToString(),
                email = DateTime.Now.Ticks.ToString() + "kkk@abv.bg",
                phone = "516654858" + DateTime.Now.Ticks.ToString(),
                comment = "no comment" + DateTime.Now.Ticks.ToString()
            };
            request.AddBody(body);

            // Act 
            var response = client.Execute(request, Method.Post);

            // Assert

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var contactsResponse = client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contact>>(contactsResponse.Content);

            var expectedResult = body.firstName + " " + body.lastName;
            var actualresult = contacts.Last().firstName + " " + contacts.Last().lastName;
            Assert.AreEqual(expectedResult, actualresult);
        }
    }
}