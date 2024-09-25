namespace Schemio.SQL.Tests.EntitySetup
{
    internal class CustomerContext : IEntityContext
    {
        public int CustomerId { get; set; }
        public string[] Paths { get; set; }
    }
}