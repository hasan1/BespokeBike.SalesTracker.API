namespace BespokeBike.SalesTracker.API.Model
{
    public interface  ISale
    {
         int SaleId { get; set; }
         int EmployeeId { get; set; }
         int CustomerId { get; set; }
         DateTime SalesDate { get; set; }
         decimal TotalAmount { get; set; }
         bool IsActive { get; set; }
    }
    public class Sale : ISale
    {
        public int SaleId { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
