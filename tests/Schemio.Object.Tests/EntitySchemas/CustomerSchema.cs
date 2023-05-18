using Schemio.Object.Core;
using Schemio.Object.Tests.Entities;

namespace Schemio.Object.Tests.EntitySchemas
{
    internal class CustomerSchema : IEntitySchema<Customer>
    {
        public IEnumerable<Mapping<Customer, IQueryResult>> Mappings { get; }
        public decimal Version => 1;

        public CustomerSchema()
        {
            //Mappings = CreateSchema.For<Customer>()
            //    .Map<UserContextQuery, ClientCommonUserContextTransformer>(For.Paths("clients/common/context"), dependantQuery =>
            //        dependantQuery.Dependents
            //            .Map<ContextOrganisationLogoQuery, ClientOrganisationLogoTransformer>(For.XPath("clients/common/context/currentuserdetails/groupdetails/organisationlogo"))
            //            .Map<ContextGroupLogoQuery, ClientGroupLogoTransformer>(For.XPath("clients/common/context/currentuserdetails/groupdetails/grouplogo"))
            //            .Map<CommonContextAvatarQuery, ClientPhotoTransformer>(For.XPath("clients/common/context/currentuserdetails/photo"))
            //        );
        }
    }
}