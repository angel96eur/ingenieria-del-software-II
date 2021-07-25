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

namespace Centro_de_estudios.UIT.Compras
{
    public class UCCompraMateriales_UIT : IDisposable
    {

        IWebDriver _driver;
        string _URI;

        void IDisposable.Dispose()
        {
            _driver.Close();
            _driver.Dispose();
        }

        public UCCompraMateriales_UIT()
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
            _URI = "https://localhost:44376/";
        }

        [Fact]
        public void initial_step_opening_the_web_page()
        {
            //Arrange
            string expectedTitle = "Home Page - Centro de estudios"; string expectedText = "Register";
            //Act
            //El navegador cargará la URI indicada
            _driver.Navigate().GoToUrl(_URI);
            //Assert
            //Comprueba que el título coincide con el esperado
            Assert.Equal(expectedTitle, _driver.Title);
            //Comprueba si la página contiene el string indicado
            Assert.Contains(expectedText, _driver.PageSource);
        }

        [Fact]
        public void precondition_perform_login()
        {

            _driver.Navigate()
                    .GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("francisco@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("APassword1234%");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }


        private void first_step_accessing_compras()
        {
            _driver.FindElement(By.Id("CompraController")).Click();

        }


        private void third_filter_materiales_byTitulo(string titleFilter)
        {
            _driver.FindElement(By.Id("materialTitulo")).SendKeys(titleFilter);

            _driver.FindElement(By.Id("filterbyTituloTipoMaterial")).Click();
        }


        private void third_filter_materiales_byTipoMaterial(string tipoSelected)
        {

            var tipoMaterial = _driver.FindElement(By.Id("tipomaterialSelected"));

            //create select element object 
            SelectElement selectElement = new SelectElement(tipoMaterial);
            //select Action from the dropdown menu
            selectElement.SelectByText(tipoSelected);

            _driver.FindElement(By.Id("filterbyTituloTipoMaterial")).Click();

        }

        private void fourth_select_materiales_and_submit()
        {

            _driver.FindElement(By.Id("Material_1")).Click();
            _driver.FindElement(By.Id("Material_2")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fourth_select_un_material_and_submit()
        {

            _driver.FindElement(By.Id("Material_2")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fourth_select_un_material_7__and_submit()
        {

            _driver.FindElement(By.Id("Material_7")).Click();

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fifth_fill_in_information_and_press_create_2(string deliveryAddress, string quantityMaterial1, string email, string prefix, string telefono)
        {

            _driver.FindElement(By.Id("DireccionEnvio")).SendKeys(deliveryAddress);

            _driver.FindElement(By.Id("Material_cantidadMaterial_7")).Clear();
            _driver.FindElement(By.Id("Material_cantidadMaterial_7")).SendKeys(quantityMaterial1);

            _driver.FindElement(By.Id("r12")).Click();

            _driver.FindElement(By.Id("email")).SendKeys(email);

            _driver.FindElement(By.Id("paypal_prefix")).SendKeys(prefix);

            _driver.FindElement(By.Id("paypal_phone")).Clear();
            _driver.FindElement(By.Id("paypal_phone")).SendKeys(telefono);

            _driver.FindElement(By.Id("CreateButton")).Click();


        }

        private void fourth_alternate_not_selecting_materiales()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void fifth_fill_in_information_and_press_create_un_material(string deliveryAddress, string quantityMaterial1)
        {

            _driver.FindElement(By.Id("DireccionEnvio")).SendKeys(deliveryAddress);

            _driver.FindElement(By.Id("Material_cantidadMaterial_2")).Clear();
            _driver.FindElement(By.Id("Material_cantidadMaterial_2")).SendKeys(quantityMaterial1);



            _driver.FindElement(By.Id("r11")).Click();



            _driver.FindElement(By.Id("CreateButton")).Click();


        }


        private void fifth_fill_in_information_and_press_create_1(string deliveryAddress, string quantityMaterial1, string email, string prefix, string telefono)
        {

            _driver.FindElement(By.Id("DireccionEnvio")).SendKeys(deliveryAddress);

            _driver.FindElement(By.Id("Material_cantidadMaterial_2")).Clear();
            _driver.FindElement(By.Id("Material_cantidadMaterial_2")).SendKeys(quantityMaterial1);

            _driver.FindElement(By.Id("r12")).Click();

            _driver.FindElement(By.Id("email")).SendKeys(email);

            _driver.FindElement(By.Id("paypal_prefix")).SendKeys(prefix);

            _driver.FindElement(By.Id("paypal_phone")).Clear();
            _driver.FindElement(By.Id("paypal_phone")).SendKeys(telefono);

            _driver.FindElement(By.Id("CreateButton")).Click();


        }


        [Fact]
        public void UC3_basic_flow()
        {
            //Arrange
            string[] expectedText = { "Details","Details",
                  "Compra","Francisco","Moreno", "PrecioTotal","1","Lapiz" };

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            fourth_select_un_material_and_submit();
            fifth_fill_in_information_and_press_create_1("Calle de la Universidad 1", "1", "fran@uclm.com", "111", "111111111");

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }


        [Fact]
        public void UC3_alternate_flow_1_NoMaterialesSelected()
        {
            //Arrange
            string expectedText = "Debes de seleccionar al menos un material";

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            fourth_alternate_not_selecting_materiales();


            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);
        }




        [Fact]
        public void UC3_alternate_flow_2_filteringbyTitle()
        {
            //Arrange
            string[] expectedText = { "Bic", "2,00 €", "Boligrafos" };

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            third_filter_materiales_byTitulo(expectedText[0]);

            var movieRow = _driver.FindElements(By.Id("Material_Titulo_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(movieRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(movieRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        public void UC3_alternate_flow_3_filteringbyTipoMaterial()
        {
            //Arrange
            string[] expectedText = { "Libreta de Rayas", "5,00 €", "Libreta" };

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            third_filter_materiales_byTitulo(expectedText[2]);

            var movieRow = _driver.FindElements(By.Id("Material_Titulo_" + expectedText[0]));

            //checks the expected row exists
            Assert.NotNull(movieRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(movieRow.First(l => l.Text.Contains(expected)));
        }



        [Fact]
        public void UC3_alternate_flow_4_noDireccionEnvio()
        {
            //Arrange
            string expectedText = "Please, set your address for delivery";

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            fourth_select_un_material_and_submit();
            fifth_fill_in_information_and_press_create_un_material("", "1");

            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Fact]
        public void UC3_alternate_flow_5_demasiadosMateriales()
        {
            //Arrange
            string expectedText = "No hay suficiente material titulado Gestion de Redes, por favor selecciones menos o igual que 11";

            //Act
            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            fourth_select_un_material_7__and_submit();
            fifth_fill_in_information_and_press_create_2("Calle de la Universidad 1", "100", "fran@uclm.com", "111", "111111111");

            //Assert
            var errorMessage = _driver.FindElements(By.TagName("li")).First(l => l.Text.Contains("No hay suficiente material titulado Gestion de Redes, por favor selecciones menos o igual que 11")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }



        [Fact]
        public void UC3_alternate_flow_6_NingunMateriales()
        {
            //Arrange
            string expectedText = "Por favor seleccione al menos un material a comprar o cancele compra";

            //Act
            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            fourth_select_un_material_and_submit();
            fifth_fill_in_information_and_press_create_1("Calle de la Universidad 1", "0", "fran@uclm.com", "111", "111111111");

            //Assert
            var errorMessage = _driver.FindElements(By.TagName("li")).First(l => l.Text.Contains("Por favor seleccione al menos un material a comprar o cancele compra")).Text;



            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        [InlineData("Calle de la Universidad 1", "2", "", "444", "111111111", "Please, set your email of paypal")]
        [InlineData("Calle de la Universidad 1", "2", "fran@uclm.com", "", "111111111", "Please, set your prefix of paypal")]
        [InlineData("Calle de la Universidad 1", "2", "fran@uclm.com", "444", "", "Please, set your phone of paypal")]
        [InlineData("Calle de la Universidad 1", "", "fran@uclm.com", "444", "1111111111", "Please, introduce una cantidad superior a 1")]

        public void UC3_alternate_flow_7_10_testingErrorsMandatorydata(string deliveryAddress, string quantityMovie1, string creditCardNumber, string CCV, string expirationDate, string expectedText)
        {

            //Act
            precondition_perform_login();
            first_step_accessing_compras();
            fourth_select_un_material_and_submit();
            fifth_fill_in_information_and_press_create_1(deliveryAddress, quantityMovie1, creditCardNumber, CCV, expirationDate);

            //Assert
            //the expected error is shown in the view
            var errorShown = _driver.FindElements(By.TagName("span")).FirstOrDefault(l => l.Text.Contains(expectedText));
            Assert.NotNull(errorShown);

        }

    }

}