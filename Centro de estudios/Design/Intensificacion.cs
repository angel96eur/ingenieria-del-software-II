//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models
{
    public class Intensificacion
    {
        [Key]
        public virtual int IntesificacionID
        {
            get;
            set;
        }

        [Required]
        public virtual String NombreIntesificacion
        {
            get;
            set;
        }

        public virtual ICollection<Asignatura> Asignaturas
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            var myObject = obj as Intensificacion;

            if(null != myObject)
            {
                return this.IntesificacionID == myObject.IntesificacionID
                    && this.NombreIntesificacion == myObject.NombreIntesificacion
                    && this.Asignaturas == myObject.Asignaturas;
            }

            else
            {
                return false;
            }
        }

    }
}
