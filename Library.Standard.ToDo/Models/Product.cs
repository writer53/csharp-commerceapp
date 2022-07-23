namespace Library.TaskManagement.Models
{
    public class Product 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }

        public int CartId { get; set; } //for matching cart item to inventory

        public double Price { get; set; }

        public bool Bogo { get; set; } //1 is bogo, 0 is not

        public virtual double TotalPrice
        {
            get
            {
                return -1;
            }
        }


        public override string ToString()
        {
            return $"{Id} {Name} :: {Description}";
        }

        //public Item(ItemViewModel ivm)
        //{

        //}
    }
}
