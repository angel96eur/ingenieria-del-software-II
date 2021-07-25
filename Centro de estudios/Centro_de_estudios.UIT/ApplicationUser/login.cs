using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
//Necesario para obtener Find dentro de las ICollection o IList
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Centro_de_estudios.UIT.ApplicationUser
{
    public class UCLogin_UIT : IDisposable
    {

        IWebDriver _driver;
        string _URI;

        public UCLogin_UIT()
        {
            var optionsc = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            var optionsff = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };
            var optionsie = new InternetExplorerOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            //For pipelines use this option
            //It doesnot show the browser
            //optionsc.AddArgument("--headless");
            //optionsff.AddArgument("--headless");
            //optionsie.AddArgument("--headless");

           
           string browser = "Firefox";
            //string browser = "IE";
            switch (browser)
            {
                case "Chrome":
                    _driver = new ChromeDriver(optionsc);
                    break;
                case "Firefox":
                    _driver = new FirefoxDriver(optionsff);
                    break;
                //case "IE":
                //This driver is not working
                //    _driver = new InternetExplorerDriver(optionsie);
                //    break;
                default:
                    _driver = new ChromeDriver(optionsc);
                    break;
            }

            //Added to make ChromeDriver wait when an element is not found.
            //It will wait for a maximum of 50 seconds.
            //It has been added to wait for payment method options.
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
            //For pipelines this has to be set to 
            _URI = "https://localhost:44376/";

            initial_step_opening_the_web_page();

        }


        public void initial_step_opening_the_web_page()
        {
            _driver.Navigate()
                .GoToUrl(_URI);
        }

        [Fact]
        public void login_valid_attempt()
        {

            _driver.Navigate()
                    .GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("angel@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("Password1234%");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }


        [Fact]
        public void login_InvalidLoginAttemp()
        {


            _driver.Navigate()
                        .GoToUrl(_URI + "Identity/Account/Login");

            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("peter@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("nnn");

            _driver.FindElement(By.Id("login-submit"))
                .Click();


            var errorMessage = _driver.FindElements(By.TagName("li")).First(l => l.Text.Contains("Invalid")).Text;


            Assert.Equal("Invalid login attempt.", errorMessage);

        }


        [Theory]
        [InlineData("angel@uclm.com", "Passwofgrd1234%")]
        [InlineData("fran@uclm.com", "APasdswsord1234%")]
        [InlineData("alvaro@uclm.com", "APassword1g234%")]
        public void login_InvalidLoginAttemp_Data_Driven_With_InlineData(string email, string password)
        {

            //Arrange
            string expectedText = "Invalid login attempt.";

            //Act
            _driver.Navigate()
                        .GoToUrl(_URI + "Identity/Account/Login");

            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys(email);

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys(password);

            //It sleeps during 3 seconds so the acctions are visible
            Thread.Sleep(3000);

            _driver.FindElement(By.Id("login-submit"))
                .Click();


            //Assert
            var errorMessage = _driver.FindElements(By.TagName("li")).First(l => l.Text.Contains("Invalid")).Text;
            Assert.Equal(expectedText, errorMessage);

        }


        [Theory]
        [ClassData(typeof(LoginTestDataGenerator))]
        public void login_InvalidLoginAttemp_Data_Driven_With_ClassData(string email, string password)
        {
            //Arrange
            string expectedText = "Invalid login attempt.";

            //Act
            _driver.Navigate()
                        .GoToUrl(_URI + "Identity/Account/Login");

            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys(email);

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys(password);

            //It sleeps during 3 seconds so the acctions are visible
            Thread.Sleep(3000);

            _driver.FindElement(By.Id("login-submit"))
                .Click();


            //Assert
            var errorMessage = _driver.FindElements(By.TagName("li")).First(l => l.Text.Contains("Invalid")).Text;
            Assert.Equal(expectedText, errorMessage);

        }







        void IDisposable.Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }
    }
}
