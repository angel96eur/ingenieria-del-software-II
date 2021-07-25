using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Centro_de_estudios.Models
{
    public class Impartir
    {
        [Key]
        public int ImpartirID
        {
            get;
            set;
        }

        [ForeignKey("ProfesorId")]
        public virtual Profesor Profesor
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

        public Impartir()
        {
            ImpartirAsignaturas = new List<ImpartirAsignatura>();
        }

        public override bool Equals(object obj)
        {
            Impartir impartir = obj as Impartir;
            int i;
            bool result = false;

            result = ((this.Profesor.Nombre == impartir.Profesor.Nombre)
                && (this.ImpartirAsignaturas.Count == impartir.ImpartirAsignaturas.Count));

            for(i=0; i<this.ImpartirAsignaturas.Count; i++)
            {
                result = result && this.ImpartirAsignaturas[i].Equals(impartir.ImpartirAsignaturas[i]);
            }
            return result;

        }
    }
}
