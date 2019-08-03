namespace OpenClosePrinciple.Filters
{
    using OpenClosePrinciple.Interface;

    public class SizeSpecification : ISpecification<Product> 
    {
        private Enums.Size size;
        
        public SizeSpecification(Enums.Size size) {
            this.size = size;
        }

        public bool IsSatisfied(Product product) {
            return product?.Size == size;
        }
    }
}
