using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models
{
    public class Asignatura
    {
        [Key]
        public virtual int AsignaturaID
        {
            get;
            set;
        }

        [Required]
        [StringLength(35, ErrorMessage = "No puede tener mas de 35 caracteres")]
        public virtual String NombreAsignatura
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1,75, ErrorMessage = "Entre 1 y 75")]
        public virtual int Precio
        {
            get;
            set;
        }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Cantidad alumnos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "seleccione al menos una asignatura")]
        public virtual int cantidadAlumnos {
            get;
            set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaComienzo
        {
            get;
            set;
        }

        public virtual IList<Matricula_Asignatura> MatriculaAsignaturas
        {
            get;
            set;
        }

        public virtual IList<ImpartirAsignatura> ImpartirAsignaturas
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Cantidad Compra")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, seleccione al menos una asignatura")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimo 1 mes")]
        public virtual int MinimoMesesDocencia
        {
            get;
            set;
        }

        [Required]
        public virtual Intensificacion Intensificacion
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {
            var myObject = obj as Asignatura;

            if(null != myObject)
            {
                return this.AsignaturaID == myObject.AsignaturaID
                    && this.NombreAsignatura == myObject.NombreAsignatura
                    && this.Precio == myObject.Precio
                    && this.MinimoMesesDocencia == myObject.MinimoMesesDocencia
                    && this.FechaComienzo == myObject.FechaComienzo
                    && this.Intensificacion.Equals(myObject.Intensificacion);
                    
            }
            else
            {
                return false;
            }
        }
    }
}
