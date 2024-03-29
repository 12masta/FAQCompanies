using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;

namespace FaqCompanies.Tests;

[TestFixture]
public class BasicTests : PageTest
{
    private string _serverAddress;

    [SetUp]
    public void SetUp()
    {
        var sut = new CustomWebApplicationFactory();
        _serverAddress = sut.ServerAddress;
    }

    [Test]
    public async Task HomePageHasLoadedCorrectly()
    {
        await Page.GotoAsync(_serverAddress);

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Index"));
    }

    [Test]
    public async Task FaqHasLoadedCorrectly()
    {
        await Page.GotoAsync(_serverAddress + "faq-custom");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("FAQCompaniesChat"));
    }
}