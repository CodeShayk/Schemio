using Schemio.Core;
using Schemio.SQL.Tests.EntitySetup.Entities;
using Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Transforms
{
    public class CommunicationTransform : BaseTransformer<CommunicationRecord, Customer>
    {
        public override void Transform(CommunicationRecord queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.Communication = new Communication
            {
                ContactId = queryResult.ContactId,
                Email = queryResult.Email,
                Phone = queryResult.Telephone
            };

            if (queryResult.HouseNo != null)
                customer.Communication.Address = new Address
                {
                    AddressId = queryResult.AddressId,
                    HouseNo = queryResult.HouseNo,
                    City = queryResult.City,
                    Country = queryResult.Country,
                    PostalCode = queryResult.PostalCode,
                    Region = queryResult.Region
                };
        }
    }
}