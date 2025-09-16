using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Automation_Assessment.UI
{
    [TestFixture]
    public class Tests
    {
        private IBrowser _browser;
        private IPage _page;
        private IBrowserContext _context;

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [Test]
        public async Task Given_When_Then()
        {
            // Arrange

            // Act

            // Assert
            Assert.Pass();
        }
    }
}