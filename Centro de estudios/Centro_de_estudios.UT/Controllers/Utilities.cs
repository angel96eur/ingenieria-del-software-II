using Centro_de_estudios.Data;
using Centro_de_estudios.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Centro_de_estudios.UT.Controllers
{
    public static class Utilities
    {
        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("Centro de estudios")
                    .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }

        public static void InitializeDbIntensificacionesForTests(ApplicationDbContext db)
        {
            db.Intensificacion.Add(new Intensificacion { NombreIntensificacion = "Software" });
            db.Intensificacion.Add(new Intensificacion { NombreIntensificacion = "Hardware" });

            db.SaveChanges();

        }

        public static void InitializeDbTipoMaterialForTests(ApplicationDbContext db)
        {
            db.TipoMaterial.Add(new TipoMaterial { Nombre = "Borrador" });
            db.TipoMaterial.Add(new TipoMaterial { Nombre = "Corrector" });

            db.SaveChanges();

        }

        public static void ReInitializeDbIntensificacionesForTests(ApplicationDbContext db)
        {
            db.Intensificacion.RemoveRange(db.Intensificacion);
            db.SaveChanges();
        }
        public static void ReInitializeDbTipoMaterialForTests(ApplicationDbContext db)
        {
            db.TipoMaterial.RemoveRange(db.TipoMaterial);
            db.SaveChanges();
        }

        public static void InitializeDAsignaturasForTests(ApplicationDbContext db)
        {
            var intensificacion = new Intensificacion { NombreIntensificacion = "Software" };
            db.Intensificacion.Add(intensificacion);
            db.Asignatura.Add(new Asignatura { AsignaturaID = 1, NombreAsignatura = "Diseño software", Intensificacion = intensificacion, FechaComienzo = new DateTime(2011, 10, 20), MinimoMesesDocencia = 2, Precio = 200, cantidadAlumnos = 2});
            db.Asignatura.Add(new Asignatura { AsignaturaID = 2, NombreAsignatura = "Sistemas Empotrados", Intensificacion = intensificacion, FechaComienzo = new DateTime(2001, 05, 10), MinimoMesesDocencia = 1, Precio = 150, cantidadAlumnos = 2 });
            db.Asignatura.Add(new Asignatura { AsignaturaID = 3, NombreAsignatura = "API", Intensificacion = intensificacion, FechaComienzo = new DateTime(2007, 01, 02), MinimoMesesDocencia = 4, Precio = 100, cantidadAlumnos = 2 });

            db.Profesor.Add(new Profesor { Id = "1", UserName = "peter@uclm.com", PhoneNumber = "967959595", Email = "peter@uclm.com", Nombre = "Peter", PrimerApellido = "Jackson", SegundoApellido = "García" });
            db.Impartir.Add(new Impartir { ImpartirId = 1, fecha = new DateTime(2019, 10, 11), mesesDocenciaTotal = 2 });
            db.Impartir.Add(new Impartir { ImpartirId = 2, fecha = new DateTime(2019, 10, 10), mesesDocenciaTotal = 5 });
            db.impartirAsignatura.Add(new ImpartirAsignatura { cantidadAsignatura = 1, Id = 1, ImpartirId = 1, AsignaturaId = 1 });
            db.impartirAsignatura.Add(new ImpartirAsignatura { cantidadAsignatura = 1, Id = 2, ImpartirId = 2, AsignaturaId = 2 });
            db.impartirAsignatura.Add(new ImpartirAsignatura { cantidadAsignatura = 1, Id = 3, ImpartirId = 2, AsignaturaId = 3 });

            db.SaveChanges();
        }

        public static void ReInitializeDbAsignaturasForTests(ApplicationDbContext db)
        {
            db.Intensificacion.RemoveRange(db.Intensificacion);
            db.Asignatura.RemoveRange(db.Asignatura);
            db.SaveChanges();
        }

        public static void InitializeDMaterialForTests(ApplicationDbContext db)
        {
            var tipoMaterial = new TipoMaterial { Nombre = "Borrador" };
            db.TipoMaterial.Add(tipoMaterial);
            db.Material.Add(new Material { MaterialID = 1, TipoMaterial = tipoMaterial, CantidadCompra = 5, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 12, Titulo = "Goma" });
            db.Material.Add(new Material { MaterialID = 2, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Saca Puntas"});
            db.Material.Add(new Material { MaterialID = 3, TipoMaterial = tipoMaterial, CantidadCompra = 2, FechaLanzamiento = new DateTime(2010, 10, 20), PrecioCompra = 5, Titulo = "Boli azul" });

            //tipoMaterial = new TipoMaterial { Nombre = "Corrector" };
            //db.TipoMaterial.Add(tipoMaterial);
            //db.Material.Add(new Material { MaterialID = 2, TipoMaterial = tipoMaterial, CantidadCompra = 7, FechaLanzamiento = new DateTime(2011, 10, 20), PrecioCompra = 5, Titulo = "Tipex" });

            db.SaveChanges();
        }

        public static void ReInitializeDbMaterialForTests(ApplicationDbContext db)
        {
            db.TipoMaterial.RemoveRange(db.TipoMaterial);
            db.Material.RemoveRange(db.Material);
            db.SaveChanges();
        }

    }
}
