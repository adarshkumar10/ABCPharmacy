using System.Text.Json;
using System.Threading;
using ABCPharmacy.Api.Models;
using ABCPharmacy.Api.Repositories.Interfaces;

namespace ABCPharmacy.Api.Repositories;

public class SalesRepository : ISalesRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _mutex = new(1,1);
    private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

    public SalesRepository(string filePath) => _filePath = filePath;

    private async Task<List<Sale>> ReadAllAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<Sale>();
            return JsonSerializer.Deserialize<List<Sale>>(json) ?? new List<Sale>();
        }
        finally
        {
            _mutex.Release();
        }
    }

    private async Task WriteAllAsync(List<Sale> list)
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

    public Task<List<Sale>> GetAllAsync() => ReadAllAsync();

    public async Task AddAsync(Sale sale)
    {
        var all = await ReadAllAsync();
        all.Add(sale);
        await WriteAllAsync(all);
    }
}