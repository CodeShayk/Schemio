using Schemio.Object.Tests.EntitySetup.Entities;
using Schemio.Object.Tests.EntitySetup.Queries;

namespace Schemio.Object.Tests.EntitySetup.Transforms
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