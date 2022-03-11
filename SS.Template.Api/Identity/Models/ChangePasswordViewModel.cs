using System.ComponentModel.DataAnnotations;

namespace SS.Template.Api.Identity.Models
{
    public class ChangePasswordViewModel : SetPasswordModelBase
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
