namespace Schemio.Object.Tests.EntitySetup
{
    internal class CustomerContext : IDataContext
    {
        public string[] Paths { get; set; }
        public decimal CurrentVersion => 1;
        public int CustomerId { get; set; }
    }
}