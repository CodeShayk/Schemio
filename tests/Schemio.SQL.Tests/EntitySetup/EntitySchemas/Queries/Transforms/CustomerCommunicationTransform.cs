using Schemio.SQL.Tests.EntitySetup.Entities;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries.Transforms
{
    public class CustomerCommunicationTransform : BaseTransformer<CommunicationResult, Customer>
    {
        public override Customer Transform(CommunicationResult queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.Communication = new Communication
            {
                ContactId = queryResult.Id,
                Email = queryResult.Email,
                Phone = queryResult.Telephone
            };

            if (queryResult.HouseNo != null)
                customer.Communication.Address = new Address
                {
                    HouseNo = queryResult.HouseNo,
                    City = queryResult.City,
                    Country = queryResult.Country,
                    PostalCode = queryResult.PostalCode,
                    Region = queryResult.Region
                };

            return customer;
        }
    }
}