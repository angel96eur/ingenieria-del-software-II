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
        public string Email
        {
            get;
            set;
        }

        [StringLength(3, MinimumLength = 2)]
        public string Prefix
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            PayPal p = (PayPal)obj;
            return (p.Email == Email && p.Prefix == Prefix && p.Phone == Phone);
        }
    }
}
