using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models.CompraViewModels
{
    public class SelectMaterialesForPurchaseViewModel
    {
        public IEnumerable<Material> Materiales { get; set; }

        //needed to populate a dropdownlist 
        public SelectList TipoMateriales;
        //needed to store the genre selected by the user
        [Display(Name = "Tipo de Material")]
        public string tipomaterialSelected { get; set; }

        [Display(Name = "Titulo")]
        public string materialTitulo { get; set; }
    }
}
