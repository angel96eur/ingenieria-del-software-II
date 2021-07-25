using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Centro_de_estudios.Models.MatriculaViewModels
{
    public class MatriculaCreateViewModel
    {
        public virtual string Nombre
        {
            get;
            set;
        }
        [Display(Name = "Primer Apellido")]
        public virtual string PrimerApellido
        {
            get;
            set;
        }

        [Display(Name = "Segundo Apellido")]
        public virtual string SegundoApellido
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

        public DateTime MatriculaFecha
        {
            get;
            set;
        }

        public virtual IList<Matricula_Asignatura> Matricula_Asignaturas
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Direccion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduzca su direccion")]

        public String Direccion
        {
            get;
            set;
        }

        [Display(Name = "Metodo pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, seleccione una forma de pago")]
        public String MetodoPago
        {
            get;
            set;
        }

        public MatriculaCreateViewModel()
        {

            Matricula_Asignaturas = new List<Matricula_Asignatura>();
        }
        public CreditCard CreditCard { get; set; }
        public PayPal PayPal { get; set; }

        public override bool Equals(object obj)
        {
            MatriculaCreateViewModel matricula = obj as MatriculaCreateViewModel;
            int i;
            bool result = false;


            result = ((this.Nombre == matricula.Nombre)
                && (this.PrimerApellido == matricula.PrimerApellido)
                && (this.SegundoApellido == matricula.SegundoApellido)
                && (this.EstudianteId == matricula.EstudianteId)
                && (this.PrecioTotal == matricula.PrecioTotal)
                && (this.Direccion == matricula.Direccion)
                //the timepsan is less than a minute between them
                && (this.MatriculaFecha.Subtract(matricula.MatriculaFecha) < new TimeSpan(0, 1, 0))
                && (
                   ((this.MetodoPago == null) && (matricula.MetodoPago == null))
                   || this.MetodoPago.Equals(matricula.MetodoPago)
                )
                );


            result = result && (this.Matricula_Asignaturas.Count == matricula.Matricula_Asignaturas.Count);


            for (i = 0; i < this.Matricula_Asignaturas.Count; i++)
                result = result && (this.Matricula_Asignaturas[i].Equals(matricula.Matricula_Asignaturas[i]));

            return result;
        }
    }
}

