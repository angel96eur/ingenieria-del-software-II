using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Centro_de_estudios.Models;
using Xunit;

namespace Centro_de_estudios.UT.Model
{
    public class Intensificacion_test
    {
        [Fact]
        public void CanChangeIntensificacionNombre()
        {
            var p = new Asignatura { NombreAsignatura = "Test" };
            p.NombreAsignatura = "Nuevo Nombre";
            Assert.Equal("Nuevo Nombre", p.NombreAsignatura);
        }
    }
}
