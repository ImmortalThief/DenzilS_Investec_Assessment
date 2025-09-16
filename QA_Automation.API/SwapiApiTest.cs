using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json.Linq;

namespace Automation_Assesment.API
{
    [TestFixture]
    public class SwapiApiTest
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://swapi.dev/api/")
            };
        }

        [Test]
        public async Task GivenRequestMadeToSwapi_WhenNameIsR2D2_ThenSkinColourIsWhiteAndBlue()
        {
            // Arrange
            var response = await _client.GetAsync("people/");
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            // Act
            var results = json["results"] as JArray;
            var r2d2 = results?.FirstOrDefault(p => (string?)p["name"] == "R2-D2");

            // Assert
            using (new AssertionScope())
            {
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                results.Should().NotBeNull("Results should not be null");
                r2d2.Should().NotBeNull("R2-D2 should be present in the results");
                r2d2?["skin_color"]?.ToString().Should().Be("white, blue");
            }
        }

        [TearDown]
        public void Teardown()
        {
            _client.Dispose();
        }
    }
}