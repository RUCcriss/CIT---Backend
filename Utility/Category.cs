using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Utility
{
    public class Category
    {
        [JsonPropertyName("cid")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public Category(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    public class CategoryService
    {
        private int nextId = 4;
        // initialize list with provided categories
        private List<Category> categories = new List<Category>
        {
            new Category(1, "Beverages"),
            new Category(2, "Condiments"),
            new Category(3, "Confections")
        };

        public List<Category> GetCategories()
        {
            //Returns copy to not expose list. Private only limits visibility via CategoryService.categories
            return new List<Category>(categories);
        }

        public Category? GetCategory(int cid)
        {
            return categories.FirstOrDefault(c => c.Id == cid);
        }

        public bool UpdateCategory(int id, string newName)
        {
            Category? category = GetCategory(id);

            if (category != null)
            {
                category.Name = newName;
                return true;
            }
            return false;
        }

        public bool DeleteCategory(int id)
        {
            Category? category = categories.FirstOrDefault(c => c.Id == id);
            return category != null && categories.Remove(category);
        }

        public int CreateCategory(string name)
        {
            int idChosen = nextId++;
            categories.Add(new Category(idChosen, name));
            return idChosen;
        }

        public bool CreateCategory(int id, string name)
        {
            Category? category = categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                categories.Add(new Category(id, name));
                return true;
            }
            return false;
        }
    }
}
