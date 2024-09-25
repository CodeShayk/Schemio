namespace Schemio.SQL.Tests.EntitySetup
{
    internal class CustomerContext : IEntityContext
    {
        public object EntityId { get; set; }
        public string[] Paths { get; set; }
    }
}