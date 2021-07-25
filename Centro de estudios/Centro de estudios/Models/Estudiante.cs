using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Centro_de_estudios.Models
{
    public class Estudiante : ApplicationUser
    {
        public virtual IList<Matricula> Matricula
        {
            get;
            set;
        }

        public virtual IList<Compra> Compra
        {
            get;
            set;
        }
    }
}
