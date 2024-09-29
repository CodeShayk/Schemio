using Microsoft.EntityFrameworkCore;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerQuery : BaseSQLRootQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveRootQueryParameter(IDataContext context)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context.Entity;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.CustomerId
            };
        }

        public override IEnumerable<IQueryResult> Run(DbContext dbContext)
        {
            return dbContext.Set<Customer>()
                        .Where(c => c.Id == QueryParameter.CustomerId)
                        .Select(c => new CustomerResult
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Code = c.Code
                        });
        }
    }
}