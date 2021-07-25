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
using Centro_de_estudios.Models.ImpartirViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Centro_de_estudios.UT.Controllers;

namespace Centro_de_estudios.UT.Controller.ImpartirsController_test
{
    public class Impartir_SelectAsignaturasForImpartir_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext impartirContext;

        public Impartir_SelectAsignaturasForImpartir_test()
        {
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            Utilities.InitializeDAsignaturasForTests(context);

            context.Users.Add(new Profesor { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });

            context.SaveChanges();

            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            impartirContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            impartirContext.User = identity;
        }

        [Fact]
        public async Task SelectAsignatura_Get_WithoutAnyFilter()
        {
            using (context)
            {

                // Arrange
                var controller = new ImpartirsController(context);
                controller.ControllerContext.HttpContext = impartirContext;
                Intensificacion intensificacion = new Intensificacion { NombreIntensificacion = "Optativa" };
                var intensificaciones = new List<Intensificacion> { intensificacion };
                var expectedIntensificaciones = new SelectList(intensificaciones.Select(g => g.NombreIntensificacion).ToList());


                var expectedAsignaturas = new Asignatura[3] { new Asignatura {AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200 },
                                        new Asignatura { AsignaturaID = 2, NombreAsignatura = "Sistemas Empotrados", Intensificacion = intensificacion, FechaComienzo = new DateTime(2001, 05, 10), MinimoMesesDocencia = 1, Precio = 150},
                                       new Asignatura{ AsignaturaID = 3, NombreAsignatura = "API", Intensificacion = intensificacion, FechaComienzo = new DateTime(2007, 01, 02), MinimoMesesDocencia = 4, Precio = 100}};




                // Act
                var result = controller.SelectAsignaturasForImpartir(null, null);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectAsignaturasForImpartirViewModel model = viewResult.Model as SelectAsignaturasForImpartirViewModel;

                Assert.Equal(expectedAsignaturas, model.Asignaturas, Comparer.Get<Asignatura>((p1, p2) => p1.NombreAsignatura == p2.NombreAsignatura  && p1.MinimoMesesDocencia == p2.MinimoMesesDocencia));
                Assert.Equal(expectedIntensificaciones, model.Intensificaciones, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

        [Fact]
        public async Task SelectAsignatura_Get_WithFilterAsignaturaNombre()
        {
            using (context)
            {
                // Arrange
                var controller = new ImpartirsController(context);
                controller.ControllerContext.HttpContext = impartirContext;
                Intensificacion intensificacion = new Intensificacion { NombreIntensificacion = "Optativa" };
                var intensificaciones = new List<Intensificacion> { intensificacion };
                var expectedIntensificaciones = new SelectList(intensificaciones.Select(g => g.NombreIntensificacion).ToList());

                var expectedAsignaturas = new Asignatura[1] { new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200 } };
                                       
                // Act
                var result = controller.SelectAsignaturasForImpartir("Diseño", null);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectAsignaturasForImpartirViewModel model = viewResult.Model as SelectAsignaturasForImpartirViewModel;

                Assert.Equal(expectedAsignaturas, model.Asignaturas, Comparer.Get<Asignatura>((p1, p2) => p1.NombreAsignatura == p2.NombreAsignatura && p1.MinimoMesesDocencia == p2.MinimoMesesDocencia));
                Assert.Equal(expectedIntensificaciones, model.Intensificaciones, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

        [Fact]
        public async Task SelectAsignatura_Get_WithFilterIntensificacion()
        {
            using (context)
            {

                // Arrange
                var controller = new ImpartirsController(context);
                controller.ControllerContext.HttpContext = impartirContext;
                Intensificacion intensificacion = new Intensificacion { NombreIntensificacion = "Hardware" };
                var intensificaciones = new List<Intensificacion> { intensificacion };
                var expectedIntensificaciones = new SelectList(intensificaciones.Select(g => g.NombreIntensificacion).ToList());


                var expectedAsignaturas = new Asignatura[3] { new Asignatura {AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200 },
                                        new Asignatura { AsignaturaID = 2, NombreAsignatura = "Sistemas Empotrados", Intensificacion = intensificacion, FechaComienzo = new DateTime(2001, 05, 10), MinimoMesesDocencia = 1, Precio = 150},
                                       new Asignatura{ AsignaturaID = 3, NombreAsignatura = "API", Intensificacion = intensificacion, FechaComienzo = new DateTime(2007, 01, 02), MinimoMesesDocencia = 4, Precio = 100}};


                // Act
                var result = controller.SelectAsignaturasForImpartir(null, "Optativa");

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectAsignaturasForImpartirViewModel model = viewResult.Model as SelectAsignaturasForImpartirViewModel;

                //Assert.Equal(expectedAsignaturas, model.Asignaturas, Comparer.Get<Asignatura>((p1, p2) => p1.NombreAsignatura == p2.NombreAsignatura && p1.MinimoMesesDocencia == p2.MinimoMesesDocencia));
                Assert.Equal(expectedIntensificaciones, model.Intensificaciones, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                // Check that both collections (expected and result returned) have the same elements with the same name

            }
        }

        [Fact]
        public async Task SelectAsignatura_Post_AsignaturaNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new ImpartirsController(context);
                controller.ControllerContext.HttpContext = impartirContext;
                Intensificacion intensificacion = new Intensificacion { NombreIntensificacion = "Optativa" };
                var intensificaciones = new List<Intensificacion> { intensificacion };
                var expectedIntensificaciones = new SelectList(intensificaciones.Select(g => g.NombreIntensificacion).ToList());


                var expectedAsignaturas = new Asignatura[3] { new Asignatura {AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200 },
                                        new Asignatura { AsignaturaID = 2, NombreAsignatura = "Sistemas Empotrados", Intensificacion = intensificacion, FechaComienzo = new DateTime(2001, 05, 10), MinimoMesesDocencia = 1, Precio = 150},
                                       new Asignatura{ AsignaturaID = 3, NombreAsignatura = "API", Intensificacion = intensificacion, FechaComienzo = new DateTime(2007, 01, 02), MinimoMesesDocencia = 4, Precio = 100}};


                SelectedAsignaturasForImpartirViewModel selected = new SelectedAsignaturasForImpartirViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectAsignaturasForImpartir(selected);

                //Assert
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectAsignaturasForImpartirViewModel model = viewResult.Model as SelectAsignaturasForImpartirViewModel;

                Assert.Equal(expectedAsignaturas, model.Asignaturas, Comparer.Get<Asignatura>((p1, p2) => p1.NombreAsignatura == p2.NombreAsignatura && p1.MinimoMesesDocencia == p2.MinimoMesesDocencia));
                Assert.Equal(expectedIntensificaciones, model.Intensificaciones, Comparer.Get<SelectListItem>((s1, s2) => s1.Value == s2.Value));
                // Check that both collections (expected and result returned) have the same elements with the same name
            }
        }

        [Fact]
        public async Task SelectAsignatura_Post_AsignaturasSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new ImpartirsController(context);
                controller.ControllerContext.HttpContext = impartirContext;

                String[] ids = new string[1] { "1" };
                SelectedAsignaturasForImpartirViewModel asignaturas = new SelectedAsignaturasForImpartirViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectAsignaturasForImpartir(asignaturas);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentAsignaturas = viewResult.RouteValues.Values.First();
                Assert.Equal(asignaturas.IdsToAdd, currentAsignaturas);

            }
        }
    }
}
