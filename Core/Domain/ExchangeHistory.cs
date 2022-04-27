using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class ExchangeHistory:BaseEntity
    {
        [Required]
        public DateTime ExchangeDate { get; set; } 
        [Required]
        public decimal Rate { get; set; }
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
    }
}
