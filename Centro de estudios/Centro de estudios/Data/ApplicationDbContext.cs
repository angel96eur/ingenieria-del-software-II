using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Centro_de_estudios.Models;
using Microsoft.AspNetCore.Identity;

namespace Centro_de_estudios.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Asignatura> Asignatura { get; set; }
        public DbSet<Impartir> Impartir { get; set; }
        public DbSet<ImpartirAsignatura> impartirAsignatura { get; set; }
        public DbSet<Intensificacion> Intensificacion { get; set; }
        public DbSet<Profesor> Profesor { get; set; }
		public DbSet<Compra> Compra { get; set; }
        public DbSet<CompraMaterial> CompraMaterial { get; set; }
        public DbSet<Estudiante> Estudiante { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<MetodoPago> MetodoPago { get; set; }
        public DbSet<PayPal> PayPal { get; set; }
        public DbSet<TipoMaterial> TipoMaterial { get; set; }
        public DbSet<Matricula_Asignatura> Matricula_Asignatura { get; set; }
        public DbSet<Matricula> Matricula { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
