using Schemio.Core.Tests.EntitySetup.Entities;
using Schemio.Core.Tests.EntitySetup.Queries;

namespace Schemio.Core.Tests.EntitySetup.Transforms
{
    public class CustomerTransform : BaseTransformer<CustomerResult, Customer>
    {
        public override void Transform(CustomerResult queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.Id = queryResult.Id;
            customer.Name = queryResult.CustomerName;
            customer.Code = queryResult.CustomerCode;
        }
    }
}