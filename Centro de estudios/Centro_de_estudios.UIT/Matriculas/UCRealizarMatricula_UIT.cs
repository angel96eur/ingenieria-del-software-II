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

namespace Centro_de_estudios.UIT.Matriculas
{
    public class UCRealizarMatricula_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;
        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }

        public UCRealizarMatricula_UIT()
        {
            //Opciones para cargar la página y aceptar certificados no seguros
            var optionsc = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
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
                .SendKeys("alvaro@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("APassword1234%");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }

        private void first_step_accessing_ImpartirAsignatura()
        {
            _driver.FindElement(By.Id("MatriculaController")).Click();

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

            _driver.FindElement(By.Id("Asignatura_20")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void last_select_asignaturas_and_submit()
        {

            _driver.FindElement(By.Id("Asignatura_27")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void last_fill_in_information_and_press_create(string deliveryAddress, string quantityAlumnos1, string email, string prefix, string telefono)
        {

            _driver.FindElement(By.Id("DireccionEnvio")).SendKeys(deliveryAddress);

            _driver.FindElement(By.Id("Asignatura_cantidadAsignatura_27")).Clear();
            _driver.FindElement(By.Id("Asignatura_cantidadAsignatura_27")).SendKeys(quantityAlumnos1);

            _driver.FindElement(By.Id("r12")).Click();

            _driver.FindElement(By.Id("email")).SendKeys(email);

            _driver.FindElement(By.Id("paypal_prefix")).SendKeys(prefix);

            _driver.FindElement(By.Id("paypal_phone")).Clear();
            _driver.FindElement(By.Id("paypal_phone")).SendKeys(telefono);

            _driver.FindElement(By.Id("CreateButton")).Click();


        }

        private void fourth_alternate_not_selecting_asignatura()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fifth_fill_in_information_and_press_create(string deliveryAddress, string quantityAlumnos1, string email, string prefix, string telefono)
        {

            _driver.FindElement(By.Id("DireccionEnvio")).SendKeys(deliveryAddress);

            _driver.FindElement(By.Id("Asignatura_cantidadAsignatura_20")).Clear();
            _driver.FindElement(By.Id("Asignatura_cantidadAsignatura_20")).SendKeys(quantityAlumnos1);

            _driver.FindElement(By.Id("r12")).Click();

            _driver.FindElement(By.Id("email")).SendKeys(email);

            _driver.FindElement(By.Id("paypal_prefix")).SendKeys(prefix);

            _driver.FindElement(By.Id("paypal_phone")).Clear();
            _driver.FindElement(By.Id("paypal_phone")).SendKeys(telefono);

            _driver.FindElement(By.Id("CreateButton")).Click();


        }

        [Fact]
        public void UC2_1_basic_flow()
        {
            //Arrange
            string[] expectedText = { "Details","Details",
                  "Matricula","Alvaro","Leon", "PrecioTotal","1","Sistemas operativos II" };

            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            second_select_asignaturas_and_submit();
            fifth_fill_in_information_and_press_create("Calle de la Universidad 1", "1", "alvaro@uclm.com", "111", "1234567");

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        [Fact]
        public void UC2_alternate_flow_1_NoAsignaturaSelected()
        {
            //Arrange
            string expectedText = "Debe seleccionar al menos una asignatura";

            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            fourth_alternate_not_selecting_asignatura();


            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);
        }

        [Fact]
        public void UC2_alternate_flow_2_filteringIntensificacion()
        {
            //Arrange
            string[] expectedText = { "Sistemas operativos II", "100,00 € ", "Computadores" };

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

        [Fact]
        public void UC2_alternate_flow_3_filterNombreAsignatura()
        {
            //filtramos por por el nombre de la asignatura
            //Arrange
            string[] expectedText = { "Sistemas operativos II", "100,00 € ", "Computadores" };

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
        public void UC2_alternate_flow_4_noDireccionEnvio()
        {
            //Arrange
            string expectedText = "Por favor, introduzca su direccion";

            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            second_select_asignaturas_and_submit();
            fifth_fill_in_information_and_press_create("", "1", "alvaro@uclm.com", "111", "1234567");

            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Fact]
        public void UC2_alternate_flow_5_demasiadosAlumnos()
        {
            //Arrange
            string expectedText = "No hay suficiente material titulado Multimedia, por favor selecciones menos o igual que 200";

            //Act
            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            last_select_asignaturas_and_submit();
            last_fill_in_information_and_press_create("Calle de la Universidad 1", "300", "alvaro@uclm.com", "111", "1234567");

            //Assert
            var errorMessage = _driver.FindElements(By.TagName("li")).First(l => l.Text.Contains("No hay suficiente material titulado Multimedia, por favor selecciones menos o igual que 200")).Text;


            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Fact]
        public void UC2_alternate_flow_6_NingunaPlaza()
        {
            //Arrange
            string expectedText = "Por favor seleccione al menos un material a comprar o cancele compra";

            //Act
            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            second_select_asignaturas_and_submit();
            fifth_fill_in_information_and_press_create("Calle de la Universidad 1", "0", "alvaro@uclm.com", "111", "1234567");

            //Assert
            var errorMessage = _driver.FindElements(By.TagName("li")).First(l => l.Text.Contains("Por favor seleccione al menos un material a comprar o cancele compra")).Text;



            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        [InlineData("Calle de la Universidad 1", "2", "", "444", "111111111", "Please, set your email of paypal")]
        [InlineData("Calle de la Universidad 1", "2", "alvaro@uclm.com", "", "111111111", "Please, set your prefix of paypal")]
        [InlineData("Calle de la Universidad 1", "2", "alvaro@uclm.com", "444", "", "Please, set your phone of paypal")]
        [InlineData("Calle de la Universidad 1", "", "alvaro@uclm.com", "444", "1111111111", "Please, introduce una cantidad superior a 1")]



        public void UC2_alternate_flow_7_10_testingErrorsMandatorydata(string deliveryAddress, string quantityAlumnos1, string creditCardNumber, string CCV, string expirationDate, string expectedText)
        {

            //Act
            precondition_perform_login();
            first_step_accessing_ImpartirAsignatura();
            second_select_asignaturas_and_submit();
            fifth_fill_in_information_and_press_create(deliveryAddress, quantityAlumnos1, creditCardNumber, CCV, expirationDate);

            //Assert
            //the expected error is shown in the view
            var errorShown = _driver.FindElements(By.TagName("span")).FirstOrDefault(l => l.Text.Contains(expectedText));
            Assert.NotNull(errorShown);

        }
    }
}
