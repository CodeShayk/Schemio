using Schemio.Core;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    public class OrderItemResult : IQueryResult
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}