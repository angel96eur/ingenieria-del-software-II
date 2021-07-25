using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Centro_de_estudios.Models
{
    public class Matricula
    {
        [Key]
        public int MatriculaId
        {
            get;
            set;
        }

        [ForeignKey("EstudianteId")]
        public virtual Estudiante Estudiante
        {
            get;
            set;
        }

        public string EstudianteId
        {
            get;
            set;
        }

        public double PrecioTotal
        {
            get;
            set;
        }

        public DateTime FechaMatricula
        {
            get;
            set;
        }

        public virtual IList<Matricula_Asignatura> MatriculaAsignaturas
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca una direccion")]
        public String Residencia
        {
            get;
            set;
        }

        [Required()]
        public MetodoPago MetodoPago
        {
            get;
            set;
        }

        public Matricula()
        {
            MatriculaAsignaturas = new List<Matricula_Asignatura>();
        }

        public override bool Equals(object obj)
        {
            Matricula matricula = obj as Matricula;
            int i;
            bool result = false;

            result = ((this.Estudiante.Nombre == matricula.Estudiante.Nombre)
                && (this.Residencia == matricula.Residencia)
                && (this.MetodoPago == matricula.MetodoPago)
                && (this.FechaMatricula.Subtract(matricula.FechaMatricula).TotalMinutes < 5));

            result = result && (this.MatriculaAsignaturas.Count == matricula.MatriculaAsignaturas.Count);

            for (i = 0; i < this.MatriculaAsignaturas.Count; i++)
                result = result && (this.MatriculaAsignaturas[i].Equals(matricula.MatriculaAsignaturas[i]));

            return result;
        }
    }
}
