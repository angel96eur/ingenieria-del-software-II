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
using Centro_de_estudios.Models.ImpartirViewModel;
using Centro_de_estudios.UT.Controllers;

namespace Centro_de_estudios.UT.Controller.ImpartirsController_test
{
    public class Impartir_Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext impartirContext;




        public Impartir_Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDAsignaturasForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            impartirContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            impartirContext.User = identity;

        }

        [Fact]
        public async Task Create_Get_WithSelectedMovies()
        {
            using (context)
            {

                // Arrange
                var controller = new ImpartirsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = impartirContext;

                String[] ids = new string[1] { "1" };
                var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
                SelectedAsignaturasForImpartirViewModel asignaturas = new SelectedAsignaturasForImpartirViewModel() { IdsToAdd = ids };
                Asignatura expectedAsignatura = new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200, Intensificacion = intensificacion };
                Profesor expectedProfesor = new Profesor { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };

                IList<ImpartirAsignatura> expectedImpartirAsignaturas = new ImpartirAsignatura[1] { new ImpartirAsignatura { Id = 1, cantidadAsignatura = 1, Asignatura = expectedAsignatura } };
                ImpartirCreateViewModel expectedImpartir = new ImpartirCreateViewModel { ImpartirAsignaturas = expectedImpartirAsignaturas, Nombre = expectedProfesor.Nombre, PrimerApellido = expectedProfesor.PrimerApellido, SegundoApellido = expectedProfesor.SegundoApellido };

                // Act
                var result = controller.Create(asignaturas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                ImpartirCreateViewModel currentImpartir = viewResult.Model as ImpartirCreateViewModel;

                Assert.Equal(currentImpartir, expectedImpartir);

            }
        }

        [Fact]
        public async Task Create_Get_WithoutAsignatura()
        {
            using (context)
            {

                // Arrange
                var controller = new ImpartirsController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = impartirContext;
                SelectedAsignaturasForImpartirViewModel asignaturas = new SelectedAsignaturasForImpartirViewModel();
                Profesor expectedProfesor = new Profesor { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };

                ImpartirCreateViewModel expectedImpartir = new ImpartirCreateViewModel { Nombre = expectedProfesor.Nombre, PrimerApellido = expectedProfesor.PrimerApellido, SegundoApellido = expectedProfesor.SegundoApellido };

                // Act
                var result = controller.Create(asignaturas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                ImpartirCreateViewModel currentImpartir = viewResult.Model as ImpartirCreateViewModel;
                var error = viewResult.ViewData.ModelState["AsignaturaNoSelected"].Errors.FirstOrDefault();
                Assert.Equal(currentImpartir, expectedImpartir, Comparer.Get<ImpartirCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal("Deberías seleccionar una asignatura para ser impartida, por favor", error.ErrorMessage);
            }
        }

    }
}
