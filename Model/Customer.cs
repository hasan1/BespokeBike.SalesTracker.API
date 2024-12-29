namespace BespokeBike.SalesTracker.API.Model
{

    public interface ICustomer
    {
        int CustomerId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Address { get; set; }
        string Phone { get; set; }
        DateTime StartDate { get; set; }
        string Email { get; set; }
        bool IsActive { get; set; }
    }

    public class Customer : ICustomer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
