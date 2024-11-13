using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CommunicationQuery : BaseSQLQuery<CommunicationResult>
    {
        protected override Func<DbContext, Task<CommunicationResult>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            var customer = (CustomerResult)parentQueryResult;

            return async dbContext => await dbContext.Set<Communication>()
                .Where(p => p.Customer.Id == customer.Id)
                .Select(c => new CommunicationResult
                {
                    Id = c.CommunicationId,
                    AddressId = c.Address.AddressId,
                    Telephone = c.Phone,
                    Email = c.Email,
                    HouseNo = c.Address.HouseNo,
                    City = c.Address.City,
                    Region = c.Address.Region,
                    PostalCode = c.Address.PostalCode,
                    Country = c.Address.Country
                })
                .FirstOrDefaultAsync();
        }
    }
}