using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CurrencyDto
    {
        [Required(ErrorMessage ="Please Enter currency Name")]
        public string CurrencyName { get; set; }
        [Required(ErrorMessage = "Please Enter currency Sign")]
        [MaxLength(6,ErrorMessage = "currency Sign must be less Than 6 Char")]
        public string CurrencySign { get; set; }
        [Required(ErrorMessage = "Please Enter currency Is Active Or Not Active")]
        public bool IsActive { get; set; }
    }
}
