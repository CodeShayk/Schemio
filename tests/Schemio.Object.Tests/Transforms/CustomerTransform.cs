using Schemio.Object.Core;
using Schemio.Object.Tests.Entities;
using Schemio.Object.Tests.Queries;

namespace Schemio.Object.Tests.Transforms
{
    public class CustomerTransform : BaseTransformer<CustomerResult, Customer>
    {
        public override Customer Transform(CustomerResult queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.CustomerId = queryResult.Id;
            customer.CustomerName = queryResult.CustomerName;
            customer.CustomerCode = queryResult.CustomerCode;
            return customer;
        }
    }
}