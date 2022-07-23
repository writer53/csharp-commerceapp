using Library.TaskManagement.Models;
using Library.TaskManagement.Utility;
using Library.TaskManagement.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{

    public class InventoryService
    {
        private string persistPath 
            = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private ListNavigator<Product> listNavigator;
        private List<Product> inventoryList;
        public List<Product> Products
        {
            get
            {
                return inventoryList;
            }
        }

        public int NextId
        {
            get
            {
                if (!Products.Any())
                {
                    return 1;
                }

                return Products.Select(t => t.Id).Max() + 1;
            }
        }

        private static InventoryService current;

        public static InventoryService Current
        {
            get
            {
                if (current == null)
                {
                    current = new InventoryService();
                }

                return current;
            }
        }

        private InventoryService()
        {
            inventoryList = new List<Product>();

            listNavigator = new ListNavigator<Product>(inventoryList);

        }

        public void AddOrUpdate(Product todo)
        {
            if (todo.Id <= 0)
            {
                todo.Id = NextId;
                Products.Add(todo); //adds new product to list
            }

        }

        public void MeasureChangeQuantity(Product item) //take in a cart id int
        {
            if (Products.Any(i => i.Id == item.CartId))
            {
                //get cart and change variable
            } else
            {

            }
        }

        public void MeasureChangeWeight(Product todo)
        {

        }


        public void Delete(int id)
        {
            var todoToDelete = inventoryList.FirstOrDefault(t => t.Id == id);
            if (todoToDelete == null)
            {
                return;
            }
            inventoryList.Remove(todoToDelete);
        }

        public void Load(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }

            var todosJson = File.ReadAllText(fileName);
            inventoryList = JsonConvert.DeserializeObject<List<Product>>
                (todosJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();

        }

        public void Save(string fileName = null)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            } else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }
            var todosJson = JsonConvert.SerializeObject(inventoryList
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, todosJson);
        }

        //GROSS
        //public IEnumerable<Item> Search(string query)
        //{
        //    return Items.Where(i => i.Description.Contains(query) || i.Name.Contains(query));
        //}

        //Stateful Implementation
        private string _query;
        private bool _sort;

        public IEnumerable<Product> Search(string query)
        {
            _query = query;
            return ProcessedList;
        }

        public IEnumerable<Product> ProcessedList{
            get
            {
                if(string.IsNullOrEmpty(_query))
                {
                    return Products;
                }
                return Products
                    .Where(i => string.IsNullOrEmpty(_query) ||( i.Description.Contains(_query)
                        || i.Name.Contains(_query))) //search
                    .OrderBy(i => i.Name);          //sort
            }
        }

    }
}
