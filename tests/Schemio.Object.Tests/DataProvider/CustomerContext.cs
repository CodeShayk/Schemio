using Schemio.Object.Core;

namespace Schemio.Object.Tests.DataProvider
{
    internal class CustomerContext : IDataContext
    {
        public string[] Paths { get; set; }
        public decimal CurrentVersion => 1;
        public int CustomerId { get; set; }
    }
}