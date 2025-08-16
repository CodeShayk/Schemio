using System;

namespace Schemio.Core
{
    public class EntityConfigurationAttribute : Attribute
    {
        private Type Entity { get; }

        public EntityConfigurationAttribute(Type entity)
        {
            Entity = entity;
        }
    }

    public class QueryEngineAttribute : Attribute
    {
        public QueryEngineAttribute()
        {
        }
    }

    public class PathMatcherAttribute : Attribute
    {
        public PathMatcherAttribute()
        {
        }
    }
}