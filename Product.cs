using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBigData
{
    public class Product
    {
        public string id { get; set; }
        public string nameProduct { get; set; }
        public string idProduct { get; set; }
        public string urlProduct { get; set; }
        public string nameCategory { get; set; }

        public Product()
        {

        }
        public Product(string id, string nameProduct, string idProduct, string urlProduct, string nameCategory)
        {
            this.id = id;
            this.nameCategory = nameCategory;
            this.idProduct = idProduct;
            this.urlProduct = urlProduct;
            this.nameProduct = nameProduct;
        }
    }
}
