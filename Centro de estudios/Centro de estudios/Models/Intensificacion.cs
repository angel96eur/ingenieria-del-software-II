using Microsoft.AspNetCore.Mvc;
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
        public virtual int IntensificacionID
        {
            get;
            set;
        }

        [Required]
        public virtual String NombreIntensificacion
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
                return this.IntensificacionID == myObject.IntensificacionID
                    && this.NombreIntensificacion == myObject.NombreIntensificacion
                    && this.Asignaturas.Equals(myObject.Asignaturas);
            }

            else
            {
                return false;
            }
        }
    }
}
