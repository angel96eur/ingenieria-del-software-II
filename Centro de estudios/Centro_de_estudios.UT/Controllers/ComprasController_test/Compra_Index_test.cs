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
    public class Compra_Index_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext MaterialContext;
         
        public Compra_Index_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDMaterialForTests(context);

            context.Users.Add(new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" });

            context.SaveChanges();

            context.Compra.Add(new Compra { Estudiante = context.Estudiante.First(), EstudianteID = context.Estudiante.First().Id, DeireccionDeEnvio = "Avd. España s/n", MetodoDePago = new PayPal { Email = "francisco@uclm.com", Phone = "967959595", Prefix = "+34" }, FechaCompra = new DateTime(2018, 10, 18), PrecioTotal = 30, CompraMateriales = { } });
            //context.Purchase.Add(new Purchase {Customer = null, DeliveryAddress = "Avd. España s/n", PaymentMethod = new PayPal { Email = "peter@uclm.com", Phone = "967959595", Prefix = "+34" }, PurchaseDate = new DateTime(2018, 10, 18), TotalPrice = 30, PurchaseItems = { } });
            context.SaveChanges();

            foreach (var material in context.Material.ToList())
                context.CompraMaterial.Add(new CompraMaterial { Material = material, Compra = context.Compra.First(), Cantidad = 1 });

            context.SaveChanges();

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("francisco@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            MaterialContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            MaterialContext.User = identity;

        }

        [Fact]
        public async Task Index_Get()
        {
            using (context)
            {
                var tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                var materiales = new List<Material>();
                materiales.Add(new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" });
                materiales.Add(new Material { MaterialID = 2, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Saca Puntas" });
                materiales.Add(new Material { MaterialID = 3, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Boli azul" });

                var user = new Estudiante { UserName = "francisco@uclm.com", PhoneNumber = "967959595", Email = "francisco@uclm.com", Nombre = "Francisco", PrimerApellido = "Moreno", SegundoApellido = "Jimenez" };

                var compra = new Compra { Estudiante = context.Estudiante.First(), EstudianteID = context.Estudiante.First().Id, DeireccionDeEnvio = "Avd. España s/n", MetodoDePago = new PayPal { Email = "francisco@uclm.com", Phone = "967959595", Prefix = "+34" }, FechaCompra = new DateTime(2018, 10, 18), PrecioTotal = 30, CompraMateriales = { } };

                var items = new List<CompraMaterial>();
                foreach (var material in materiales)
                    items.Add(new CompraMaterial { Material = material, Compra = compra, Cantidad = 1 });

                compra.CompraMateriales = items;

                var expectedCompras = new List<Compra> { compra };

                context.SaveChanges();

                var controller = new ComprasController(context);

                // Another way to set up the User.Identity.Name
                /*controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{new Claim(ClaimTypes.Name, "peter@uclm.com")}, "Customer"))
                    }
                };*/

                controller.ControllerContext.HttpContext = MaterialContext;

                // Act
                var result = await controller.Index();

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as IEnumerable<Compra>;

                Assert.Equal(model, expectedCompras, Comparer.Get<Compra>((p1, p2) => p1.MetodoDePago.Equals(p2.MetodoDePago) && p1.FechaCompra == p2.FechaCompra && p1.PrecioTotal == p2.PrecioTotal && p1.DeireccionDeEnvio == p2.DeireccionDeEnvio && p1.Estudiante.Equals(p2.Estudiante)));
                foreach (var expectedPurchase in expectedCompras)
                    Assert.Equal(model.First().CompraMateriales, expectedPurchase.CompraMateriales, Comparer.Get<CompraMaterial>((p1, p2) => p1.Cantidad == p2.Cantidad && p1.Material.Equals(p2.Material)));
            }
        }
    }
}
