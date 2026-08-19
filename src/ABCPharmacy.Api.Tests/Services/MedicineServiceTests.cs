using System;
using System.Linq;
using System.Threading.Tasks;
using ABCPharmacy.Api.Models;
using ABCPharmacy.Api.Repositories;
using ABCPharmacy.Api.Services;
using ABCPharmacy.Api.Tests.TestHelpers;
using Xunit;

namespace ABCPharmacy.Api.Tests.Services;

public class MedicineServiceTests : IDisposable
{
    private readonly string _filePath;
    private readonly MedicineRepository _repo;
    private readonly MedicineService _service;

    public MedicineServiceTests()
    {
        _filePath = TempFileHelper.CreateTempJsonFile(new System.Collections.Generic.List<Medicine>());
        _repo = new MedicineRepository(_filePath);
        _service = new MedicineService(_repo);
    }

    [Fact]
    public async Task AddAsync_AddsMedicine_And_CanFetch()
    {
        var med = new Medicine
        {
            FullName = "TestMed",
            Brand = "BrandX",
            ExpiryDate = DateTime.UtcNow.AddDays(60),
            Quantity = 20,
            Price = 1.5m,
            Notes = "notes"
        };

        var created = await _service.AddAsync(med);
        Assert.NotNull(created);
        var all = await _service.GetAllAsync();
        Assert.Single(all);
        Assert.Equal("TestMed", all.First().FullName);
    }

    [Fact]
    public async Task AddAsync_RejectsExpiredMedicine()
    {
        var med = new Medicine
        {
            FullName = "Expired",
            ExpiryDate = DateTime.UtcNow.AddDays(-1),
            Quantity = 1,
            Price = 1.0m
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(med));
    }

    [Fact]
    public async Task GetAll_SearchFiltersByName()
    {
        await _service.AddAsync(new Medicine { FullName = "Alpha", ExpiryDate = DateTime.UtcNow.AddDays(10), Quantity = 5, Price = 1 });
        await _service.AddAsync(new Medicine { FullName = "Beta", ExpiryDate = DateTime.UtcNow.AddDays(10), Quantity = 5, Price = 1 });

        var res = await _service.GetAllAsync("alp");
        Assert.Single(res);
        Assert.Equal("Alpha", res[0].FullName);
    }

    public void Dispose()
    {
        TempFileHelper.DeleteIfExists(_filePath);
    }
}