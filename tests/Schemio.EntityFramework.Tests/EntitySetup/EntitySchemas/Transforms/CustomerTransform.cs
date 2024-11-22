using Schemio.Core;
using Schemio.EntityFramework.Tests.EntitySetup.Entities;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Transforms
{
    public class CustomerTransform : BaseTransformer<CustomerRecord, Customer>
    {
        public override void Transform(CustomerRecord queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.Id = queryResult.Id;
            customer.Name = queryResult.Name;
            customer.Code = queryResult.Code;
        }
    }
}