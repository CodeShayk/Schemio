namespace Schemio.Core.Tests.EntitySetup
{
    internal class CustomerContext : IEntityRequest
    {
        public int CustomerId { get; set; }
        public string[] SchemaPaths { get; set; }
    }
}