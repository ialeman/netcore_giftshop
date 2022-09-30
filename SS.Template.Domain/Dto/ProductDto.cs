using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Template.Domain.Dto
{
    public class ProductDto
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }
    }
}
