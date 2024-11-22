using Schemio.Core;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    public class OrderRecord : IQueryResult
    {
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
    }
}