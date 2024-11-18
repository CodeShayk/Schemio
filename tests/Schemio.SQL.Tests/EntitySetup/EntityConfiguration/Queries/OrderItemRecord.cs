using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class OrderItemRecord : IQueryResult
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}