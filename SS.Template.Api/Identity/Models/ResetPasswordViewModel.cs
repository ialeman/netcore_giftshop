using System.ComponentModel.DataAnnotations;
using SS.Template.Core;

namespace SS.Template.Api.Identity.Models
{
    public class ResetPasswordViewModel : SetPasswordModelBase
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [StringLength(AppConstants.EmailLength)]
        public virtual string Email { get; set; }
    }
}
