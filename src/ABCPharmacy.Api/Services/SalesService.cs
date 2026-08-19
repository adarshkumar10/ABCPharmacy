using ABCPharmacy.Api.Models;
using ABCPharmacy.Api.Repositories.Interfaces;
using ABCPharmacy.Api.Services.Interfaces;

namespace ABCPharmacy.Api.Services;

public class SalesService : ISalesService
{
    private readonly ISalesRepository _salesRepo;
    private readonly IMedicineRepository _medRepo;

    public SalesService(ISalesRepository salesRepo, IMedicineRepository medRepo)
    {
        _salesRepo = salesRepo;
        _medRepo = medRepo;
    }

    public async Task<Sale> RecordSaleAsync(Guid medicineId, int quantity)
    {
        if (quantity <= 0) throw new InvalidOperationException("Quantity must be greater than zero.");

        var med = await _medRepo.GetByIdAsync(medicineId);
        if (med == null) throw new InvalidOperationException("Medicine not found.");
        if (med.Quantity < quantity) throw new InvalidOperationException("Insufficient stock.");

        med.Quantity -= quantity;
        await _medRepo.UpdateAsync(med);

        var sale = new Sale
        {
            MedicineId = medicineId,
            Quantity = quantity,
            Total = Math.Round(med.Price * quantity, 2),
            Date = DateTime.UtcNow
        };

        await _salesRepo.AddAsync(sale);
        return sale;
    }

    public Task<List<Sale>> GetAllAsync() => _salesRepo.GetAllAsync();
}