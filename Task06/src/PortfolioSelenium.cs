#:package TUnit@1.43.11
#:package Selenium.WebDriver@4.43.0
#:package Selenium.Support@4.43.0

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

public sealed class PortfolioSiteTests
{
    private const string DefaultBaseUrl = "https://portfolio.yudashkin-dev.ru/";
    private const string DefaultRemoteUrl = "http://localhost:4444/wd/hub";

    [Test]
    public async Task UC01_HomePage_ShowsCoreSections()
    {
        using var session = BrowserSession.Create();
        session.Driver.Navigate().GoToUrl(BaseUrl);
        WaitForDocumentReady(session.Driver);

        var header = WaitForVisible(session.Driver, By.CssSelector("h1"));
        await Assert.That(header.Text.Contains("Юдашкин Олег Артемович", StringComparison.OrdinalIgnoreCase)).IsTrue();

        AssertSectionVisible(session.Driver, "Ключевые навыки");
        AssertSectionVisible(session.Driver, "Что ещё обо мне");
        AssertSectionVisible(session.Driver, "Опыт работы");
        AssertSectionVisible(session.Driver, "Контакты");
        AssertSectionVisible(session.Driver, "Интересные пет-проекты");
    }

    [Test]
    public async Task UC02_PetProjects_LinksOpenGitHubRepositories()
    {
        using var session = BrowserSession.Create();
        session.Driver.Navigate().GoToUrl(BaseUrl);
        WaitForDocumentReady(session.Driver);

        await VerifyExternalLink(session.Driver, "home-lab", "https://github.com/tokKurumi/home-lab");
        session.Driver.Navigate().Back();
        WaitForDocumentReady(session.Driver);

        await VerifyExternalLink(session.Driver, "FIAS-GAR", "https://github.com/tokKurumi/FIAS-GAR");
    }

    [Test]
    public async Task UC03_Contacts_LinksAreCorrect()
    {
        using var session = BrowserSession.Create();
        session.Driver.Navigate().GoToUrl(BaseUrl);
        WaitForDocumentReady(session.Driver);

        var contactSection = WaitForVisible(session.Driver, By.XPath("//h2[normalize-space()='Контакты']"));
        await Assert.That(contactSection.Displayed).IsTrue();

        var emailLink = WaitForVisible(session.Driver, By.CssSelector("a[href^='mailto:']"));
        var phoneLink = WaitForVisible(session.Driver, By.CssSelector("a[href^='tel:+79376710465']"));
        var reservePhoneLink = WaitForVisible(session.Driver, By.CssSelector("a[href^='tel:+79510576290']"));
        var telegramLink = WaitForVisible(session.Driver, By.CssSelector("a[href='https://t.me/tkurumi901']"));
        var gitHubLink = WaitForVisible(session.Driver, By.CssSelector("a[href='https://github.com/tokKurumi']"));

        await Assert.That(StartsWith(emailLink.GetAttribute("href"), "mailto:tkurumi901@gmail.com")).IsTrue();
        await Assert.That(StartsWith(phoneLink.GetAttribute("href"), "tel:+79376710465")).IsTrue();
        await Assert.That(StartsWith(reservePhoneLink.GetAttribute("href"), "tel:+79510576290")).IsTrue();
        await Assert.That(StartsWith(telegramLink.GetAttribute("href"), "https://t.me/tkurumi901")).IsTrue();
        await Assert.That(StartsWith(gitHubLink.GetAttribute("href"), "https://github.com/tokKurumi")).IsTrue();

        NavigateAndAssert(session.Driver, telegramLink.GetAttribute("href"), "https://t.me/tkurumi901");
        session.Driver.Navigate().Back();
        WaitForDocumentReady(session.Driver);
        NavigateAndAssert(session.Driver, gitHubLink.GetAttribute("href"), "https://github.com/tokKurumi");
    }

    [Test]
    public async Task UC04_UnknownPage_Returns404()
    {
        using var session = BrowserSession.Create();
        session.Driver.Navigate().GoToUrl(new Uri(new Uri(BaseUrl), "non-existent-page").AbsoluteUri);
        WaitForDocumentReady(session.Driver);

        var heading = WaitForVisible(session.Driver, By.XPath("//h1[normalize-space()='404 Not Found']"));
        await Assert.That(heading.Displayed).IsTrue();
        await Assert.That(session.Driver.Title.Contains("404 Not Found", StringComparison.OrdinalIgnoreCase)).IsTrue();
    }

    [Test]
    public async Task UC05_SkillsSection_HasExpectedTags()
    {
        using var session = BrowserSession.Create();
        session.Driver.Navigate().GoToUrl(BaseUrl);
        WaitForDocumentReady(session.Driver);

        AssertSectionVisible(session.Driver, "Ключевые навыки");
        await Assert.That(WaitForTextVisible(session.Driver, ".NET / .NET Framework").Displayed).IsTrue();
        await Assert.That(WaitForTextVisible(session.Driver, "Docker").Displayed).IsTrue();
        await Assert.That(WaitForTextVisible(session.Driver, "CI/CD").Displayed).IsTrue();
    }

    [Test]
    public async Task UC06_AboutSection_ContainsKeyStatements()
    {
        using var session = BrowserSession.Create();
        session.Driver.Navigate().GoToUrl(BaseUrl);
        WaitForDocumentReady(session.Driver);

        AssertSectionVisible(session.Driver, "Что ещё обо мне");
        await Assert.That(WaitForTextVisible(session.Driver, "Увлекаюсь домашней лабораторией из self-hosted сервисов.").Displayed).IsTrue();
        await Assert.That(WaitForTextVisible(session.Driver, "Изучаю практики DevOps и эксплуатацию инфраструктуры.").Displayed).IsTrue();
    }

    [Test]
    public async Task UC07_Contacts_EmailsAndPhonesVisibleAsText()
    {
        using var session = BrowserSession.Create();
        session.Driver.Navigate().GoToUrl(BaseUrl);
        WaitForDocumentReady(session.Driver);

        AssertSectionVisible(session.Driver, "Контакты");
        await Assert.That(WaitForTextVisible(session.Driver, "+7 937 671 04 65").Displayed).IsTrue();
        await Assert.That(WaitForTextVisible(session.Driver, "+7 951 057 62 90").Displayed).IsTrue();
        await Assert.That(WaitForTextVisible(session.Driver, "tkurumi901@gmail.com").Displayed).IsTrue();
    }

    private static string BaseUrl => EnsureTrailingSlash(Environment.GetEnvironmentVariable("PORTFOLIO_BASE_URL") ?? DefaultBaseUrl);

    private static string EnsureTrailingSlash(string value)
    {
        return value.EndsWith('/') ? value : value + "/";
    }

    private static bool StartsWith(string? value, string prefix)
    {
        return (value ?? string.Empty).StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
    }

    private static void NavigateAndAssert(IWebDriver driver, string? targetUrl, string expectedPrefix)
    {
        var url = targetUrl ?? string.Empty;
        driver.Navigate().GoToUrl(url);
        WaitForDocumentReady(driver);
        if (!driver.Url.StartsWith(expectedPrefix, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Unexpected navigation result. Expected '{expectedPrefix}', actual '{driver.Url}'.");
        }
    }

    private static void AssertSectionVisible(IWebDriver driver, string headingText)
    {
        var heading = WaitForVisible(driver, By.XPath($"//*[self::h2 or self::h3][normalize-space()='{headingText}']"));
        if (!heading.Displayed)
        {
            throw new InvalidOperationException($"Section '{headingText}' is not visible.");
        }
    }

    private static async Task VerifyExternalLink(IWebDriver driver, string linkText, string expectedUrl)
    {
        var link = WaitForVisible(driver, By.XPath($"//a[normalize-space()='{linkText}']"));
        var href = link.GetAttribute("href");
        await Assert.That(StartsWith(href, expectedUrl)).IsTrue();

        driver.Navigate().GoToUrl(href ?? string.Empty);
        WaitForDocumentReady(driver);
        await Assert.That(driver.Url.StartsWith(expectedUrl, StringComparison.OrdinalIgnoreCase)).IsTrue();
    }

    private static void WaitForDocumentReady(IWebDriver driver)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.Until(currentDriver =>
            string.Equals(
                ((IJavaScriptExecutor)currentDriver).ExecuteScript("return document.readyState")?.ToString(),
                "complete",
                StringComparison.OrdinalIgnoreCase));
    }

    private static IWebElement WaitForTextVisible(IWebDriver driver, string text)
    {
        return WaitForVisible(driver, By.XPath($"//*[normalize-space()='{text}']"));
    }

    private static IWebElement WaitForVisible(IWebDriver driver, By locator)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        return wait.Until(currentDriver =>
            currentDriver.FindElements(locator).FirstOrDefault(element => element.Displayed)
            ?? throw new WebDriverTimeoutException($"Timed out waiting for locator {locator}."));
    }

    private sealed class BrowserSession : IDisposable
    {
        private BrowserSession(IWebDriver driver)
        {
            Driver = driver;
        }

        public IWebDriver Driver { get; }

        public static BrowserSession Create()
        {
            var remoteUrl = Environment.GetEnvironmentVariable("SELENIUM_REMOTE_URL") ?? DefaultRemoteUrl;
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1440,1400");

            var driver = new RemoteWebDriver(new Uri(remoteUrl), options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(45);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

            return new BrowserSession(driver);
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
