using System;
using System.Collections.Generic;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
//Needed to findelements in ICollection or IList 
using System.Linq;
using System.Threading;
using Xunit;

namespace Centro_de_estudios.UIT.Impartirs
{
    public class UCImpartirAsignaturas_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;
        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }

        public UCImpartirAsignaturas_UIT()
        {
            //Opciones para cargar la página y aceptar certificados no seguros
            var optionsc = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal, AcceptInsecureCertificates = true
            };

            //Instancia el controlador de firefox
            _driver = new FirefoxDriver(optionsc);

            //Tiempo máximo del controlador para cargar el servicio 
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            //URI de la aplicación
            _URI = "https://localhost:44376";
        }

        [Fact]
        public void initial_step_opening_the_web_page()
        {
            //Arrange
            string expectedTitle = "Home Page - Centro de estudios";
            string expectedText = "Register";

            //Act
            //El navegador cargará la URI indicada
            _driver.Navigate().GoToUrl(_URI);

            //Assert
            //Comprueba que el titulo coincide con el esperado
            Assert.Equal(expectedTitle, _driver.Title);
            //Comprueba si la pagina contiene el string indicado
            Assert.Contains(expectedText, _driver.PageSource);
        }

        public void precondition_perform_login()
        {

            _driver.Navigate()
                    .GoToUrl(_URI + "/Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("angel@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("Password1234%");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }

        private void first_step_accessing_ImpartirAsignatura()
        {
            _driver.FindElement(By.Id("ImpartirController")).Click();

        }

        private void second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("SelectAsignaturasForImpartir")).Click();
        }

        private void third_filter_asignaturas_porNombre(string titleFilter)
        {
            _driver.FindElement(By.Id("asignaturaNombre")).SendKeys(titleFilter);

            _driver.FindElement(By.Id("filterbyNombreIntensificacion")).Click();
        }

        private void third_filter_Asignatura_porIntensificacion(string intensificacionSelected)
        {

            var intensificacion = _driver.FindElement(By.Id("asignaturaIntensificacionSelected"));

            //create select element object 
            SelectElement selectElement = new SelectElement(intensificacion);
            //select Action from the dropdown menu
            selectElement.SelectByText(intensificacionSelected);

            _driver.FindElement(By.Id("filterbyNombreIntensificacion")).Click();

        }

        private void second_select_asignaturas_and_submit()
        {

            _driver.FindElement(By.Id("Asignatura_19")).Click();
            _driver.FindElement(By.Id("Asignatura_21")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fourth_alternate_not_selecting_asignatura()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fourth_fill_in_information_and_press_create(string quantityAsignatura1, string quantityAsignatura2)
        {

            _driver.FindElement(By.Id("Asignatura_cantidadAsignatura_19")).Clear();
            _driver.FindElement(By.Id("Asignatura_cantidadAsignatura_19")).SendKeys(quantityAsignatura1);

            _driver.FindElement(By.Id("Asignatura_cantidadAsignatura_21")).Clear();
            _driver.FindElement(By.Id("Asignatura_cantidadAsignatura_21")).SendKeys(quantityAsignatura2);

            _driver.FindElement(By.Id("CreateButton")).Click();
        }

        [Fact]
        public void UC1_1_basic_flow()
        {
            //Arrange
            string[] expectedText = { "Details","Details",
                "Impartir","Angel","Ortega", "mesesDocenciaTotal","6","Gestion de Redes","Seguridad de redes"};
            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            second_select_asignaturas_and_submit();
            fourth_fill_in_information_and_press_create("2", "2");

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        [Fact]
        public void UC1_2_alternative_flow()
        {
            //flujo alternativo donde no se seleccionan asignaturas
            //Arrange
            string expectedText = "Debes de seleccionar al menos una asignatura";

            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            fourth_alternate_not_selecting_asignatura();
            //Assert
            // var errorMessage = _driver.FindElements(By.TagName("li")).First(l => l.Text.Contains("You must select")).Text;

            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Fact]
        public void UC1_3_alternate_flow()
        {
            //la cantidad de meses indicados no son los suficientes
            //Arrange
            string expectedText = "You should select at least a Asignatura to be purchased, please";

            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            second_select_asignaturas_and_submit();
            fourth_fill_in_information_and_press_create("2", "0");
            //Assert
            var messageError = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, messageError.Substring(0, expectedText.Count()));

        }

        [Fact]
        public void UC1_4_alternate_flow_filterNombreAsignatura()
        {
            //filtramos por por el nombre de la asignatura
            //Arrange
            string[] expectedText = { "Gestion de Redes", "1", "Computadores" };

            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();

            third_filter_asignaturas_porNombre(expectedText[0]);

            var movieRow = _driver.FindElements(By.Id("Asignatura_Nombre_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(movieRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(movieRow.First(l => l.Text.Contains(expected)));

        }

        [Fact]
        public void UC1_5_alternate_flow_2_filteringIntensificacion()
        {
            //Arrange
            string[] expectedText = { "Gestion de Redes", "1", "Computadores" };

            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            third_filter_Asignatura_porIntensificacion(expectedText[2]);

            //Assert            
            var movieRow = _driver.FindElements(By.Id("Asignatura_Nombre_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(movieRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(movieRow.First(l => l.Text.Contains(expected)));

        }
    }
}
