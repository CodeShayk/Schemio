using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerRecord : IQueryResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}