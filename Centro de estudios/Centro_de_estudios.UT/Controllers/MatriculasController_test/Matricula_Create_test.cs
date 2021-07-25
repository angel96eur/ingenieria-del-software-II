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
using Centro_de_estudios.Models.MatriculaViewModels;
using Centro_de_estudios.UT.Controllers;

namespace Centro_de_estudios.UT.Controller.MatriculasController_test
{
    public class Matricula_Create_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext matricularContext;


         

        public Matricula_Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Utilities.InitializeDAsignaturasForTests(context);

            context.Users.Add(new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });

            context.SaveChanges();


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            matricularContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            matricularContext.User = identity;

        }

        [Fact]
        public async Task Create_Get_WithSelectedAsignaturas()
        {
            using (context)
            {

                // Arrange
                var controller = new MatriculasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = matricularContext;

                String[] ids = new string[1] { "1" };
                SelectedAsignaturasForMatriculaViewModel asignaturas = new SelectedAsignaturasForMatriculaViewModel() { IdsToAdd = ids };
                Asignatura expectedAsignatura = new Asignatura { NombreAsignatura = "Diseño software", FechaComienzo = new DateTime(2011, 10, 20), Precio = 200, cantidadAlumnos = 2 };
                Estudiante expectedEstudiante = new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };

                IList<Matricula_Asignatura> expectedMatriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 1, Asignatura = expectedAsignatura } };
                MatriculaCreateViewModel expectedMatricula = new MatriculaCreateViewModel { Matricula_Asignaturas = expectedMatriculaAsignaturas, Nombre = expectedEstudiante.Nombre, PrimerApellido = expectedEstudiante.PrimerApellido, SegundoApellido = expectedEstudiante.SegundoApellido };

                // Act
                var result = controller.Create(asignaturas);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result);
                MatriculaCreateViewModel currentMatricula = viewResult.Model as MatriculaCreateViewModel;

                Assert.Equal(currentMatricula, expectedMatricula, Comparer.Get<MatriculaCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal(currentMatricula.Matricula_Asignaturas[0].Asignatura, expectedMatriculaAsignaturas[0].Asignatura, Comparer.Get<Asignatura>((p1, p2) => p1.NombreAsignatura == p2.NombreAsignatura && p1.Precio == p2.Precio));

            }
        }
        [Fact]
        public async Task Create_Get_WithoutAsignatura()
        {
            using (context)
            {

                // Arrange
                var controller = new MatriculasController(context);
                //simulate user's connection
                controller.ControllerContext.HttpContext = matricularContext;
                SelectedAsignaturasForMatriculaViewModel selectedAsignaturas = new SelectedAsignaturasForMatriculaViewModel();
                Estudiante expectedEstudiante = new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };

                MatriculaCreateViewModel expectedPurchase = new MatriculaCreateViewModel { Nombre = expectedEstudiante.Nombre, PrimerApellido = expectedEstudiante.PrimerApellido, SegundoApellido = expectedEstudiante.SegundoApellido };
                // Act
                var result = controller.Create(selectedAsignaturas);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                MatriculaCreateViewModel currentPurchase = viewResult.Model as MatriculaCreateViewModel;
                var error = viewResult.ViewData.ModelState["No ha seleccionado asignatura"].Errors.FirstOrDefault();
                Assert.Equal(currentPurchase, expectedPurchase, Comparer.Get<MatriculaCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal($"Por favor, seleccione al menos una asignatura", error.ErrorMessage);
            }
        }


        [Fact]
        public async Task Create_Post_WithoutEnoughAsignaturasToBeMatriculado()
        {
            using (context)
            {

                // Arrange
                var controller = new MatriculasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = matricularContext;
                var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
                Asignatura asignatura = new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), cantidadAlumnos = 300, Precio = 200 };
                Estudiante estudiante = new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };
                var payment1 = new PayPal { Email = "peter@uclm.com", Phone = "967959595", Prefix = "+34" };

                IList<Matricula_Asignatura> matriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 1, Asignatura = asignatura } };
                MatriculaCreateViewModel matricula = new MatriculaCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, Matricula_Asignaturas = matriculaAsignaturas, Direccion = "Albacete", PayPal = payment1 };

                IList<Matricula_Asignatura> expectedMatriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 1, Asignatura = asignatura } };
                MatriculaCreateViewModel expectedMatricula = new MatriculaCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, Matricula_Asignaturas = expectedMatriculaAsignaturas, Direccion = "Albacete", PayPal = payment1 };


                // Act
                var result = controller.CreatePost(matricula, matriculaAsignaturas, null, payment1);

                //
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                MatriculaCreateViewModel currentPurchase = viewResult.Model as MatriculaCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentPurchase, expectedMatricula, Comparer.Get<MatriculaCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal($"No hay suficiente material titulado Diseño software, por favor selecciones menos o igual que 1", error.ErrorMessage);
                Assert.Equal(currentPurchase.Matricula_Asignaturas[0].Asignatura, expectedMatriculaAsignaturas[0].Asignatura, Comparer.Get<Asignatura>((p1, p2) => p1.Equals(p2)));
            }
        }



        [Fact]
        public async Task Create_Post_WithAlumnos0ForMatricula()
        {
            using (context)
            {

                // Arrange
                var controller = new MatriculasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = matricularContext;
                var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
                Asignatura asignatura = new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), cantidadAlumnos = 2, Precio = 200 };
                Estudiante estudiante = new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };
                var payment1 = new PayPal { Email = "peter@uclm.com", Phone = "967959595", Prefix = "+34" };

                IList<Matricula_Asignatura> matriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 0, Asignatura = asignatura } };
                MatriculaCreateViewModel matricula = new MatriculaCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, Matricula_Asignaturas = matriculaAsignaturas, Direccion = "Albacete", PayPal = payment1 };

                IList<Matricula_Asignatura> expectedMatriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 0, Asignatura = asignatura } };
                MatriculaCreateViewModel expectedMatricula = new MatriculaCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, Matricula_Asignaturas = expectedMatriculaAsignaturas, Direccion = "Albacete", PayPal = payment1 };


                // Act
                var result = controller.CreatePost(matricula, matriculaAsignaturas, null, payment1);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                MatriculaCreateViewModel currentMatricula = viewResult.Model as MatriculaCreateViewModel;

                //Assert

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(currentMatricula, expectedMatricula, Comparer.Get<MatriculaCreateViewModel>((p1, p2) => p1.Nombre == p2.Nombre && p1.PrimerApellido == p2.PrimerApellido && p1.SegundoApellido == p2.SegundoApellido));
                Assert.Equal($"No hay suficiente material titulado Diseño software, por favor selecciones menos o igual que 2", error.ErrorMessage);                
            } 
        }



        [Fact]
        public async Task Create_Post_HavingEnoughMesesDocenciaWithPaypal()
        {
            using (context)
            {
                // Arrange
                var controller = new MatriculasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = matricularContext;
                var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
                Asignatura asignatura = new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), cantidadAlumnos = 2, Precio = 200 };
                Estudiante estudiante = new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };
                var payment1 = new PayPal { Email = "peter@uclm.com", Phone = "967959595", Prefix = "+34" };

                IList<Matricula_Asignatura> matriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 2 , Asignatura = asignatura } };
                MatriculaCreateViewModel matricula = new MatriculaCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, Matricula_Asignaturas = matriculaAsignaturas, Direccion = "Albacete", MetodoPago = "PayPal", PayPal = payment1 };


                IList<Matricula_Asignatura> expectedMatriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 2 , Asignatura = asignatura } };
                Matricula expectedMatricula = new Matricula { Estudiante = estudiante, MatriculaAsignaturas = expectedMatriculaAsignaturas, Residencia = "Albacete", MetodoPago = payment1, PrecioTotal = 400, FechaMatricula = System.DateTime.Now };

                // Notice: the total price expected is 24, that is, 2 movies purchased for 12

                // Act
                var result = controller.CreatePost(matricula, matriculaAsignaturas, null, payment1);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

                //we should check the matricula has been created in the database
                var actualMatricula = context.Matricula.Include(p => p.MatriculaAsignaturas).FirstOrDefault(p => p.MatriculaId == 1);
                //By checking its attributes
                Assert.Equal(actualMatricula, expectedMatricula, Comparer.Get<Matricula>(
                                                  (p1, p2) => p1.PrecioTotal == p2.PrecioTotal
                                                             && p1.MetodoPago.Equals(p2.MetodoPago)
                                                             && p1.FechaMatricula.Year == p2.FechaMatricula.Year
                                                             && p1.FechaMatricula.Month == p2.FechaMatricula.Month
                                                             && p1.FechaMatricula.Day == p2.FechaMatricula.Day
                                                             && p1.Estudiante.UserName == p2.Estudiante.UserName
                                                             && p1.Residencia == p2.Residencia));
                //By checking its compunded attritbutes
                //MatriculaAsignaturas:
                Assert.Equal(actualMatricula.MatriculaAsignaturas, expectedMatricula.MatriculaAsignaturas, Comparer.Get<Matricula_Asignatura>(
                                                   (i1, i2) => i1.cantidad == i2.cantidad
                                                             && i1.Asignatura.NombreAsignatura == i2.Asignatura.NombreAsignatura));
                //And that the asignaturas associated to the mesesdocencia for matricula 
                //of each associated asignatura has been modified accordingly 
                for (int i = 0; i < actualMatricula.MatriculaAsignaturas.Count; i++)
                    Assert.Equal(context.Asignatura.ToList()[i].cantidadAlumnos, expectedMatriculaAsignaturas[i].Asignatura.cantidadAlumnos - matriculaAsignaturas[i].cantidad);

            }

        }

        [Fact]
        public async Task Create_Post_HavingEnoughMesesDocenciaWithCreditCard()
        {
            using (context)
            {

                // Arrange
                var controller = new MatriculasController(context);

                //simulate user's connection
                controller.ControllerContext.HttpContext = matricularContext;
                var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
                Asignatura asignatura = new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), cantidadAlumnos = 2, Precio = 200 };
                Estudiante estudiante = new Estudiante { UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" };
                var payment1 = new CreditCard { CreditCardNumber = "1111111111111111", CCV = "111", ExpirationDate = new DateTime(2020, 10, 10) };

                IList<Matricula_Asignatura> matriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 2, Asignatura = asignatura } };
                MatriculaCreateViewModel matricula = new MatriculaCreateViewModel { Nombre = estudiante.Nombre, PrimerApellido = estudiante.PrimerApellido, SegundoApellido = estudiante.SegundoApellido, Matricula_Asignaturas = matriculaAsignaturas, Direccion = "Albacete", MetodoPago = "CreditCard", CreditCard = payment1 };

                IList<Matricula_Asignatura> expectedMatriculaAsignaturas = new Matricula_Asignatura[1] { new Matricula_Asignatura { Id = 1, cantidad = 2, Asignatura = asignatura } };
                Matricula expectedMatricula = new Matricula { Estudiante = estudiante, MatriculaAsignaturas = expectedMatriculaAsignaturas, Residencia = "Albacete", MetodoPago = payment1, PrecioTotal = 400, FechaMatricula = System.DateTime.Now };
                // Notice: the total price expected is 24, that is, 2 movies purchased for 12

                // Act
                var result = controller.CreatePost(matricula, matriculaAsignaturas, payment1, null);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

                //we should check the matricula has been created in the database
                var actualMatricula = context.Matricula.Include(p => p.MatriculaAsignaturas).FirstOrDefault(p => p.MatriculaId == 1);
                //By checking its attributes
                Assert.Equal(actualMatricula, expectedMatricula, Comparer.Get<Matricula>(
                                                  (p1, p2) => p1.PrecioTotal == p2.PrecioTotal
                                                             && p1.MetodoPago.Equals(p2.MetodoPago)
                                                             && p1.FechaMatricula.Year == p2.FechaMatricula.Year
                                                             && p1.FechaMatricula.Month == p2.FechaMatricula.Month
                                                             && p1.FechaMatricula.Day == p2.FechaMatricula.Day
                                                             && p1.Estudiante.UserName == p2.Estudiante.UserName
                                                             && p1.Residencia == p2.Residencia));
                //By checking its compunded attritbutes
                //MatriculaAsignaturas:
                Assert.Equal(actualMatricula.MatriculaAsignaturas, expectedMatricula.MatriculaAsignaturas, Comparer.Get<Matricula_Asignatura>(
                                                   (i1, i2) => i1.cantidad == i2.cantidad
                                                             && i1.Asignatura.NombreAsignatura == i2.Asignatura.NombreAsignatura));
                //And that the asignaturas associated to the mesesdocencia for matricula 
                //of each associated asignatura has been modified accordingly 
                for (int i = 0; i < actualMatricula.MatriculaAsignaturas.Count; i++)
                    Assert.Equal(context.Asignatura.ToList()[i].cantidadAlumnos, expectedMatriculaAsignaturas[i].Asignatura.cantidadAlumnos - matriculaAsignaturas[i].cantidad);

            }

        }
    }
}
