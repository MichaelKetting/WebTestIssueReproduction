using System;
using System.IO;
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

      using (var driver = new ChromeDriver())
      {
        driver.Navigate().GoToUrl (GetTestPageUrl());
        var element = driver.FindElement (By.Name ("TextField"));
        for (int index = 0; index < input.Length; index += 2)
        {
          var sub = input.Substring (0, index);
          var data = sub;

          element.Clear();
          element.SendKeys (data);
          Assert.That (element.GetAttribute ("value"), Is.EqualTo (data));
        }
      }
    }

    [Test]
    public void Selenium_TestWithAllCharacters_UseOnlyWorkingCharacters_WorksMostTimes ()
    {
      var input = c_allWorkingCharacters;

      using (var driver = new ChromeDriver())
      {
        driver.Navigate().GoToUrl (GetTestPageUrl());
        var element = driver.FindElement (By.Name ("TextField"));
        for (int index = 0; index < input.Length; index += 2)
        {
          var sub = input.Substring (0, index);
          var data = sub;

          element.Clear();
          element.SendKeys (data);
          Assert.That (element.GetAttribute ("value"), Is.EqualTo (data));
        }
      }
    }

    [Test]
    public void Selenium_TestWithIndividualCharacters_IncludeBrokenCharacters_WorksSometimes ()
    {
      var input = c_allCharactersIncludingBrokenOnes;

      using (var driver = new ChromeDriver())
      {
        driver.Navigate().GoToUrl (GetTestPageUrl());
        var element = driver.FindElement (By.Name ("TextField"));
        for (int index = 0; index < input.Length; index += 2)
        {
          var character = input[index];
          var data = "x" + character + " ";

          element.Clear();
          element.SendKeys (data);
          Assert.That (element.GetAttribute ("value"), Is.EqualTo (data));
        }
      }
    }

    [Test]
    public void Coypu_TestWithAllCharacters_FailsSometimes ()
    {
      var input = c_allCharactersIncludingBrokenOnes;

      var sessionConfiguration = new SessionConfiguration { Browser = Browser.Chrome };
      sessionConfiguration.Driver = typeof (SeleniumWebDriver);
      using (var browserSession = new BrowserSession (sessionConfiguration))
      {
        browserSession.Visit (GetTestPageUrl());

        var element = browserSession.FindField ("TextField");
        var clearTextBox = Keys.Control + "a" + Keys.Control + Keys.Delete;
        var value = clearTextBox + input;

        element.SendKeys (value);
        Assert.That (element.Value, Is.EqualTo (input));
      }
    }

    [Test]
    public void Coypu_TestWithAllCharactersTwice_IncludeBrokenCharacters_FailsMostOfTheTime ()
    {
      var input = c_allCharactersIncludingBrokenOnes + c_allCharactersIncludingBrokenOnes;

      var sessionConfiguration = new SessionConfiguration { Browser = Browser.Chrome };
      sessionConfiguration.Driver = typeof (SeleniumWebDriver);
      using (var browserSession = new BrowserSession (sessionConfiguration))
      {
        browserSession.Visit (GetTestPageUrl());

        var element = browserSession.FindField ("TextField");
        var clearTextBox = Keys.Control + "a" + Keys.Control + Keys.Delete;
        var value = clearTextBox + input;

        element.SendKeys (value);
        Assert.That (element.Value, Is.EqualTo (input));
      }
    }

    [Test]
    public void Coypu_TestWithAllCharacters_UseOnlyWorkingCharacters_WorksMostTimes ()
    {
      var input = c_allWorkingCharacters;

      var sessionConfiguration = new SessionConfiguration { Browser = Browser.Chrome };
      sessionConfiguration.Driver = typeof (SeleniumWebDriver);
      using (var browserSession = new BrowserSession (sessionConfiguration))
      {
        browserSession.Visit (GetTestPageUrl());

        var element = browserSession.FindField ("TextField");
        var clearTextBox = Keys.Control + "a" + Keys.Control + Keys.Delete;
        var value = clearTextBox + input;

        element.SendKeys (value);
        Assert.That (element.Value, Is.EqualTo (input));
      }
    }


    private string GetTestPageUrl ()
    {
      return Path.GetFullPath ("WebSite/TestPage.html");
    }
  }
}