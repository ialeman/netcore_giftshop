using System;
using Microsoft.AspNetCore.Identity;

namespace SS.Template.Api.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public const string Admin = "Admin";
        public const string Terminal = "Terminal";
    }
}
