namespace Schemio.Core.Tests.EntitySetup.Queries
{
    internal class OrderItemParameter : IQueryParameter
    {
        public List<int> OrderIds { get; set; }
    }
}