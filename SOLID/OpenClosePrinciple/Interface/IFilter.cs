namespace OpenClosePrinciple.Interface
{
    using System.Collections.Generic;

    public interface IFilter<T> {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }
}
