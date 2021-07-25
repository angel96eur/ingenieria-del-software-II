using Centro_de_estudios.Controllers;
using Centro_de_estudios.Data;
using Centro_de_estudios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Centro_de_estudios.Models.CompraViewModels;
using Centro_de_estudios.Models.CompraViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Centro_de_estudios.UT.Controllers;



namespace Centro_de_estudios.UT.Controllers.ComprasController_test
{
    public class Compra_SelectMateriales_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext MaterialContext;

        public Compra_SelectMateriales_test()
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
        public async Task SelectMovie_Get_WithoutAnyFilter()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = MaterialContext;
                TipoMaterial tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                var tipomateriales = new List<TipoMaterial> { tipoMaterial };
                var expectedTiposMateriales = new SelectList(tipomateriales.Select(g => g.Nombre).ToList());


                var expectedMateriales = new Material[3] { new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" },
                                                           new Material { MaterialID = 2, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Saca Puntas"},
                                                           new Material { MaterialID = 3, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Boli azul" }};
                                                    




                // Act
                var result = controller.SelectMaterialesForCompra(null, null);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectMaterialesForPurchaseViewModel model = viewResult.Model as SelectMaterialesForPurchaseViewModel;

                Assert.Equal(expectedMateriales, model.Materiales, Comparer.Get<Material>((p1, p2) => p1.Titulo == p2.Titulo && p1.FechaLanzamiento == p2.FechaLanzamiento && p1.PrecioCompra == p2.PrecioCompra && p1.CantidadCompra == p2.CantidadCompra));
                Assert.Equal(expectedTiposMateriales, model.TipoMateriales, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

        [Fact]
        public async Task SelectMaterial_Get_WithFilterMaterialTitulo()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = MaterialContext;
                TipoMaterial tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                var tipomateriales = new List<TipoMaterial> { tipoMaterial };
                var expectedTipoMaterial = new SelectList(tipomateriales.Select(g => g.Nombre).ToList());

                var expectedMateriales = new Material[1] { new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" } };

                // Act
                var result = controller.SelectMaterialesForCompra("Goma", null);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectMaterialesForPurchaseViewModel model = viewResult.Model as SelectMaterialesForPurchaseViewModel;

                Assert.Equal(expectedMateriales, model.Materiales, Comparer.Get<Material>((p1, p2) => p1.Titulo == p2.Titulo && p1.FechaLanzamiento == p2.FechaLanzamiento && p1.PrecioCompra == p2.PrecioCompra && p1.CantidadCompra == p2.CantidadCompra));
                Assert.Equal(expectedTipoMaterial, model.TipoMateriales, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

        [Fact]
        public async Task SelectMaterial_Get_WithFilterGenre()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = MaterialContext;
                TipoMaterial tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                var tipoMaterials = new List<TipoMaterial> { tipoMaterial };
                var expectedtipos = new SelectList(tipoMaterials.Select(g => g.Nombre).ToList());


                var expectedMateriales = new Material[3] { new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" },
                                                           new Material { MaterialID = 2, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Saca Puntas" },
                                                           new Material { MaterialID = 3, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Boli azul" }};

                // Act
                var result = controller.SelectMaterialesForCompra(null, "Borrador");

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectMaterialesForPurchaseViewModel model = viewResult.Model as SelectMaterialesForPurchaseViewModel;

                Assert.Equal(expectedMateriales, model.Materiales, Comparer.Get<Material>((p1, p2) => p1.Titulo == p2.Titulo && p1.FechaLanzamiento == p2.FechaLanzamiento && p1.PrecioCompra == p2.PrecioCompra && p1.CantidadCompra == p2.CantidadCompra));
                Assert.Equal(expectedtipos, model.TipoMateriales, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

        [Fact]
        public async Task SelectMaterial_Post_MaterialesNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = MaterialContext;
                TipoMaterial tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
                var tipoMaterials = new List<TipoMaterial> { tipoMaterial };
                var expectedTipos = new SelectList(tipoMaterials.Select(g => g.Nombre).ToList());


                var expectedMateriales = new Material[3] { new Material {MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma"  },
                                                           new Material {MaterialID = 2, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Saca Puntas"},
                                                           new Material { MaterialID = 3, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Boli azul" }};

                SelectedMaterialesForCompraViewModel selected = new SelectedMaterialesForCompraViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectMaterialesForCompra(selected);

                //Assert
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectMaterialesForPurchaseViewModel model = viewResult.Model as SelectMaterialesForPurchaseViewModel;

                Assert.Equal(expectedMateriales, model.Materiales, Comparer.Get<Material>((p1, p2) => p1.Titulo == p2.Titulo && p1.FechaLanzamiento == p2.FechaLanzamiento && p1.PrecioCompra == p2.PrecioCompra && p1.CantidadCompra == p2.CantidadCompra));
                Assert.Equal(expectedTipos, model.TipoMateriales, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                // Check that both collections (expected and result returned) have the same elements with the same name
            }
        }

        [Fact]
        public async Task SelectMaterial_Post_MaterialesSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new ComprasController(context);
                controller.ControllerContext.HttpContext = MaterialContext;

                String[] ids = new string[1] { "1" };
                SelectedMaterialesForCompraViewModel materiales = new SelectedMaterialesForCompraViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectMaterialesForCompra(materiales);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentMateriales = viewResult.RouteValues.Values.First();
                Assert.Equal(materiales.IdsToAdd, currentMateriales);

            }
        }
    }
}
