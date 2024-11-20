using Schemio.Core;

namespace Schemio.API.Tests.EntitySetup.QueryResults
{
    public class OrderResult : IQueryResult
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
    }
}