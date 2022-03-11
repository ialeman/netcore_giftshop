using System.ComponentModel.DataAnnotations;

namespace SS.Template.Api.Identity.Models
{
    public class LoginViewModel : EmailModelBase
    {
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
    }
}
