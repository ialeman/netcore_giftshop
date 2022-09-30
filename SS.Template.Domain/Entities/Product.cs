using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SS.Template.Model;

namespace SS.Template.Domain.Entities
{
    public class Product: EntityBase
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }

    }
}
