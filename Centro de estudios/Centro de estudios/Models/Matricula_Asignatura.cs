using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Centro_de_estudios.Models
{
    public class Matricula_Asignatura
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        [ForeignKey("AsignaturaId")]
        public virtual Asignatura Asignatura
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Meses Docencia")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, introduce una cantidad igual o superior a los meses minimos")]
        public virtual int MesesDocencia
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Cantidad")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, introduce una cantidad superior a 1")]
        public virtual int cantidad {
            get;
            set;
        }
        public virtual int AsignaturaId
        {
            get;
            set;
        }

        [ForeignKey("MatriculaId")]
        public virtual Matricula Matricula
        {
            get;
            set;
        }

        public virtual int MatriculaId
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            Matricula_Asignatura matriculaAsignatura = obj as Matricula_Asignatura;
            if ((this.MesesDocencia == matriculaAsignatura.MesesDocencia) && (this.MatriculaId == matriculaAsignatura.MatriculaId) && (this.AsignaturaId == matriculaAsignatura.AsignaturaId)
                && (this.Asignatura == matriculaAsignatura.Asignatura) && (this.Matricula == matriculaAsignatura.Matricula))
                return true;
            return false;
        }
    }
}
