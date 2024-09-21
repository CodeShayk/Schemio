namespace Schemio
{
    internal class EntityContext : IEntityContext
    {
        public object EntityId { get; set; }
        public string[] Paths { get; set; }
    }
}