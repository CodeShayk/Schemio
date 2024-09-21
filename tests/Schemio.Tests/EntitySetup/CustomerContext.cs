namespace Schemio.Tests.EntitySetup
{
    internal class CustomerContext : SchemioContext
    {
        public decimal CurrentVersion => 1;
        public int CustomerId { get; set; }
    }
}