using Schemio.Object.Core;

namespace Schemio.Object.Tests.Entities
{
    public class OrderItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}