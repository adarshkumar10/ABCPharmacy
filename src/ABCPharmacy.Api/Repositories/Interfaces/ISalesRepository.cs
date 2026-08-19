using ABCPharmacy.Api.Models;

namespace ABCPharmacy.Api.Repositories.Interfaces;

public interface ISalesRepository
{
    Task<List<Sale>> GetAllAsync();
    Task AddAsync(Sale sale);
}