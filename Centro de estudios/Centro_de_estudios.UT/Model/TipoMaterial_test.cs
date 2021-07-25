using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Centro_de_estudios.Models;
using Xunit;

namespace Centro_de_estudios.UT.Model
{
    public class TipoMaterial_test
    {
        [Fact]
        public void CanChangeTipoMaterialNombre()
        {
            var p = new TipoMaterial { Nombre = "Test" };
            p.Nombre = "Nuevo Nombre";
            Assert.Equal("Nuevo Nombre", p.Nombre);
        }
    }
}
