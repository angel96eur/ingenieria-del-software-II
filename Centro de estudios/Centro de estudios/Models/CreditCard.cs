using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models
{
    public class CreditCard : MetodoPago
    {
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "You have to introduce 16 numbers")]
        [Display(Name = "Credit Card")]
        [Required]
        public virtual string CreditCardNumber { get; set; }

        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "You have to introduce 3 numbers")]
        [Required]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy")]
        public virtual DateTime ExpirationDate
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            CreditCard p = (CreditCard)obj;
            return (base.Equals(obj) && p.CreditCardNumber == CreditCardNumber && p.CCV == CCV && p.ExpirationDate == ExpirationDate);
        }
    }
}
