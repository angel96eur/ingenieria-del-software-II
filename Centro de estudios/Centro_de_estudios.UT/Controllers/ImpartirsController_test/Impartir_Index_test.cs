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
using Centro_de_estudios.Models.MatriculaViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Centro_de_estudios.UT.Controllers;

namespace Centro_de_estudios.UT.Controller.ImpartirsController_test
{
    public class Impartir_Index_test
    {


        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext impartirsContext;


        public Impartir_Index_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.ReInitializeDbAsignaturasForTests(context);
            //context.Users.Add(new Profesor { UserName = "angel96eur", Email = "angel96eur@gmail.com", Nombre = "angel", PrimerApellido = "Ortega", SegundoApellido = "Alfaro" });

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            impartirsContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            impartirsContext.User = identity;

        }


        [Fact]
        public async Task Index_Get()
        {
            using (context)
            {
                int i;
                var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
                var asignaturas = new List<Asignatura>();
                asignaturas.Add(new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200 });
                asignaturas.Add(new Asignatura { AsignaturaID = 2, NombreAsignatura = "Sistemas Empotrados", Intensificacion = intensificacion, FechaComienzo = new DateTime(2001, 05, 10), MinimoMesesDocencia = 1, Precio = 150 });
                asignaturas.Add(new Asignatura { AsignaturaID = 3, NombreAsignatura = "API", Intensificacion = intensificacion, FechaComienzo = new DateTime(2007, 01, 02), MinimoMesesDocencia = 4, Precio = 100 });

                Profesor profesor = new Profesor  { UserName = "angel96eur", Email = "angel96eur@gmail.com", Nombre = "angel", PrimerApellido = "Ortega", SegundoApellido = "Alfaro" };


                var expectedImpartir = new Impartir { ImpartirId = 1, Profesor = profesor, ProfesorId = profesor.Id, fecha = new DateTime(2018, 10, 18), mesesDocenciaTotal = 7, ImpartirAsignaturas = { } };

                var items = new List<ImpartirAsignatura>();
                for (i = 0; i < asignaturas.Count(); i++)
                    items.Add(new ImpartirAsignatura { Id = i + 1, Asignatura = asignaturas[i], AsignaturaId = i + 1, Impartir = expectedImpartir, ImpartirId = expectedImpartir.ImpartirId, cantidadAsignatura= 1 }); ;

                expectedImpartir.ImpartirAsignaturas = items;

                var expectedImpartirs = new List<Impartir> { };

                var controller = new ImpartirsController(context);

                // Another way to set up the User.Identity.Name
                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{new Claim(ClaimTypes.Name, "peter@uclm.com")}, "Customer"))
                    }
                };

                controller.ControllerContext.HttpContext = impartirsContext;

                // Act
                var result = await controller.Index();

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                List<Impartir> model = viewResult.Model as List<Impartir>;

                Assert.Equal(expectedImpartirs.Count, model.Count);

                for (i = 0; i < model.Count(); i++)
                    Assert.Equal(expectedImpartirs[i], model[i]);

            }
        }

    }
}