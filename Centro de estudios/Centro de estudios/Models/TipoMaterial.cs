using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models
{
    public class TipoMaterial
    {
        public virtual int TipoMaterialID
        {
            get;
            set;
        }

        [Required]
        public virtual String Nombre
        {
            get;
            set;
        }

        public virtual ICollection<Material> Materiales
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {

            var myObject = obj as TipoMaterial;

            if (null != myObject)
            {
                return this.TipoMaterialID == myObject.TipoMaterialID
                   && this.Nombre == myObject.Nombre
                   && this.Materiales == myObject.Materiales;
            }
            else
            {
                return false;
            }
        }

    }
}
