using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models.MatriculaViewModels
{
    public class SelectAsignaturasForMatriculaViewModel
    {
        public IEnumerable<Asignatura> Asignaturas { get; set; }
 
        public SelectList Intensificaciones;
        
        [Display(Name = "Intensificacion")]
        public string asignaturaIntensificacionSelected { get; set; }

        [Display(Name = "Nombre_asignatura")]
        public string asignaturaNombre { get; set; }
    }
}
