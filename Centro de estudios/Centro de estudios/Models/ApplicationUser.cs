using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Centro_de_estudios.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual string Nombre
        {
            get;
            set;
        }
        [Display(Name = "Primer apellido")]
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
    }
}
