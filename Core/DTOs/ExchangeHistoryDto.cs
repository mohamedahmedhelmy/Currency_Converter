using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ExchangeHistoryDto
    {
        [Required(ErrorMessage = "Please enter Exchange Date")]
        public DateTime ExchangeDate { get; set; }
        [Required(ErrorMessage ="Please enter Rate By Us Dolar")]
        [Range(0.00000001,10,ErrorMessage ="Rate must be Greater than Zero")]
        public decimal Rate { get; set; }
        [Required(ErrorMessage ="please enter The Currency Id ")]
        public int CurrencyId { get; set; }
    }
}
