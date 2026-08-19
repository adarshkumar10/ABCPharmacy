using System;
using System.Linq;
using System.Threading.Tasks;
using ABCPharmacy.Api.Models;
using ABCPharmacy.Api.Repositories;
using ABCPharmacy.Api.Services;
using ABCPharmacy.Api.Tests.TestHelpers;
using Xunit;

namespace ABCPharmacy.Api.Tests.Services;

public class SalesServiceTests : IDisposable
{
    private readonly string _medFile;
    private readonly string _salesFile;
    private readonly MedicineRepository _medRepo;
    private readonly SalesRepository _salesRepo;
    private readonly SalesService _salesService;

    public SalesServiceTests()
    {
        _medFile = TempFileHelper.CreateTempJsonFile(new System.Collections.Generic.List<Medicine>());
        _salesFile = TempFileHelper.CreateTempJsonFile(new System.Collections.Generic.List<Sale>());

        _medRepo = new MedicineRepository(_medFile);
        _salesRepo = new SalesRepository(_salesFile);
        _salesService = new SalesService(_salesRepo, _medRepo);
    }

    [Fact]
    public async Task RecordSale_ReducesStock_And_CreatesSale()
    {
        var med = new Medicine
        {
            FullName = "SaleMed",
            ExpiryDate = DateTime.UtcNow.AddDays(30),
            Quantity = 10,
            Price = 2.0m
        };

        await _medRepo.AddAsync(med);

        var sale = await _salesService.RecordSaleAsync(med.Id, 3);

        Assert.Equal(3, sale.Quantity);
        var updatedMed = await _medRepo.GetByIdAsync(med.Id);
        Assert.Equal(7, updatedMed!.Quantity);

        var sales = await _salesRepo.GetAllAsync();
        Assert.Single(sales);
        Assert.Equal(sale.Id, sales.First().Id);
    }

    [Fact]
    public async Task RecordSale_Throws_When_InsufficientStock()
    {
        var med = new Medicine
        {
            FullName = "LowStock",
            ExpiryDate = DateTime.UtcNow.AddDays(30),
            Quantity = 1,
            Price = 1.0m
        };

        await _medRepo.AddAsync(med);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _salesService.RecordSaleAsync(med.Id, 2));
    }

    public void Dispose()
    {
        TempFileHelper.DeleteIfExists(_medFile);
        TempFileHelper.DeleteIfExists(_salesFile);
    }
}