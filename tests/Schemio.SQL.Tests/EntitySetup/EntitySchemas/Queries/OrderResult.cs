using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class OrderResult : IQueryResult
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
    }
}