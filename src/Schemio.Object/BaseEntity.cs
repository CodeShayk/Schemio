using Schemio.Data.Core;

namespace Schemio.Object.Tests.Entities
{
    public class BaseEntity : IEntity
    {
        public decimal Version { get; set; } = 1;
    }
}