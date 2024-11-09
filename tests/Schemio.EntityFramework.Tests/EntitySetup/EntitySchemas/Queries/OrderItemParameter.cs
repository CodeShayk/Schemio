using Schemio.Core;
using Schemio.Core.Helpers;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class OrderItemParameter : IQueryParameter
    {
        public OrderItemParameter()
        {
            OrderIds = new List<int>();
        }

        public string ToCsv()
        {
            return OrderIds.ToCSV();
        }

        public List<int> OrderIds { get; set; }
    }
}