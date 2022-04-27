using Core;
using Core.Domain;
using Core.Repository;
using Core.Service;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IBaseRepository<Currency> Currencies { get; private set; }

        public IExchangeHistoryService ExchangeHistoryService { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Currencies = new BaseRepository<Currency>(_context);
            ExchangeHistoryService = new ExchangeHistoryService(_context);

        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
