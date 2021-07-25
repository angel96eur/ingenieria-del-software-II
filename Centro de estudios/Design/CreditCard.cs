//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Centro_de_estudios.Models
{
    public class CreditCard: MetodoPago
    {


        [RegularExpression(@"^[0-9]{16}$")]
        [Display(Name ="Credit Card")]
        public virtual string CreditCardNumber { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]

        public virtual DateTime ExpirationDate { get; set; }
    }
}
