using System;

namespace Assignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Web Service :-)");

            var categoryService = new CategoryService();

            categoryService.CreateCategory(1, "Håndsprit");

            var category = categoryService.GetCategory(1);
            if (category != null)
            {
                Console.WriteLine($"Category found: id={category.cid}, name={category.name}");
            }
            else
            {
                Console.WriteLine("Category not found.");
            }
        }
    }
}