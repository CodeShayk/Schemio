using Schemio.API.Tests.EntitySetup.QueryResults;
using Schemio.Core;

namespace Schemio.API.Tests.EntitySetup.WebApis
{
    internal class CommunicationWebQuery : WebQuery<CommunicationResult>
    {
        public CommunicationWebQuery() : base(Endpoints.BaseAddress)
        {
        }

        protected override Func<Uri> GetQuery(IDataContext context, IQueryResult parentApiResult)
        {
            var customer = (CustomerResult)parentApiResult;
            return () => new Uri(string.Format(Endpoints.BaseAddress + Endpoints.Communication, customer.Id), UriKind.Absolute);
        }
    }
}