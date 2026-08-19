namespace ABCPharmacy.Api.Models;

public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MedicineId { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}