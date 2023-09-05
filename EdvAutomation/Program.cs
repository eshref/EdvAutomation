﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using SeleniumExtras.WaitHelpers;

namespace EdvAutomation;

public static class Program
{
    private static bool loggedIn = false;

    static void Main(string[] args)
    {
        // Set the path to the Chrome WebDriver executable
        var driverPath = @"C:\Users\asafarov\Downloads\chromedriver-win64\chromedriver.exe";

        Console.WriteLine("started processing...");

        // Create a ChromeDriver instance
        using var driver = new ChromeDriver(driverPath);

        // List of example URLs
        var urls = File.ReadAllLines("edvlist.txt");

        var notAddedEdvs = new List<string>();

        foreach (var url in urls)
        {
            try
            {
                Program.processUrl(url, driver);
            }
            catch (Exception e)
            {
                notAddedEdvs.Add(url);

                Console.WriteLine($"An error occurred while processing url. Url: {url}. Exception: {e.Message}");
            }
        }

        File.WriteAllLines("notAddedEdvs.txt", notAddedEdvs);

        Console.WriteLine("finished processing.");
    }

    private static void processUrl(string url, ChromeDriver driver)
    {
        if (!loggedIn)
        {
            Program.login(driver);
        }

        driver.Navigate().GoToUrl("https://edvgerial.kapitalbank.az/az/dashboard");

        // Extract ID from the URL
        var docId = url.Split("?doc=")[1];

        // Locate and fill the ID input field by class name
        var idInput = driver.FindElement(By.ClassName("fiscal-id"));
        idInput.Clear();
        idInput.SendKeys(docId);

        // Locate and click the "Search" button by class name
        var searchButton = driver.FindElement(By.ClassName("submit-receipt"));
        searchButton.Click();

        // Wait for the popup to appear by class name
        var popup = new WebDriverWait(driver, TimeSpan.FromSeconds(30))
            .Until(ExpectedConditions.ElementIsVisible(By.ClassName("add-receipt-popup")));

        // Click the button within the popup by class name
        var popupButton = popup.FindElement(By.ClassName("submit"));
        popupButton.Click();

        Thread.Sleep(TimeSpan.FromSeconds(5));
    }

    private static void login(ChromeDriver driver)
    {
        // Navigate to the target website
        driver.Navigate().GoToUrl("https://edvgerial.kapitalbank.az/az/dashboard");

        // login
        var mobileTextBox = driver.FindElement(By.Id("mobile"));
        mobileTextBox.Clear();
        mobileTextBox.SendKeys("***");

        var passwordTextBox = driver.FindElement(By.Id("password"));
        passwordTextBox.Clear();
        passwordTextBox.SendKeys("***");

        var loginButton = driver.FindElement(By.ClassName("login"));
        loginButton.Click();

        loggedIn = true;

        Thread.Sleep(TimeSpan.FromSeconds(2));
    }
}