using Schemio.Data.Core;

namespace Schemio.Object.Tests.Queries
{
    internal class EthnicityQuery : BaseQuery<PersonParameter, EthnicityResult>
    {
        public override void ResolveRootQueryParameter(IDataContext context)
        {
            // Does not execute as root or level 1 queries.
        }

        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to person query.
            var person = (PersonResult)parentQueryResult;
            QueryParameter = new PersonParameter
            {
                PersonId = person.Id
            };
        }
    }

    internal class EthnicityResult : IQueryResult
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}