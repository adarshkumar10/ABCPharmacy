namespace ABCPharmacy.Api.DTOs;

public class SaleCreateDto
{
    public Guid MedicineId { get; set; }
    public int Quantity { get; set; }
}