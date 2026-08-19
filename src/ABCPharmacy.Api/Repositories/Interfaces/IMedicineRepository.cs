using ABCPharmacy.Api.Models;

namespace ABCPharmacy.Api.Repositories.Interfaces;

public interface IMedicineRepository
{
    Task<List<Medicine>> GetAllAsync();
    Task<Medicine?> GetByIdAsync(Guid id);
    Task AddAsync(Medicine medicine);
    Task UpdateAsync(Medicine medicine);
}