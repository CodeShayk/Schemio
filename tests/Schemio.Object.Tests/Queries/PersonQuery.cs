using Schemio.Data.Core;

namespace Schemio.Object.Tests.Queries
{
    internal class PersonQuery : BaseQuery<PersonParameter, PersonResult>
    {
        public override void ResolveRootQueryParameter(IDataContext context)
        {
            // Does not execute as root or level 1 queries.
        }

        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new PersonParameter
            {
                PersonId = customer.PersonId
            };
        }
    }

    internal class PersonResult : IQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EthnicityId { get; set; }
    }

    internal class PersonParameter : IQueryParameter
    {
        public int PersonId { get; set; }
    }
}