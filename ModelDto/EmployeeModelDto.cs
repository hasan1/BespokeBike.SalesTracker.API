namespace BespokeBike.SalesTracker.API.ModelDto
{
    public class EmployeeGetDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? Manager { get; set; }
        public bool IsActive { get; set; }
    }

    public class EmployeeCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? Manager { get; set; }
        public bool IsActive { get; set; }
    }

    public class EmployeeUpdateDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? Manager { get; set; }
        public bool IsActive { get; set; }
    }
}

