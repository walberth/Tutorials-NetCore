namespace OpenClosePrinciple
{
    using System.Collections.Generic;

    public class ProductFilter {
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Enums.Color color) {
            foreach (var product in products) {
                if (product.Color == color) {
                    yield return product;
                }
            }
        }

        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Enums.Size size) {
            foreach (var product in products) {
                if (product.Size == size) {
                    yield return product;
                }
            }
        }
    }
}
