using Schemio.Core;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerRecord : IQueryResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}