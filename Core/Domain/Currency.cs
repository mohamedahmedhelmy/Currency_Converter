using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Currency : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string CurrencyName { get; set; }
        [Required]
        [MaxLength(6)]
        public string CurrencySign { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public virtual ICollection<ExchangeHistory> ExchangeHistory { get; set; }
    }
}
