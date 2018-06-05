using VsSummit2018.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace VsSummit2018.Application.Resources
{
    public class UserProfileUpdateInfo
    {
        public int UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
