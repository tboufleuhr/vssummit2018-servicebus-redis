using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Domain
{
    public class UserProfile : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public byte[] Photo { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
