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
    public class Impartir_Details_test
    {
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("Centro_de_estudios")
                    .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext impartirContext;

        public Impartir_Details_test()
        {
            _contextOptions = CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);

            var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
            context.Intensificacion.Add(intensificacion);
            context.Asignatura.Add(new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200 });
            context.Asignatura.Add(new Asignatura { AsignaturaID = 2, NombreAsignatura = "Sistemas Empotrados", Intensificacion = intensificacion, FechaComienzo = new DateTime(2001, 05, 10), MinimoMesesDocencia = 1, Precio = 150 });
            context.Asignatura.Add(new Asignatura { AsignaturaID = 3, NombreAsignatura = "API", Intensificacion = intensificacion, FechaComienzo = new DateTime(2007, 01, 02), MinimoMesesDocencia = 4, Precio = 100 });

            context.Users.Add(new Profesor { UserName = "angel@uclm.com", Email = "angel@uclm.com", Nombre = "Angel", PrimerApellido = "Ortega", SegundoApellido = "Alfaro" });

            context.SaveChanges();

            context.Impartir.Add(new Impartir { Profesor = context.Profesor.First(), ProfesorId = context.Profesor.First().Id, fecha = new DateTime(2018, 10, 18), mesesDocenciaTotal = 30, ImpartirAsignaturas = { } });
            //context.Purchase.Add(new Purchase {Customer = null, DeliveryAddress = "Avd. España s/n", PaymentMethod = new PayPal { Email = "peter@uclm.com", Phone = "967959595", Prefix = "+34" }, PurchaseDate = new DateTime(2018, 10, 18), TotalPrice = 30, PurchaseItems = { } });
            context.SaveChanges();

            foreach (var asignatura in context.Asignatura.ToList())
                context.impartirAsignatura.Add(new ImpartirAsignatura { Asignatura = asignatura, Impartir = context.Impartir.First(), cantidadAsignatura = 1 });

            context.SaveChanges();

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("angel@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            impartirContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            impartirContext.User = identity;

        }


        [Fact]
        public async Task Details_withoutId()
        {
            // Arrange
            using (context)
            {
                var controller = new ImpartirsController(context);
                controller.ControllerContext.HttpContext = impartirContext;

                // Act
                var result = await controller.Details(null);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        public async Task Details_Impartir_notfound()
        {
            // Arrange
            using (context)
            {
                var controller = new ImpartirsController(context);
                controller.ControllerContext.HttpContext = impartirContext;
                var id = context.Impartir.Last().ImpartirId + 1;

                // Act
                var result = await controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result);

            }
        }

        [Fact]
        public async Task Details_Impartir_found()
        {
            // Arrange
            using (context)
            {
                int i;
                var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
                var asignaturas = new List<Asignatura>();
                asignaturas.Add(new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200 });
                asignaturas.Add(new Asignatura { AsignaturaID = 2, NombreAsignatura = "Sistemas Empotrados", Intensificacion = intensificacion, FechaComienzo = new DateTime(2001, 05, 10), MinimoMesesDocencia = 1, Precio = 150 });
                asignaturas.Add(new Asignatura { AsignaturaID = 3, NombreAsignatura = "API", Intensificacion = intensificacion, FechaComienzo = new DateTime(2007, 01, 02), MinimoMesesDocencia = 4, Precio = 100 });

                Profesor profesor = new Profesor { UserName = "angel@uclm.com", Email = "angel@uclm.com", Nombre = "Angel", PrimerApellido = "Ortega", SegundoApellido = "Alfaro" };

                var expectedImpartir = new Impartir { ImpartirId =1,Profesor = profesor, ProfesorId = profesor.Id, fecha = new DateTime(2018, 10, 18), mesesDocenciaTotal = 30, ImpartirAsignaturas = { } };

                var items = new List<ImpartirAsignatura>();
                foreach(var asignatura in asignaturas)
                {
                    items.Add(new ImpartirAsignatura { Asignatura = asignatura, Impartir = expectedImpartir, cantidadAsignatura = 1 });
                }
                expectedImpartir.ImpartirAsignaturas = items;

                context.SaveChanges();

                var controller = new ImpartirsController(context);
                controller.ControllerContext.HttpContext = impartirContext;
                var id = context.Impartir.Last().ImpartirId;

                // Act
                var result = await controller.Details(1);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as Impartir;

                Assert.Equal(model, expectedImpartir, Comparer.Get<Impartir>((p1, p2) =>  p1.fecha == p2.fecha && p1.mesesDocenciaTotal == p2.mesesDocenciaTotal /*&& p1.Profesor.Equals(p2.Profesor)*/));
                Assert.Equal(model.ImpartirAsignaturas, expectedImpartir.ImpartirAsignaturas, Comparer.Get<ImpartirAsignatura>((p1, p2) => p1.cantidadAsignatura == p2.cantidadAsignatura /*&& p1.Asignatura.Equals(p2.Asignatura)*/));

            }
        }
    }
}
