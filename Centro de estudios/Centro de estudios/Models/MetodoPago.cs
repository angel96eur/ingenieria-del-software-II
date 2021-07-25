using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models
{
    public class MetodoPago
    {
        [Key]
        public virtual int ID
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            MetodoPago p = (MetodoPago)obj;
            return (p.ID == ID);
        }
    }
}
