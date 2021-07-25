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

namespace Centro_de_estudios.UT.Controller.MatriculasController_test
{
    public class Matricula_Index_test
    {


        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext matricularContext;


        public Matricula_Index_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDAsignaturasForTests(context);

            context.Users.Add(new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });

            context.SaveChanges();

            context.Matricula.Add(new Matricula { Estudiante = context.Estudiante.First(), EstudianteId = context.Estudiante.First().Id, Residencia = "Avd. España s/n", MetodoPago = new PayPal { Email = "peter@uclm.com", Phone = "967959595", Prefix = "+34" }, FechaMatricula = new DateTime(2018, 10, 18), PrecioTotal = 30, MatriculaAsignaturas = { } });
            //context.Purchase.Add(new Purchase {Customer = null, DeliveryAddress = "Avd. España s/n", PaymentMethod = new PayPal { Email = "peter@uclm.com", Phone = "967959595", Prefix = "+34" }, PurchaseDate = new DateTime(2018, 10, 18), TotalPrice = 30, PurchaseItems = { } });
            context.SaveChanges();

            foreach (var asignatura in context.Asignatura.ToList())
                context.Matricula_Asignatura.Add(new Matricula_Asignatura { Asignatura = asignatura, Matricula = context.Matricula.First(), cantidad = 1 });

            context.SaveChanges();

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            matricularContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            matricularContext.User = identity;
        }


        [Fact]
        public async Task Index_Get()
        {
            using (context)
            {
                var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
                var asignaturas = new List<Asignatura>();
                asignaturas.Add(new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), cantidadAlumnos = 2, Precio = 200 });
                asignaturas.Add(new Asignatura { AsignaturaID = 2, NombreAsignatura = "Sistemas Empotrados", Intensificacion = intensificacion, FechaComienzo = new DateTime(2001, 05, 10), cantidadAlumnos = 2, Precio = 150 });
                asignaturas.Add(new Asignatura { AsignaturaID = 3, NombreAsignatura = "API", Intensificacion = intensificacion, FechaComienzo = new DateTime(2007, 01, 02), cantidadAlumnos = 2, Precio = 100 });

                var user = new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };

                var matricula = new Matricula { Estudiante = context.Estudiante.First(), EstudianteId = context.Estudiante.First().Id, Residencia = "Avd. España s/n", MetodoPago = new PayPal { Email = "peter@uclm.com", Phone = "967959595", Prefix = "+34" }, FechaMatricula = new DateTime(2018, 10, 18), PrecioTotal = 30, MatriculaAsignaturas = { } };

                var items = new List<Matricula_Asignatura>();
                foreach (var asignatura in asignaturas)
                    items.Add(new Matricula_Asignatura { Asignatura = asignatura, Matricula = matricula, cantidad = 1 });

                matricula.MatriculaAsignaturas = items;

                var expectedMatriculas = new List<Matricula> { matricula };

                context.SaveChanges();

                var controller = new MatriculasController(context);

                // Another way to set up the User.Identity.Name
                /*controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{new Claim(ClaimTypes.Name, "peter@uclm.com")}, "Customer"))
                    }
                };*/

                controller.ControllerContext.HttpContext = matricularContext;

                // Act
                var result = await controller.Index();

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as IEnumerable<Matricula>;

                Assert.Equal(model, expectedMatriculas, Comparer.Get<Matricula>((p1, p2) => p1.MetodoPago.Equals(p2.MetodoPago) && p1.FechaMatricula == p2.FechaMatricula && p1.PrecioTotal == p2.PrecioTotal && p1.Residencia == p2.Residencia && p1.Estudiante.Equals(p2.Estudiante)));
                foreach (var expectedMatricula in expectedMatriculas)
                    Assert.Equal(model.First().MatriculaAsignaturas, expectedMatricula.MatriculaAsignaturas, Comparer.Get<Matricula_Asignatura>((p1, p2) => p1.cantidad == p2.cantidad));
            }
        }
    }
}