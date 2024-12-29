namespace BespokeBike.SalesTracker.API.Model
{
    public interface IProduct
    {
         int ProductId { get; set; }
         string Name { get; set; }
         string Manufacturer { get; set; }
         string Style { get; set; }
         int QuantityOnHand { get; set; }
         bool IsActive { get; set; }
    }
    public class Product : IProduct
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Style { get; set; }
        public int QuantityOnHand { get; set; }
        public bool IsActive { get; set; }
    }
}

