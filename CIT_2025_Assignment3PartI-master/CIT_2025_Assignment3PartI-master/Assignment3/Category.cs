using System.Collections.Generic;
using System.Linq;

namespace Assignment3
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(int cid, string name)
        {
            this.Id = cid;
            this.Name = name;
        }
    }

    public class CategoryService
    {
        private List<Category> categories = new List<Category>();

        public List<Category> GetCategories()
        {
            return new List<Category>(categories);
        }

        public Category? GetCategory(int cid)
        {
            return categories.FirstOrDefault(c => c.Id == cid);
        }

        public bool UpdateCategory(int id, string newName)
        {
            Category? category = categories.FirstOrDefault(c => c.Id == id);

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
            if (category != null)
            {
                categories.Remove(category);
                return true;
            }

            return false;
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
