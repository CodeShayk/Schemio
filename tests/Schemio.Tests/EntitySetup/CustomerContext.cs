namespace Schemio.Tests.EntitySetup
{
    internal class CustomerContext : IEntityContext
    {
        public decimal CurrentVersion => 1;
        public int CustomerId { get; set; }

        public object EntityId { get; set; }

        public string[] Paths { get; set; }
    }
}