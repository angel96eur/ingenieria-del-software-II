using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Centro_de_estudios.Models
{
    public class Compra
    {
        [Key]
        public int CompraID
        {
            get;
            set;
        }

        [ForeignKey("EstudianteID")]
        public virtual Estudiante Estudiante
        {
            get;
            set;
        }

        public string EstudianteID
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

        [DataType(DataType.MultilineText)]
        [Display(Name = "Direccion de envio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, pon tu direccion de envio")]

        public String DeireccionDeEnvio
        {
            get;
            set;
        }



        [Display(Name = "Metodo de Pago")]
        [Required()]
        public MetodoPago MetodoDePago
        {
            get;
            set;
        }

        public virtual IList<CompraMaterial> CompraMateriales
        {
            get;
            set;
        }

        public Compra()
        {

            CompraMateriales = new List<CompraMaterial>();
        }

        public override bool Equals(object obj)
        {
            Compra purchase = obj as Compra;
            int i;
            bool result = false;


            result = ((this.Estudiante.Nombre == purchase.Estudiante.Nombre)
                && (this.DeireccionDeEnvio == purchase.DeireccionDeEnvio)
                && (this.MetodoDePago == purchase.MetodoDePago)
                //if there is no more than 5 minutes
                && (this.FechaCompra.Subtract(purchase.FechaCompra).TotalMinutes < 5));

            result = result && (this.CompraMateriales.Count == purchase.CompraMateriales.Count);

            for (i = 0; i < this.CompraMateriales.Count; i++)
                result = result && (this.CompraMateriales[i].Equals(purchase.CompraMateriales[i]));

            return result;
        }
    }
}
