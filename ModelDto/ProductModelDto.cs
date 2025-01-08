namespace BespokeBike.SalesTracker.API.ModelDto
{
    public class ProductGetDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Style { get; set; }
        public int QuantityOnHand { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Style { get; set; }
        public int QuantityOnHand { get; set; }      
    }

    public class ProductUpdateDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Style { get; set; }
        public int QuantityOnHand { get; set; }
        public bool IsActive { get; set; }
    }
}

