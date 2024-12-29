using BespokeBike.SalesTracker.API.Model;
using BespokeBike.SalesTracker.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BespokeBike.SalesTracker.API.Service
{
    public interface ISaleService
    {
        Task<IEnumerable<Sale>> GetAllSales();
        Task<Sale?> GetSaleById(int saleId);
        Task<int> AddSale(Sale sale);
        Task<bool> UpdateSale(Sale sale);
        Task<bool> DeleteSale(int saleId);
    }

    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public Task<IEnumerable<Sale>> GetAllSales()
        {
            return _saleRepository.GetAllSales();
        }

        public Task<Sale?> GetSaleById(int saleId)
        {
            return _saleRepository.GetSaleById(saleId);
        }

        public Task<int> AddSale(Sale sale)
        {
            return _saleRepository.AddSale(sale);
        }

        public Task<bool> UpdateSale(Sale sale)
        {
            return _saleRepository.UpdateSale(sale);
        }

        public Task<bool> DeleteSale(int saleId)
        {
            return _saleRepository.DeleteSale(saleId);
        }
    }
}
