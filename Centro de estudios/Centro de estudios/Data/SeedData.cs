using Centro_de_estudios.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Centro_de_estudios.Data
{
    public static class SeedData
    {
        //public static void Initialize(UserManager<ApplicationUser> userManager,
        //            RoleManager<IdentityRole> roleManager)
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var role = serviceProvider.GetRequiredService(typeof(RoleManager<IdentityRole>));
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();


            List<string> rolesNames = new List<string> { "Estudiante", "Profesor" };

            SeedRoles(roleManager, rolesNames);
            SeedUsers(userManager, rolesNames);
            SeedAsignaturas(dbContext);
            SeedMateriales(dbContext);

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {

            foreach (string roleName in roles)
            {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<IdentityUser> userManager, List<string> roles)
        {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("gregorio@uclm.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "gregorio@uclm.com";
                user.Email = "gregorio@uclm.com";
                user.Nombre = "Gregorio";
                user.PrimerApellido = "Diaz";
                user.SegundoApellido = "Descalzo";

                IdentityResult result = userManager.CreateAsync(user, "APassword1234%").Result;

                if (result.Succeeded)
                {
                    //Employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false;
                }
            }

            if (userManager.FindByNameAsync("angel@uclm.com").Result == null)
            {
                Profesor user = new Profesor();
                user.UserName = "angel@uclm.com";
                user.Email = "angel@uclm.com";
                user.Nombre = "Angel";
                user.PrimerApellido = "Ortega";
                user.SegundoApellido = "Alfaro";

                IdentityResult result = userManager.CreateAsync(user, "Password1234%").Result;

                if (result.Succeeded)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false;
                }
            }

            if (userManager.FindByNameAsync("alvaro@uclm.com").Result == null)
            {
                Estudiante user = new Estudiante();
                user.UserName = "alvaro@uclm.com";
                user.Email = "alvaro@uclm.com";
                user.Nombre = "Alvaro";
                user.PrimerApellido = "Leon";
                user.SegundoApellido = "Cebrian";

                IdentityResult result = userManager.CreateAsync(user, "APassword1234%").Result;

                if (result.Succeeded)
                {
                    //Employee role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false;
                }
            }

            if (userManager.FindByNameAsync("francisco@uclm.com").Result == null)
            {
                Estudiante user = new Estudiante();
                user.UserName = "francisco@uclm.com";
                user.Email = "francisco@uclm.com";
                user.Nombre = "Francisco";
                user.PrimerApellido = "Moreno";
                user.SegundoApellido = "Jimenez";

                IdentityResult result = userManager.CreateAsync(user, "APassword1234%").Result;

                if (result.Succeeded)
                {
                    //Employee role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false;
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.com").Result == null)
            {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                Estudiante user = new Estudiante();
                user.UserName = "peter@uclm.com";
                user.Email = "peter@uclm.com";
                user.Nombre = "Peter";
                user.PrimerApellido = "Jackson";
                user.SegundoApellido = "Jackson";

                IdentityResult result = userManager.CreateAsync(user, "OtherPass12$").Result;

                if (result.Succeeded)
                {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false;
                }
            }

        }

        //datos sobre el material y el tipo de material
        public static void SeedMateriales(ApplicationDbContext dbContext)
        {
            //Genres and movies are created so that they are available whenever the system is run
            Material material;
            TipoMaterial tipoMaterial = dbContext.TipoMaterial.FirstOrDefault(m => m.Nombre.Contains("Libro"));
            if (tipoMaterial == null)
            {
                tipoMaterial = new TipoMaterial()
                {
                    Nombre = "Libro"
                };
                dbContext.TipoMaterial.Add(tipoMaterial);
            }

            if (!dbContext.Material.Any(m => m.Titulo.Contains("Gestion de Redes")))
            {
                material = new Material()
                {
                    Titulo = "Gestion de Redes",
                    CantidadCompra = 40,
                    PrecioCompra = 28,
					FechaLanzamiento = new DateTime(2000,01,01),
                    TipoMaterial = tipoMaterial
                };
                dbContext.Material.Add(material);
            }

            if (!dbContext.Material.Any(m => m.Titulo.Contains("Seguridad en Redes")))
            {
                material = new Material()
                {
                    Titulo = "Seguridad en Redes",
                    CantidadCompra = 50,
                    PrecioCompra = 20,
					FechaLanzamiento = new DateTime(2000,02,02),
                    TipoMaterial = tipoMaterial
                };
                dbContext.Material.Add(material);
            }

            tipoMaterial = dbContext.TipoMaterial.FirstOrDefault(m => m.Nombre.Contains("Libreta"));
            if (tipoMaterial == null)
            {
                tipoMaterial = new TipoMaterial()
                {
                    Nombre = "Libreta"
                };
                dbContext.TipoMaterial.Add(tipoMaterial);
            }

            if (!dbContext.Material.Any(m => m.Titulo.Contains("Libreta de cuadros")))
            {
                material = new Material()
                {
                    Titulo = "Libreta de cuadros",
                    CantidadCompra = 40,
                    PrecioCompra = 3,
					FechaLanzamiento = new DateTime(2000,03,03),
                    TipoMaterial = tipoMaterial
                };
                dbContext.Material.Add(material);
            }

            if (!dbContext.Material.Any(m => m.Titulo.Contains("Libreta de Rayas")))
            {
                material = new Material()
                {
                    Titulo = "Libreta de Rayas",
                    CantidadCompra = 40,
                    PrecioCompra = 5,
					FechaLanzamiento = new DateTime(2000,04,04),
                    TipoMaterial = tipoMaterial
                };
                dbContext.Material.Add(material);
            }
            tipoMaterial = dbContext.TipoMaterial.FirstOrDefault(m => m.Nombre.Contains("Boligrafos"));
            if (tipoMaterial == null)
            {
                tipoMaterial = new TipoMaterial()
                {
                    Nombre = "Boligrafos"
                };
                dbContext.TipoMaterial.Add(tipoMaterial);
            }

            if (!dbContext.Material.Any(m => m.Titulo.Contains("Bic")))
            {
                material = new Material()
                {
                    Titulo = "Bic",
                    CantidadCompra = 60,
                    PrecioCompra = 2,
					FechaLanzamiento = new DateTime(2002,09,01),
                    TipoMaterial = tipoMaterial
                };
                dbContext.Material.Add(material);
            }

            if (!dbContext.Material.Any(m => m.Titulo.Contains("Lapiz")))
            {
                material = new Material()
                {
                    Titulo = "Lapiz",
                    CantidadCompra = 60,
                    PrecioCompra = 1,
                    FechaLanzamiento = new DateTime(2002,01,07),
                    TipoMaterial = tipoMaterial
                };
                dbContext.Material.Add(material);
            }

            tipoMaterial = dbContext.TipoMaterial.FirstOrDefault(m => m.Nombre.Contains("Licencias"));
            if (tipoMaterial == null)
            {
                tipoMaterial = new TipoMaterial()
                {
                    Nombre = "Licencias"
                };
                dbContext.TipoMaterial.Add(tipoMaterial);
            }

            if (!dbContext.Material.Any(m => m.Titulo.Contains("Licencia Packet Tracert")))
            {
                material = new Material()
                {
                    Titulo = "Licencia Packet Tracert",
                    CantidadCompra = 30,
                    PrecioCompra = 60,
					FechaLanzamiento = new DateTime(2000,05,02),
                    TipoMaterial = tipoMaterial
                };
                dbContext.Material.Add(material);
            }
            dbContext.SaveChanges();
        }

        //datos sobre el material y el tipo de material
        public static void SeedAsignaturas(ApplicationDbContext dbContext)
        {
            //Genres and movies are created so that they are available whenever the system is run
            Asignatura asignatura;
            Intensificacion intensificacion = dbContext.Intensificacion.FirstOrDefault(m => m.NombreIntensificacion.Contains("Computadores"));
            if (intensificacion == null)
            {
                intensificacion = new Intensificacion()
                {
                    NombreIntensificacion = "Computadores"
                };
                dbContext.Intensificacion.Add(intensificacion);
            }

            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("Gestion de Redes")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "Gestion de Redes",
                    MinimoMesesDocencia = 1,
                    Precio = 150,
                    FechaComienzo = DateTime.UtcNow,
                    Intensificacion = intensificacion,
                    cantidadAlumnos = 200
                };
                dbContext.Asignatura.Add(asignatura);
            }

            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("Sistemas operativos II")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "Sistemas operativos II",
                    MinimoMesesDocencia = 1,
                    Precio = 100,
                    FechaComienzo = DateTime.UtcNow,
                    cantidadAlumnos = 200,
                    Intensificacion = intensificacion
                };
                dbContext.Asignatura.Add(asignatura);
            }

            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("Seguridad de redes")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "Seguridad de redes",
                    MinimoMesesDocencia = 2,
                    Precio = 200,
                    FechaComienzo = DateTime.UtcNow,
                    cantidadAlumnos = 200,
                    Intensificacion = intensificacion
                };
                dbContext.Asignatura.Add(asignatura);
            }

            intensificacion = dbContext.Intensificacion.FirstOrDefault(m => m.NombreIntensificacion.Contains("Computacion"));
            if (intensificacion == null)
            {
                intensificacion = new Intensificacion()
                {
                    NombreIntensificacion = "Computacion"
                };
                dbContext.Intensificacion.Add(intensificacion);
            }

            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("Diseño de algoritmos")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "Diseño de algoritmos",
                    MinimoMesesDocencia = 3,
                    Precio = 150,
                    FechaComienzo = DateTime.UtcNow,
                    cantidadAlumnos = 200,
                    Intensificacion = intensificacion
                };
                dbContext.Asignatura.Add(asignatura);
            }

            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("Sistemas multiagentes")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "Sistemas multiagentes",
                    MinimoMesesDocencia = 1,
                    Precio = 200,
                    FechaComienzo = DateTime.UtcNow,
                    cantidadAlumnos = 200,
                    Intensificacion = intensificacion
                };
                dbContext.Asignatura.Add(asignatura);
            }


            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("Mineria de datos")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "Mineria de datos",
                    MinimoMesesDocencia = 3,
                    Precio = 250,
                    FechaComienzo = DateTime.UtcNow,
                    cantidadAlumnos = 200,
                    Intensificacion = intensificacion
                };
                dbContext.Asignatura.Add(asignatura);
            }

            intensificacion = dbContext.Intensificacion.FirstOrDefault(m => m.NombreIntensificacion.Contains("TIC"));
            if (intensificacion == null)
            {
                intensificacion = new Intensificacion()
                {
                    NombreIntensificacion = "TIC"
                };
            }
            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("IPO")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "IPO",
                    MinimoMesesDocencia = 1,
                    Precio = 300,
                    FechaComienzo = DateTime.UtcNow,
                    cantidadAlumnos = 200,
                    Intensificacion = intensificacion
                };
                dbContext.Asignatura.Add(asignatura);
            }

            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("Comercio electronico")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "Comercio electronico",
                    MinimoMesesDocencia = 1,
                    Precio = 100,
                    FechaComienzo = DateTime.UtcNow,
                    cantidadAlumnos = 200,
                    Intensificacion = intensificacion
                };
                dbContext.Asignatura.Add(asignatura);
            }

            if (!dbContext.Asignatura.Any(m => m.NombreAsignatura.Contains("Multimedia")))
            {
                asignatura = new Asignatura()
                {
                    NombreAsignatura = "Multimedia",
                    MinimoMesesDocencia = 1,
                    Precio = 300,
                    FechaComienzo = DateTime.UtcNow,
                    cantidadAlumnos = 200,
                    Intensificacion = intensificacion
                };
                dbContext.Asignatura.Add(asignatura);
            }
            dbContext.SaveChanges();
        }



    }

}






