using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models
{
    public class PayPal : MetodoPago
    {
        [EmailAddress]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Paypal email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your email of paypal")]
        public string Email { get; set; }

        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Paypal prefix")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your prefix of paypal")]
        [StringLength(3, MinimumLength = 2)]
        public string Prefix { get; set; }


       
        [DataType(DataType.MultilineText)]
        [Display(Name = "Paypal phone")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your phone of paypal")]
        [StringLength(7, MinimumLength = 6)]

        public string Phone { get; set; }

        public override bool Equals(object obj)
        {
            PayPal p = (PayPal)obj;
            return (p.Email == Email && p.Prefix == Prefix && p.Phone == Phone);
        }
    }
}
