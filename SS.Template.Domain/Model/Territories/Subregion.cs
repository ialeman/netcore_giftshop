using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SS.Template.Model.Territories
{
    public class Subregion : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public Guid RegionId { get; set; }

        public Region Region { get; set; }

        public ICollection<Country> Countries { get; set; }
    }
}
