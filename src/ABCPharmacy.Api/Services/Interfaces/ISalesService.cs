using ABCPharmacy.Api.Models;

namespace ABCPharmacy.Api.Services.Interfaces;

public interface ISalesService
{
    Task<Sale> RecordSaleAsync(Guid medicineId, int quantity);
    Task<List<Sale>> GetAllAsync();
}