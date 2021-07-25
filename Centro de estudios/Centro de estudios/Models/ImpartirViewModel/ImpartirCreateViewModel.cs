using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models.ImpartirViewModel
{
    public class ImpartirCreateViewModel
    {
        public virtual string Nombre
        {
            get;
            set;
        }

        [Display(Name ="Primer Apellido")]
        public virtual string PrimerApellido
        {
            get;
            set;
        }

        [Display(Name = "Segundo apellido")]
        public virtual string SegundoApellido
        {
            get;
            set;
        }

        public string ProfesorId
        {
            get;
            set;
        }

        public int mesesDocenciaTotal
        {
            get;
            set;
        }

        public DateTime fecha
        {
            get;
            set;
        }

        public virtual IList<ImpartirAsignatura> ImpartirAsignaturas
        {
            get;
            set;
        }

        public ImpartirCreateViewModel()
        {
            ImpartirAsignaturas = new List<ImpartirAsignatura>(); 
        }

        public override bool Equals(object obj)
        {
            ImpartirCreateViewModel impartir = obj as ImpartirCreateViewModel;
            int i;
            bool result = false;


            result = ((this.Nombre == impartir.Nombre)
                && (this.PrimerApellido == impartir.PrimerApellido)
                && (this.SegundoApellido == impartir.SegundoApellido)
                && (this.ProfesorId == impartir.ProfesorId)
                && (this.mesesDocenciaTotal == impartir.mesesDocenciaTotal)
                && (this.fecha == impartir.fecha));

            result = result && (this.ImpartirAsignaturas.Count == impartir.ImpartirAsignaturas.Count);


            for (i = 0; i < this.ImpartirAsignaturas.Count; i++)
                result = result && (this.ImpartirAsignaturas[i].Equals(impartir.ImpartirAsignaturas[i]));

            return result;
        }
    }
}
