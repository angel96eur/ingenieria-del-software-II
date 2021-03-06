using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "First Surname")]
        public string FirstSurname { get; set; }

        [Display(Name = "Second Surname")]
        public string SecondSurname { get; set; }

        [Required]
        [Display(Name = "Type of User")]

        public TypeOfAppUser TypeOfUser  { get; set; }
    }

    public enum TypeOfAppUser
    {
        Administrator,
        Customer,
        Employee
    }
}
