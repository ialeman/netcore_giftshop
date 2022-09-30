using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SS.Template.Model.Territories
{
    public class Region : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public ICollection<Subregion> Subregions { get; set; }
    }
}
