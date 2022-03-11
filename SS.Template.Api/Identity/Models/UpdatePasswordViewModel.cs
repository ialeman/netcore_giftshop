using System.ComponentModel.DataAnnotations;
using SS.Template.Core;

namespace SS.Template.Api.Identity.Models
{
    public class UpdatePasswordViewModel : ChangePasswordViewModel
    {
        [Required]
        [StringLength(AppConstants.EmailLength)]
        public virtual string Email { get; set; }
    }
}
