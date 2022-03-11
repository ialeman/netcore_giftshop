using System.ComponentModel.DataAnnotations;
using SS.Template.Core;

namespace SS.Template.Api.Identity.Models
{
    public abstract class EmailModelBase
    {
        [Required]
        [StringLength(AppConstants.EmailLength)]
        //[Display(Name = nameof(AppResources.Email), ResourceType = typeof(AppResources))]
        public virtual string Email { get; set; }
    }
}
