using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models.CompraViewModels
{
    public class CompraCreateViewModel
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

        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
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

        public DateTime FechaCompra
        {
            get;
            set;
        }

        public virtual IList<CompraMaterial> CompraMateriales
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Direccion de Envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]

        public String DireccionEnvio
        {
            get;
            set;
        }

        
        [Display(Name = "Metodo de Pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, select your payment method for delivery")]
        public String MetodoPago
        {
            get;
            set;
        }

        public CompraCreateViewModel()
        {

            CompraMateriales = new List<CompraMaterial>();
        }
        public CreditCard CreditCard { get; set; }
        public PayPal PayPal { get; set; }

        public override bool Equals(object obj)
        {
            CompraCreateViewModel purchase = obj as CompraCreateViewModel;
            int i;
            bool result = false;


            result = ((this.Nombre == purchase.Nombre)
                && (this.PrimerApellido == purchase.PrimerApellido)
                && (this.SegundoApellido == purchase.SegundoApellido)
                && (this.EstudianteId == purchase.EstudianteId)
                && (this.PrecioTotal == purchase.PrecioTotal)
                && (this.DireccionEnvio == purchase.DireccionEnvio)
                //the timepsan is less than a minute between them
                && (this.FechaCompra.Subtract(purchase.FechaCompra) < new TimeSpan(0, 1, 0))
                && (
                   ((this.MetodoPago == null) && (purchase.MetodoPago == null))
                   || this.MetodoPago.Equals(purchase.MetodoPago)
                )
                );


            result = result && (this.CompraMateriales.Count == purchase.CompraMateriales.Count);


            for (i = 0; i < this.CompraMateriales.Count; i++)
                result = result && (this.CompraMateriales[i].Equals(purchase.CompraMateriales[i]));

            return result;
        }
    }
}
