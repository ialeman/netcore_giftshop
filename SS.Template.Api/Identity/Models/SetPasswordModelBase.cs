using System.ComponentModel.DataAnnotations;

namespace SS.Template.Api.Identity.Models
{
    public abstract class SetPasswordModelBase
    {
        [DataType(DataType.Password)]
        //[PasswordComplexityRegularExpression]
        public string NewPassword { get; set; }
    }
}
