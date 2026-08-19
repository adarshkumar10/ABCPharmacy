using ABCPharmacy.Api.Models;
using ABCPharmacy.Api.Repositories.Interfaces;
using ABCPharmacy.Api.Services.Interfaces;

namespace ABCPharmacy.Api.Services;

public class MedicineService : IMedicineService
{
    private readonly IMedicineRepository _repo;
    public MedicineService(IMedicineRepository repo) => _repo = repo;

    public async Task<List<Medicine>> GetAllAsync(string? search = null)
    {
        var all = await _repo.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLowerInvariant();
            return all.Where(m => m.FullName.ToLowerInvariant().Contains(search)).ToList();
        }
        return all.OrderBy(m => m.FullName).ToList();
    }

    public Task<Medicine?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task<Medicine> AddAsync(Medicine medicine)
    {
        if (medicine.Price < 0) throw new InvalidOperationException("Price cannot be negative.");
        if (medicine.Quantity < 0) throw new InvalidOperationException("Quantity cannot be negative.");
        if (medicine.ExpiryDate <= DateTime.UtcNow) throw new InvalidOperationException("Expiry date must be in the future.");

        await _repo.AddAsync(medicine);
        return medicine;
    }

    public Task UpdateAsync(Medicine medicine) => _repo.UpdateAsync(medicine);
}