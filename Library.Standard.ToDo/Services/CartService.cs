using Library.TaskManagement.Models;
using Library.TaskManagement.Services;
using Library.TaskManagement.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{

    public class CartService
    {
        private string persistPath 
            = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private ListNavigator<Product> listNavigator;
        private List<Product> cartList;
        public List<Product> CartList
        {
            get
            {
                return cartList;
            }
        }

        public int NextId
        {
            get
            {
                if (!CartList.Any())
                {
                    return 1;
                }

                return CartList.Select(t => t.Id).Max() + 1;
            }
        }

        private static CartService current;

        public static CartService Current
        {
            get
            {
                if (current == null)
                {
                    current = new CartService();
                }

                return current;
            }
        }

        private CartService()
        {
            cartList = new List<Product>();

            listNavigator = new ListNavigator<Product>(cartList);

        }

        public void AddOrUpdate(Product todo)
        {
            if (todo.Id <= 0) //Add cart id
            {
                todo.Id = NextId;
                CartList.Add(todo); //adds new product to list
            }

        }

    
        public void Delete(int id)
        {
            var todoToDelete = cartList.FirstOrDefault(t => t.Id == id);
            if (todoToDelete == null)
            {
                return;
            }
            cartList.Remove(todoToDelete);
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
            cartList = JsonConvert.DeserializeObject<List<Product>>
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
            var todosJson = JsonConvert.SerializeObject(cartList
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
                    return CartList;
                }
                return CartList
                    .Where(i => string.IsNullOrEmpty(_query) ||( i.Description.Contains(_query)
                        || i.Name.Contains(_query))) //search
                    .OrderBy(i => i.Name);          //sort
            }
        }

    }
}
