using Schemio.Core;

namespace Schemio.EntityFramework.Tests.EntitySetup
{
    internal class CustomerRequest : IEntityRequest
    {
        public int CustomerId { get; set; }
        public string[] SchemaPaths { get; set; }
    }
}