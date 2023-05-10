using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commons.DTO
{
    public class ProductDTO
    {
        public ProductDTO(Guid id, string name, string price)
        {
            this.id = id;
            this.name = name;
            this.price = price;
        }
        public ProductDTO()
        {
            
        }

        public Guid id { get; set; }
        public string name { get; set; }
        public string price { get; set; }


    }
}
