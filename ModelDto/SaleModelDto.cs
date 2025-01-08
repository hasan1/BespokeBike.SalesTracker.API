namespace BespokeBike.SalesTracker.API.ModelDto
{
    public class SaleGetDto
    {
        public int SaleId { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsActive { get; set; }
    }

    public class SaleCreateDto
    {
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal TotalAmount { get; set; }       
    }

    public class SaleUpdateDto
    {
        public int SaleId { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
