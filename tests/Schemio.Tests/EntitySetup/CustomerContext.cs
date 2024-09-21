namespace Schemio.Tests.EntitySetup
{
    internal class CustomerContext : EntityContext
    {
        public decimal CurrentVersion => 1;
        public int CustomerId { get; set; }
    }
}