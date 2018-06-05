using VsSummit2018.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Application.Resources
{
    public class UserProfileInfo : CommandResponse
    {
        public int UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
