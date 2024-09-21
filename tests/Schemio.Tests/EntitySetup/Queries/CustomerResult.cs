namespace Schemio.Tests.EntitySetup.Queries
{
    public class CustomerResult : IQueryResult
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }
}