using Core.Domain;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ExchangeHistoryService :BaseRepository<ExchangeHistory>,IExchangeHistoryService
    {
        private readonly AppDbContext _Context;
        public ExchangeHistoryService(AppDbContext context) : base(context)
        {
                
            _Context = context;
        }

        public async Task<IEnumerable<ExchangeHistory>> GetByDate(DateTime Date)
        {
            return await _Context.exchangeHistory.Include(b => b.Currency).Where(b => b.ExchangeDate.Date == Date && b.Currency.IsActive == true).ToListAsync();
        }

        public async Task<IEnumerable<ExchangeHistory>> GetHighestNumberCurrencies()
        {
            var result = await _Context.exchangeHistory.Include(b => b.Currency).Where(b=>b.Currency.IsActive ==true).OrderByDescending(c => c.Rate).ToListAsync();
            if (result != null)
            {
                return result.DistinctBy(x => x.CurrencyId).ToList();
            }
            return null;
        }

        public async Task<IEnumerable<ExchangeHistory>> GetLowestNumberCurrencies()
        {
            var result = await _Context.exchangeHistory.Include(b => b.Currency).Where(b => b.Currency.IsActive == true).OrderBy(c => c.Rate).ToListAsync();
            if (result != null)
            {
                return result.DistinctBy(x => x.CurrencyId).ToList();
            }
            return null;
        }

    }
}
