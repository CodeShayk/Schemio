using System;

namespace Schemio.Core
{
    public class MapParentQueryAttribute : Attribute
    {
        private Type TTransformer { get; }
        private SchemaPaths SchemaPaths { get; }

        public MapParentQueryAttribute(Type tTransformer, params string[] schemaPaths)
        {
            TTransformer = tTransformer;
            SchemaPaths = new SchemaPaths { Paths = schemaPaths };
        }
    }
}