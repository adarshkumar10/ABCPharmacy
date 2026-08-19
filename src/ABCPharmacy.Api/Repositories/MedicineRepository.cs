using System.Text.Json;
using System.Threading;
using ABCPharmacy.Api.Models;
using ABCPharmacy.Api.Repositories.Interfaces;

namespace ABCPharmacy.Api.Repositories;

public class MedicineRepository : IMedicineRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _mutex = new(1,1);
    private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

    public MedicineRepository(string filePath) => _filePath = filePath;

    private async Task<List<Medicine>> ReadAllAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<Medicine>();
            return JsonSerializer.Deserialize<List<Medicine>>(json) ?? new List<Medicine>();
        }
        finally
        {
            _mutex.Release();
        }
    }

    private async Task WriteAllAsync(List<Medicine> list)
    {
        await _mutex.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(list, _opts);
            await File.WriteAllTextAsync(_filePath, json);
        }
        finally
        {
            _mutex.Release();
        }
    }

    public Task<List<Medicine>> GetAllAsync() => ReadAllAsync();

    public async Task<Medicine?> GetByIdAsync(Guid id) => (await ReadAllAsync()).FirstOrDefault(x => x.Id == id);

    public async Task AddAsync(Medicine medicine)
    {
        var all = await ReadAllAsync();
        all.Add(medicine);
        await WriteAllAsync(all);
    }

    public async Task UpdateAsync(Medicine medicine)
    {
        var all = await ReadAllAsync();
        var idx = all.FindIndex(m => m.Id == medicine.Id);
        if (idx < 0) throw new InvalidOperationException("Medicine not found.");
        all[idx] = medicine;
        await WriteAllAsync(all);
    }
}