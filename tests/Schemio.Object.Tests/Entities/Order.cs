namespace Schemio.Object.Tests.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
        public OrderItem[] Items { get; set; }
    }
}