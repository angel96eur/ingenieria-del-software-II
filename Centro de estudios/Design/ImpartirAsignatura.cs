//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Centro_de_estudios.Models
{
    public class ImpartirAsignatura
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
        public virtual int cantidadAsignatura
        {
            get;
            set;
        }

        public virtual int AsignaturaId
        {
            get;
            set;
        }

        [ForeignKey("ImpartirId")]
        public virtual Impartir Impartir
        {
            get;
            set;
        }

        public virtual int ImpartirId
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            ImpartirAsignatura impartirAsignatura = obj as ImpartirAsignatura;
            if ((this.cantidadAsignatura == impartirAsignatura.cantidadAsignatura) && (this.ImpartirId == impartirAsignatura.ImpartirId) && (this.AsignaturaId == impartirAsignatura.AsignaturaId) && (this.AsignaturaId == impartirAsignatura.AsignaturaId) && (this.Impartir == impartirAsignatura.Impartir))
                return true;
            return false;
        }
    }
}
