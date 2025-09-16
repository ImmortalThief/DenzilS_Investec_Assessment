using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace Automation_Assessment.UI
{
    [TestFixture]
    public class InvestecUiTest
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
        public async Task GivenUserVisitsInvestecSite_WhenTheySubscribeToInsights_ThenSuccessMessageIsShown()
        {
            // Arrange
            await _page.GotoAsync("https://www.investec.com/en_za.html");

            var cookiePopUp = _page.Locator("#onetrust-accept-btn-handler");
            if (await cookiePopUp.IsVisibleAsync())
            {
                await cookiePopUp.ClickAsync();
            }

            // Act
            await _page.ClickAsync("#search-toggle");

            await _page.FillAsync("#searchBarInput", "cash investment rates");
            await _page.PressAsync("input[type='search']", "Enter");

            await _page.ClickAsync("a[href*='understanding-interest-rates']");

            var signUpButton = _page.Locator("button:has-text('Sign up')");
            await signUpButton.ClickAsync();

            await _page.FillAsync("input[name=\"name\"]", "TestName");
            await _page.FillAsync("input[name=\"surname\"]", "TestSurname");
            await _page.FillAsync("input[name=\"email\"]", "test@investec.co.za");
            await _page.SelectOptionAsync("select[name='service']", "Savings");
            await _page.FillAsync("input[name='year_of_birth']", "1985");

            await _page.ClickAsync("button:has-text(\"Submit\")");

            // Assert
            var confirmationLocator = _page.Locator("text=We look forward to sharing out of the ordinary insights with you");
            await confirmationLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            var confirmationText = await confirmationLocator.InnerTextAsync();

            confirmationText.Should().Contain(
                "We look forward to sharing out of the ordinary insights with you",
                "the confirmation message should appear after subscribing"
            );
        }

        [TearDown]
        public async Task Teardown()
        {
            await _context.CloseAsync();
            await _browser.CloseAsync();
        }
    }
}