using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    public class CustomerQuery : SQLQuery<CustomerRecord>
    {
        protected override Func<DbContext, Task<CustomerRecord>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Executes as root or level 1 query. parentQueryResult will be null.
            var customer = (CustomerContext)context.Entity;

            return async dbContext =>
            {
                var result = await dbContext.Set<Customer>()
                        .Where(c => c.Id == customer.CustomerId)
                        .Select(c => new CustomerRecord
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Code = c.Code
                        })
                        .FirstOrDefaultAsync();

                return result;
            };
        }
    }
}