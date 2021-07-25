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

        public override bool Equals(Object obj)
        {

            var myObject = obj as Profesor;

            if (null != myObject)
            {
                return this.Nombre == myObject.Nombre
                   && this.PrimerApellido == myObject.PrimerApellido
                   && this.SegundoApellido == myObject.SegundoApellido 
                   && this.UserName == myObject.UserName
                   && this.PhoneNumber == myObject.PhoneNumber;
            }
            else
            {
                return false;
            }
        }
    }
}
