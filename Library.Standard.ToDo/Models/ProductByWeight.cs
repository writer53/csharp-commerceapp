using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Models
{
    public class ProductByWeight: Product
    {
        public double Weight { get; set; }



        public override double TotalPrice
        {
            get
            {
                return Weight * Price;
            }
        }

        public override string ToString()
        {
            return $"{Id} - {Weight} {Name} :: {Description}";
        }
    }
}
