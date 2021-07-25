using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro_de_estudios.Models
{
    public class Profesor : ApplicationUser
    {
        public virtual string Estudios {
            get;
            set;
        }
    }
}
