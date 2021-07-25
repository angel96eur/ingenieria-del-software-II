//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Centro_de_estudios.Models
{
    public class Material
    {
        [Key]
        public virtual int MaterialID
        {
            get;
            set;
        }

        [Required]
        [StringLength(70, ErrorMessage = "El titulo no puede tener mas de 70 caracteres.")]
        public virtual String Titulo
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "El precio minimo es 1 ")]
        [Display(Name = "Precio de compra")]
        public virtual int PrecioCompra
        {
            get;
            set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de lanzamiento")]
        public virtual DateTime FechaLanzamiento
        {
            get;
            set;
        }

        [Required]
        public virtual TipoMaterial TipoMaterial
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Cantidad para comprar")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad minima es 1")]
        public virtual int CantidadCompra
        {
            get;
            set;
        }

        public virtual IList<CompraMaterial> CompraMateriales
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {

            var myObject = obj as Material;

            if (null != myObject)
            {
                return this.MaterialID == myObject.MaterialID
                   && this.Titulo == myObject.Titulo
                   && this.PrecioCompra == myObject.PrecioCompra
                   && this.CantidadCompra == myObject.CantidadCompra
                   && this.FechaLanzamiento == myObject.FechaLanzamiento
                   && this.TipoMaterial.Nombre == myObject.TipoMaterial.Nombre;
            }
            else
            {
                return false;
            }
        }
    }
}
