using System;
using Microsoft.AspNetCore.Identity;
using SS.Template.Domain.Model;

namespace SS.Template.Api.Identity
{
    public class User : IdentityUser<Guid>, IStatus<EnabledStatus>
    {
        public EnabledStatus Status { get; set; }
    }
}
