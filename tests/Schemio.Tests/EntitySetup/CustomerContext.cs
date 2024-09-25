namespace Schemio.Tests.EntitySetup
{
    internal class CustomerContext : IEntityContext
    {
        public int CustomerId { get; set; }
        public string[] SchemaPaths { get; set; }
    }
}