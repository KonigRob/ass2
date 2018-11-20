using Microsoft.AspNetCore.Identity;
using System;

namespace TodoApi.Models
{
    public class TodoItem : IdentityUser
    {
        public TodoItem() : base() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsNaughty { get; set; }
        public DateTime DateCreated { get; set; }
    }
}