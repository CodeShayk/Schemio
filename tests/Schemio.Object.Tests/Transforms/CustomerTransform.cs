using Schemio.Object.Tests.Entities;
using Schemio.Object.Tests.Queries;

namespace Schemio.Object.Tests.Transforms
{
    internal class CustomerTransform : BaseTransformer<CustomerResult, Customer>
    {
        public override Customer Transform(CustomerResult queryResult, Customer entity)
        {
            entity ??= new Customer();
            entity.Id = queryResult.Id;
            entity.CustomerCode = queryResult.CustomerCode;
            return entity;
        }
    }
}