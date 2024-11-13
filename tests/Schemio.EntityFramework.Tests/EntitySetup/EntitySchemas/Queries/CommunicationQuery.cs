using Microsoft.EntityFrameworkCore;
using Schemio.Core;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CommunicationQuery : BaseSQLQuery<CustomerParameter, CommunicationResult>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }

        public override async Task<CommunicationResult> Run(DbContext dbContext)
        {
            var result = await dbContext.Set<Communication>()
                .Where(p => p.Customer.Id == QueryParameter.CustomerId)
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

            return result;
        }
    }
}