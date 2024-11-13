using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    [CacheResult]
    public class CommunicationResult : IQueryResult
    {
        public int ContactId { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public string HouseNo { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}