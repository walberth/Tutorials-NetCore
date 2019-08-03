namespace OpenClosePrinciple
{
    using System;
    using OpenClosePrinciple.Filters;

    class Program {
        static void Main(string[] args) {
            var apple = new Product("Apple", Enums.Color.Green, Enums.Size.Small);
            var tree = new Product("Tree", Enums.Color.Green, Enums.Size.Large);
            var house = new Product("Houses", Enums.Color.Blue, Enums.Size.Large);

            Product[] products = {
                apple,
                tree,
                house
            };

            var productFilter = new ProductFilter();

            Console.WriteLine(@"Green products (OLD): ");

            foreach (var product in productFilter.FilterByColor(products, Enums.Color.Green)) {
                Console.WriteLine($"{product.Name} is green");
            }

            var betterFilter = new BetterFilter();

            Console.WriteLine(@"Green products (NEW): ");

            foreach (var product in betterFilter.Filter(products, new ColorSpecification(Enums.Color.Green))) {
                Console.WriteLine($"{product.Name} is green");
            }

            Console.WriteLine(@"Green and Small products (NEW): ");

            foreach (var product in betterFilter.Filter(products, 
                                                        new AndSpecification<Product>(new ColorSpecification(Enums.Color.Green), 
                                                                                      new SizeSpecification(Enums.Size.Small)))) {
                Console.WriteLine($"{product.Name} is green and small");
            }

            Console.ReadLine();
        }
    }

    public class Product {
        public string Name;
        public Enums.Color Color;
        public Enums.Size Size;

        public Product(string name, Enums.Color color, Enums.Size size) {
            Name = name;
            Color = color;
            Size = size;
        }
    }

    public class Enums {
        public enum Color {
            Red,
            Green,
            Blue
        }

        public enum Size {
            Small,
            Medium,
            Large,
            Yuge
        }
    }
}
