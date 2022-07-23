using System;

namespace Library.TaskManagement.Models
{
    public partial class ProductByQuantity: Product
    {

       // public DateTime Deadline { get; set; }

        public double Quantity { get; set; }

       // public int AssignedUser { get; set; }
       // public bool Completed { get; set; }

        public ProductByQuantity()
        {

        }

        public override double TotalPrice
        {
            get
            {
                return Quantity * Price;
            }
        }

        public override string ToString()
        {
            return $"{Id} {Quantity} - {Name} :: {Description}";
        }
    }
}