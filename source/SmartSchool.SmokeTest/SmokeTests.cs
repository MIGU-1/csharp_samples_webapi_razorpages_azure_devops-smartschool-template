using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace SmartSchool.SmokeTest
{
  [TestClass]
  public class SmokeTests
  {
    private TestContext _testContextInstance;
    private IWebDriver _driver;
    private string _webAppURL;

    [TestMethod]
    [TestCategory("Chrome")]
    public void DeployedSite_CallWithoutPage_IndexPageShouldBeReturned()
    {
      _driver.Navigate().GoToUrl(_webAppURL + "/");
      //_driver.FindElement(By.Id("sb_form_q")).SendKeys("Azure Pipelines");
      //_driver.FindElement(By.Id("sb_form_go")).Click();
      //_driver.FindElement(By.XPath("//ol[@id='b_results']/li/h2/a/strong[3]")).Click();
      Assert.IsTrue(_driver.Title.Contains("SmartSchool.Web"), "Verified title of the page");
    }

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get => _testContextInstance;
      set
      {
        _testContextInstance = value;
      }
    }

    [TestInitialize]
    public void SetupTest()
    {
      _webAppURL = (string)TestContext.Properties["WebAppUrl"]; ;

      string browser = "Chrome";
      switch (browser)
      {
        case "Chrome":
          _driver = new ChromeDriver();
          break;
        case "Firefox":
          _driver = new FirefoxDriver();
          break;
        case "IE":
          _driver = new InternetExplorerDriver();
          break;
        default:
          _driver = new ChromeDriver();
          break;
      }

    }

    [TestCleanup]
    public void MyTestCleanup()
    {
      _driver.Quit();
    }
  }
}
