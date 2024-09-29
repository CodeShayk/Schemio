using Schemio.EntityFramework.Tests.EntitySetup.Entities;
using Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Transforms
{
    public class CustomerCommunicationTransform : BaseTransformer<CommunicationResult, Customer>
    {
        public override void Transform(CommunicationResult queryResult, Customer entity)
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
                    Region = queryResult.Region,
                    AddressId = queryResult.AddressId
                };
        }
    }
}