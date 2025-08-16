using System;

namespace Schemio.Core
{
    public class MapChildQueryAttribute : Attribute
    {
        private Type TParentQuery { get; }
        private Type TTransformer { get; }
        private ISchemaPaths SchemaPaths { get; }

        public MapChildQueryAttribute(Type tParentQuery, Type tTransformer, params string[] schemaPaths)
        {
            TParentQuery = tParentQuery;
            TTransformer = tTransformer;
            SchemaPaths = new SchemaPaths { Paths = schemaPaths };
            ;
        }
    }
}