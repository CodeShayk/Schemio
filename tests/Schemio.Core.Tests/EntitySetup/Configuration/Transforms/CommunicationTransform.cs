using Schemio.Core.Tests.EntitySetup.Configuration.Queries;
using Schemio.Core.Tests.EntitySetup.Entities;

namespace Schemio.Core.Tests.EntitySetup.Configuration.Transforms
{
    public class CommunicationTransform : BaseTransformer<CommunicationRecord, Customer>
    {
        public override void Transform(CommunicationRecord queryResult, Customer entity)
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
        }
    }
}