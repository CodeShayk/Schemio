using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class OrderRecord : IQueryResult
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
    }
}