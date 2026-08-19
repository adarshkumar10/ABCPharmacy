using ABCPharmacy.Api.Models;

namespace ABCPharmacy.Api.Services.Interfaces;

public interface IMedicineService
{
    Task<List<Medicine>> GetAllAsync(string? search = null);
    Task<Medicine?> GetByIdAsync(Guid id);
    Task<Medicine> AddAsync(Medicine medicine);
    Task UpdateAsync(Medicine medicine);
}