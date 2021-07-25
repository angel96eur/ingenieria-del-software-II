using Centro_de_estudios.Controllers;
using Centro_de_estudios.Data;
using Centro_de_estudios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Centro_de_estudios.Models.CompraViewModels;
using Centro_de_estudios.UT.Controllers;


namespace Centro_de_estudios.UT.Controllers.ComprasController_test
{
   public class Compra_Create_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext MaterialContext;

        public Compra_Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDMaterialForTests(context);

            context.Users.Add(new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" });

            context.SaveChanges();

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("francisco@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            MaterialContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            MaterialContext.User = identity;

        }

        [Fact]
        public async Task Create_Get_WithSelectedMaterial()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = MaterialContext;

                String[] ids = new string[1] { "1" };
                SelectedMaterialesForCompraViewModel movies = new SelectedMaterialesForCompraViewModel() { IdsToAdd = ids };
                Material expectedMovie = new Material { MaterialID = 1, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" };

                Estudiante expectedCustomer = new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" };

                IList<CompraMaterial> expectedPurchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 1, Material = expectedMovie } };
                CompraCreateViewModel expectedPurchase = new CompraCreateViewModel { CompraMateriales = expectedPurchaseItems, Nombre = expectedCustomer.Nombre, PrimerApellido = expectedCustomer.PrimerApellido, SegundoApellido = expectedCustomer.SegundoApellido };

                // Act
                var result = controller.Create(movies);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;

                Assert.Equal(currentPurchase, expectedPurchase, Comparer.Get<CompraCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal(currentPurchase.CompraMateriales[0].Material, expectedPurchaseItems[0].Material, Comparer.Get<Material>((p1, p2) => p1.Titulo == p2.Titulo && p1.PrecioCompra == p2.PrecioCompra));

            }
            
        }

        [Fact]
        public async Task Create_Get_WithoutMaterial()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = MaterialContext;
                SelectedMaterialesForCompraViewModel selectedMateriales = new SelectedMaterialesForCompraViewModel();
                Estudiante expectedCustomer = new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" };

                CompraCreateViewModel expectedPurchase = new CompraCreateViewModel { Nombre = expectedCustomer.Nombre, PrimerApellido = expectedCustomer.PrimerApellido, SegundoApellido = expectedCustomer.SegundoApellido };
                // Act
                var result = controller.Create(selectedMateriales);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;
                var error = viewResult.ViewData.ModelState["MaterialNoSeleccionado"].Errors.FirstOrDefault();
                Assert.Equal(currentPurchase, expectedPurchase, Comparer.Get<CompraCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal($"Debes de seleccionar un material a comprar", error.ErrorMessage);
            }
        }

        [Fact]
        public async Task Create_Post_WithoutEnoughMoviesToBeComprado()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = MaterialContext;
                var tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                Material material = new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" };
                Estudiante estudiante = new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" };
                var payment1 = new CreditCard { CreditCardNumber = "1111111111111111", CCV = "111", ExpirationDate = new DateTime(2050, 10, 10) };


                IList<CompraMaterial> purchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 10, Material = material } };
                CompraCreateViewModel purchase = new CompraCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, CompraMateriales = purchaseItems, DireccionEnvio = "Albacete", MetodoPago = "CreditCard", CreditCard = payment1 };

                IList<CompraMaterial> expectedPurchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 10, Material = material } };
                CompraCreateViewModel expectedPurchase = new CompraCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, CompraMateriales = purchaseItems, DireccionEnvio = "Albacete", MetodoPago = "CreditCard", CreditCard = payment1 };


                // Act
                var result = controller.CreatePost(purchase, purchaseItems, payment1, null);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentPurchase, expectedPurchase, Comparer.Get<CompraCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal($"No hay suficiente material titulado Goma, por favor selecciones menos o igual que 5", error.ErrorMessage);
                Assert.Equal(currentPurchase.CompraMateriales[0].Material, expectedPurchaseItems[0].Material, Comparer.Get<Material>((p1, p2) => p1.Equals(p2)));



            }
            }


        [Fact]
        public async Task Create_Post_WithQuantity0ForCompra()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = MaterialContext;
                var tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                Material material = new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" };
                Estudiante estudiante = new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" };
                var payment1 = new CreditCard { CreditCardNumber = "1111111111111111", CCV = "111", ExpirationDate = new DateTime(2050, 10, 10) };


                IList<CompraMaterial> purchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 0, Material = material } };
                CompraCreateViewModel purchase = new CompraCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, CompraMateriales = purchaseItems, DireccionEnvio = "Albacete", MetodoPago = "CreditCard", CreditCard = payment1 };

                IList<CompraMaterial> expectedPurchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 0, Material = material } };
                CompraCreateViewModel expectedPurchase = new CompraCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, CompraMateriales = purchaseItems, DireccionEnvio = "Albacete", MetodoPago = "CreditCard", CreditCard = payment1 };


                // Act
                var result = controller.CreatePost(purchase, purchaseItems, payment1, null);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                CompraCreateViewModel currentPurchase = viewResult.Model as CompraCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentPurchase, expectedPurchase, Comparer.Get<CompraCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal($"Por favor seleccione al menos un material a comprar o cancele compra", error.ErrorMessage);
                Assert.Equal(currentPurchase.CompraMateriales[0].Material, expectedPurchaseItems[0].Material, Comparer.Get<Material>((p1, p2) => p1.Equals(p2)));

            }
        }



        [Fact]
        public async Task Create_Post_HavingEnoughQuantityWithPaypal()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = MaterialContext;
                var tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                Material movie = new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" };
                Estudiante customer = new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" };
                var payment1 = new PayPal { Email = "francisco@uclm.com", Phone = "967959595", Prefix = "+34" };

                IList<CompraMaterial> purchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 2, Material = movie } };
                CompraCreateViewModel purchase = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, CompraMateriales = purchaseItems, DireccionEnvio = "Albacete", MetodoPago= "PayPal", PayPal = payment1 };
               
                IList<CompraMaterial> expectedPurchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 2, Material = movie } };
                Compra expectedPurchase = new Compra { Estudiante = customer, CompraMateriales = expectedPurchaseItems, DeireccionDeEnvio = "Albacete", MetodoDePago = payment1, PrecioTotal = 24, FechaCompra = System.DateTime.Now };
                // Notice: the total price expected is 24, that is, 2 movies purchased for 12

                // Act
                var result = controller.CreatePost(purchase, purchaseItems, null, payment1);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

                //we should check the purchase has been created in the database
                var actualPurchase = context.Compra.Include(p => p.CompraMateriales).FirstOrDefault(p => p.CompraID == 1);
                //By checking its attributes
                Assert.Equal(actualPurchase, expectedPurchase, Comparer.Get<Compra>(
                                                  (p1, p2) => p1.PrecioTotal == p2.PrecioTotal
                                                             && p1.MetodoDePago.Equals(p2.MetodoDePago)
                                                             && p1.FechaCompra.Year == p2.FechaCompra.Year
                                                             && p1.FechaCompra.Month == p2.FechaCompra.Month
                                                             && p1.FechaCompra.Day == p2.FechaCompra.Day
                                                             && p1.Estudiante.UserName == p2.Estudiante.UserName
                                                             && p1.DeireccionDeEnvio == p2.DeireccionDeEnvio));
                //By checking its compunded attritbutes
                //PurchaseItems:
                Assert.Equal(actualPurchase.CompraMateriales, expectedPurchase.CompraMateriales, Comparer.Get<CompraMaterial>(
                                                   (i1, i2) => i1.Cantidad == i2.Cantidad
                                                             && i1.Material.Titulo == i2.Material.Titulo));
                //And that the movies associated to the quantity for purchase 
                //of each associated movie has been modified accordingly 
                for (int i = 0; i < actualPurchase.CompraMateriales.Count; i++)
                    Assert.Equal(context.Material.ToList()[i].CantidadCompra, expectedPurchaseItems[i].Material.CantidadCompra - purchaseItems[i].Cantidad);

            }

        }

        [Fact]
        public async Task Create_Post_HavingEnoughQuantityWithCreditCard()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = MaterialContext;
                var tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                Material movie = new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" };
                Estudiante customer = new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" };
                var payment1 = new CreditCard { CreditCardNumber = "1111111111111111", CCV = "111", ExpirationDate = new DateTime(2020, 10, 10) };

                IList<CompraMaterial> purchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 2, Material = movie } };
                CompraCreateViewModel purchase = new CompraCreateViewModel { Nombre = customer.Nombre, PrimerApellido = customer.PrimerApellido, SegundoApellido = customer.SegundoApellido, CompraMateriales = purchaseItems, DireccionEnvio = "Albacete", MetodoPago = "CreditCard", CreditCard = payment1 };

                IList<CompraMaterial> expectedPurchaseItems = new CompraMaterial[1] { new CompraMaterial { ID = 1, Cantidad = 2, Material = movie } };
                Compra expectedPurchase = new Compra { Estudiante = customer, CompraMateriales = expectedPurchaseItems, DeireccionDeEnvio = "Albacete", MetodoDePago = payment1, PrecioTotal = 24, FechaCompra = System.DateTime.Now };
                // Notice: the total price expected is 24, that is, 2 movies purchased for 12

                // Act
                var result = controller.CreatePost(purchase, purchaseItems, payment1, null);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

                //we should check the purchase has been created in the database
                var actualPurchase = context.Compra.Include(p => p.CompraMateriales).FirstOrDefault(p => p.CompraID == 1);
                //By checking its attributes
                Assert.Equal(actualPurchase, expectedPurchase, Comparer.Get<Compra>(
                                                  (p1, p2) => p1.PrecioTotal == p2.PrecioTotal
                                                             && p1.MetodoDePago.Equals(p2.MetodoDePago)
                                                             && p1.FechaCompra.Year == p2.FechaCompra.Year
                                                             && p1.FechaCompra.Month == p2.FechaCompra.Month
                                                             && p1.FechaCompra.Day == p2.FechaCompra.Day
                                                             && p1.Estudiante.UserName == p2.Estudiante.UserName
                                                             && p1.DeireccionDeEnvio == p2.DeireccionDeEnvio));
                //By checking its compunded attritbutes
                //PurchaseItems:
                Assert.Equal(actualPurchase.CompraMateriales, expectedPurchase.CompraMateriales, Comparer.Get<CompraMaterial>(
                                                   (i1, i2) => i1.Cantidad == i2.Cantidad
                                                             && i1.Material.Titulo == i2.Material.Titulo));
                //And that the movies associated to the quantity for purchase 
                //of each associated movie has been modified accordingly 
                for (int i = 0; i < actualPurchase.CompraMateriales.Count; i++)
                    Assert.Equal(context.Material.ToList()[i].CantidadCompra, expectedPurchaseItems[i].Material.CantidadCompra - purchaseItems[i].Cantidad);

            }

        }



    }
}
