using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Transforms
{
    public class CustomerTransform : BaseTransformer<CustomerResult, Customer>
    {
        public override void Transform(CustomerResult queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.Id = queryResult.Id;
            customer.Name = queryResult.Name;
            customer.Code = queryResult.Code;
        }
    }
}