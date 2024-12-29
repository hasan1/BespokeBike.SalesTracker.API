namespace BespokeBike.SalesTracker.API.ModelDto
{
    public class CustomerGetDto
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

    public class CustomerCreateDto
    {     
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public string Email { get; set; }
   
    }

    public class CustomerUpdateDto
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
