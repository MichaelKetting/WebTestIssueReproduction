using System;
using System.IO;
using System.Runtime.CompilerServices;
using Coypu;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tests
{
  [TestFixture]
  public class Test
  {
    private const string c_allCharactersIncludingBrokenOnes =
        "^ ° ! \" § $ % & / ( ) = ? ² ³ { [ ] } \\ + * ~ ' # @ < > | A Z a z 0 1 8 9 ";

    private const string c_allWorkingCharacters =
        "! $ % & / ( ) = ? { [ ] } \\ + * ~ ' # @ < > | A Z a z 0 1 8 9 ";

    [Test]
    public void Selenium_TestWithAllCharacters_IncludeBrokenCharacters_FailsMostOfTheTime ()
    {
      var input = c_allCharactersIncludingBrokenOnes;

      var driver = CreateChromeDriver();
      try
      {
        driver.Navigate().GoToUrl (GetTestPageUrl());
        var element = driver.FindElement (By.Name ("TextField"));

        Console.WriteLine();
        Console.WriteLine ("Started Chrome and loaded test page.");

        for (int index = 0; index < input.Length; index += 2)
        {
          var sub = input.Substring (0, index);
          var data = sub;

          element.Clear();
          element.SendKeys (data);
          var actual = element.GetAttribute ("value");

          Console.WriteLine ("Sent:     START_{0}_END", data);
          Console.WriteLine ("Received: START_{0}_END", actual);

          Assert.That (actual, Is.EqualTo (data));
        }
      }
      finally
      {
        driver.Dispose();
        Console.WriteLine ("Closed Chrome");
        Console.WriteLine();
      }
    }

    [Test]
    public void Selenium_TestWithAllCharacters_UseOnlyWorkingCharacters_WorksMostTimes ()
    {
      var input = c_allWorkingCharacters;

      var driver = CreateChromeDriver();
      try
      {
        driver.Navigate().GoToUrl (GetTestPageUrl());
        var element = driver.FindElement (By.Name ("TextField"));

        Console.WriteLine();
        Console.WriteLine ("Started Chrome and loaded test page.");

        for (int index = 0; index < input.Length; index += 2)
        {
          var sub = input.Substring (0, index);
          var data = sub;

          element.Clear();
          element.SendKeys (data);
          var actual = element.GetAttribute ("value");

          Console.WriteLine ("Sent:     START_{0}_END", data);
          Console.WriteLine ("Received: START_{0}_END", actual);

          Assert.That (actual, Is.EqualTo (data));
        }
      }
      finally
      {
        driver.Dispose();
        Console.WriteLine ("Closed Chrome");
        Console.WriteLine();
      }
    }

    [Test]
    public void Selenium_TestWithIndividualCharacters_IncludeBrokenCharacters_WorksSometimes ()
    {
      var input = c_allCharactersIncludingBrokenOnes;

      var driver = CreateChromeDriver();
      try
      {
        driver.Navigate().GoToUrl (GetTestPageUrl());
        var element = driver.FindElement (By.Name ("TextField"));

        Console.WriteLine();
        Console.WriteLine ("Started Chrome and loaded test page.");

        for (int index = 0; index < input.Length; index += 2)
        {
          var character = input[index];
          var data = "x" + character + " ";

          element.Clear();
          element.SendKeys (data);
          var actual = element.GetAttribute ("value");

          Console.WriteLine ("Sent:     START_{0}_END", data);
          Console.WriteLine ("Received: START_{0}_END", actual);

          Assert.That (actual, Is.EqualTo (data));
        }
      }
      finally
      {
        driver.Dispose();
        Console.WriteLine ("Closed Chrome");
        Console.WriteLine();
      }
    }

    [Test]
    public void Coypu_TestWithAllCharacters_FailsSometimes ()
    {
      var input = c_allCharactersIncludingBrokenOnes;

      var browserSession = CreateBrowserSession();
      try
      {
        browserSession.Visit (GetTestPageUrl());
        var element = browserSession.FindField ("TextField");

        Console.WriteLine();
        Console.WriteLine ("Started Chrome and loaded test page.");

        var clearTextBox = Keys.Control + "a" + Keys.Control + Keys.Delete;
        var value = clearTextBox + input;

        element.SendKeys (value);
        var actual = element.Value;

        Console.WriteLine ("Sent:     CTRL+a CTRL+DEL_{0}_END", input);
        Console.WriteLine ("Received:                _{0}_END", actual);

        Assert.That (actual, Is.EqualTo (input));
      }
      finally
      {
        browserSession.Dispose();
        Console.WriteLine ("Closed Chrome");
        Console.WriteLine();
      }
    }

    [Test]
    public void Coypu_TestWithAllCharactersTwice_IncludeBrokenCharacters_FailsMostOfTheTime ()
    {
      var input = c_allCharactersIncludingBrokenOnes + c_allCharactersIncludingBrokenOnes;

      var browserSession = CreateBrowserSession();
      try
      {
        browserSession.Visit (GetTestPageUrl());
        var element = browserSession.FindField ("TextField");

        Console.WriteLine();
        Console.WriteLine ("Started Chrome and loaded test page.");

        var clearTextBox = Keys.Control + "a" + Keys.Control + Keys.Delete;
        var value = clearTextBox + input;

        element.SendKeys (value);
        var actual = element.Value;

        Console.WriteLine ("Sent:     CTRL+a CTRL+DEL_{0}_END", input);
        Console.WriteLine ("Received:                _{0}_END", actual);

        Assert.That (actual, Is.EqualTo (input));
      }
      finally
      {
        browserSession.Dispose();
        Console.WriteLine ("Closed Chrome");
        Console.WriteLine();
      }
    }

    [Test]
    public void Coypu_TestWithAllCharacters_UseOnlyWorkingCharacters_WorksMostTimes ()
    {
      var input = c_allWorkingCharacters;

      var browserSession = CreateBrowserSession();
      try
      {
        browserSession.Visit (GetTestPageUrl());
        var element = browserSession.FindField ("TextField");

        Console.WriteLine();
        Console.WriteLine ("Started Chrome and loaded test page.");

        var clearTextBox = Keys.Control + "a" + Keys.Control + Keys.Delete;
        var value = clearTextBox + input;

        element.SendKeys (value);
        var actual = element.Value;

        Console.WriteLine ("Sent:     CTRL+a CTRL+DEL_{0}_END", input);
        Console.WriteLine ("Received:                _{0}_END", actual);

        Assert.That (actual, Is.EqualTo (input));

      }
      finally
      {
        browserSession.Dispose();
        Console.WriteLine ("Closed Chrome");
        Console.WriteLine();
      }
    }

    private ChromeDriver CreateChromeDriver ([CallerMemberName] string callingMethod = null)
    {
      var driverService = ChromeDriverService.CreateDefaultService();
      driverService.EnableVerboseLogging = true;
      driverService.LogPath = string.Format ("{0}.ChromeDriver.log", callingMethod);
      return new ChromeDriver (driverService, new ChromeOptions());
    }

    private static BrowserSession CreateBrowserSession ([CallerMemberName] string callingMethod = null)
    {
      var sessionConfiguration = new SessionConfiguration { Browser = Browser.Chrome };
      sessionConfiguration.Driver = typeof (CustomChromeDriver);
      var browserSession = new BrowserSession (sessionConfiguration, new CustomChromeDriver (callingMethod));
      return browserSession;
    }

    private string GetTestPageUrl ()
    {
      return Path.GetFullPath ("WebSite/TestPage.html");
    }
  }
}