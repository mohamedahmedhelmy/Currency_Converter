using Core.Domain;
using Core.Repository;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Currency> Currencies { get; }
        IExchangeHistoryService ExchangeHistoryService { get; }
        int Complete();
    }
}
