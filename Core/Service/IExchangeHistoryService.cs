using Core.Domain;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IExchangeHistoryService :IBaseRepository<ExchangeHistory>
    {
        Task<IEnumerable<ExchangeHistory>> GetHighestNumberCurrencies();
        Task<IEnumerable<ExchangeHistory>> GetLowestNumberCurrencies();
        Task<IEnumerable<ExchangeHistory>> GetByDate(DateTime Date);
    }
}
