using System;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tests
{
  public class CustomChromeDriver : SeleniumWebDriver
  {
    public CustomChromeDriver (string logName)
        : base (CreateChromeDriver (logName), Browser.Chrome)
    {
    }

    private static IWebDriver CreateChromeDriver (string logName)
    {
      var driverService = ChromeDriverService.CreateDefaultService();
      driverService.EnableVerboseLogging = true;
      driverService.LogPath = string.Format ("{0}.ChromeDriver.log", logName);

      return new ChromeDriver (driverService, new ChromeOptions());
    }
  }
}