using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Func<IWebDriver, bool> isSearchBoxEnabled =
           d =>
           {
               try
               {
                   IWebElement e = d.FindElement(By.CssSelector("div[class*=\"item-card\"]"));
                   return e.Displayed;
               }
               catch (Exception)
               {
                   return false;
               }
           };


            IConfiguration configuration = new ConfigurationBuilder()
       .AddJsonFile("appsettings.json", true, true)
       .Build();
            var settings = configuration.GetSection("Settings").Get<Settings>();
            var baseUrl = settings.BaseUrl;
            Console.WriteLine("Start Batch");

            IWebDriver driver = new ChromeDriver();
            var waitDriver = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            waitDriver.IgnoreExceptionTypes(typeof(NoSuchElementException));

            //foreach (var city in settings.Cities)
            var city = settings.Cities[0];
            Console.WriteLine("Start working on " + city);
            driver.Navigate().GoToUrl(baseUrl + city);

            //findElement potrebbe andare in eccezione
            //if(!waitDriver.Until(isSearchBoxEnabled))
            //{
            //    //ErrorLayout
            //    try
            //    {
            //        driver.FindElement(By.CssSelector("div[class^=\"ErrorLayout\"]"));
            //        //report --> non ci sono risultati per city 
            //    }
            //    catch (Exception ex)
            //    {
            //        //report --> cambiamento nel Dom
            //    }
            //}

            var annunci = driver.FindElements(By.CssSelector("div.item-card"));
            var lista = new List<string>();
            Thread.Sleep(TimeSpan.FromSeconds(5));

            foreach (var annuncio in annunci)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                var titolo = annuncio.FindElement(By.TagName("h2")).Text.ToString();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                lista.Add(titolo);

                //validazione titolo - conforme a regex
                var conformeRegex = true;

                if(conformeRegex)
                {
                    annuncio.Click();
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    //div[class*=\"item-card\"]
                    var stringId = driver.FindElement(By.CssSelector("span[class*=\"AdInfo_ad-info__id\"]")).Text.ToString();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    var id = Regex.Match(stringId, @"\d+").Value;

                    //descrizione annuncio
                    var descrizione = driver.FindElement(By.CssSelector("p[class*=\"AdDescription_description\"]")).Text.ToString();
                    
                }
                else
                {
                    //report del titolo annuncio + link 
                }
            }


            Console.WriteLine(string.Join("; ", lista));
            Console.ReadLine();
        }
    }
}
