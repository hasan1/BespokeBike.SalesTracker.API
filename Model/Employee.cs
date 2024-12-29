using System;

namespace BespokeBike.SalesTracker.API.Model
{
    public interface IEmployee
    {
         int EmployeeId { get; set; }
         string FirstName { get; set; }
         string LastName { get; set; }
         string Address { get; set; }
         string Phone { get; set; }
         DateTime StartDate { get; set; }
         DateTime? TerminationDate { get; set; }
         int? Manager { get; set; }
         bool IsActive { get; set; }
    }
    public class Employee : IEmployee
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

