using System;
using System.ComponentModel.DataAnnotations;

namespace SS.Template.Model.Territories
{
    public class Country : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public Guid SubregionId { get; set; }

        public Subregion Subregion { get; set; }
    }
}
