namespace Schemio.Object.Tests.Entities
{
    internal class Person : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Ethnicity Ethnicity { get; set; }
    }
}