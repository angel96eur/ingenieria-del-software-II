//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Centro_de_estudios.Models
{
    public class CompraMaterial
    {
        [Key]
        public int ID
        {
            get;
            set;
        }

        public virtual int Cantidad
        {
            get;
            set;
        }

        [ForeignKey("MaterialID")]
        public virtual Material Material
        {
            get;
            set;
        }

        public virtual int MaterialID
        {
            get;
            set;
        }

        [ForeignKey("CompraID")]
        public virtual Compra Compra
        {
            get;
            set;
        }

        public virtual int CompraID
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {

            CompraMaterial compraMaterial = obj as CompraMaterial;
            if ((this.Cantidad == compraMaterial.Cantidad) && (this.CompraID == compraMaterial.CompraID) && (this.MaterialID == compraMaterial.MaterialID)
                && (this.Material == compraMaterial.Material) && (this.Compra == compraMaterial.Compra))
                return true;
            return false;
        }
    }
}
