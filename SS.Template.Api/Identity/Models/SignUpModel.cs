using System.ComponentModel.DataAnnotations;

namespace SS.Template.Api.Identity.Models
{
    public class SignUpModel : EmailModelBase
    {
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
    }
}
