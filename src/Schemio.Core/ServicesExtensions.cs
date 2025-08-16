using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Schemio.Core.Impl;
using Schemio.Core.PathMatchers;

namespace Schemio.Core
{
    public static class ServicesExtensions
    {
        public static void UseSchemio(this IServiceCollection services, Action<ISchemioOptions> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration), "Configuration action cannot be null.");

            if (services == null)
                throw new ArgumentNullException(nameof(services), "Service collection cannot be null.");

            var options = new SchemioOptionsBuilder(services);
            configuration?.Invoke(options);

            if (!services.Any(s => s.ServiceType == typeof(ISchemaPathMatcher)))
                services.AddTransient<ISchemaPathMatcher, XPathMatcher>();

            if (!options.Silent && options.EntityConfigurationCount == 0)
                throw new InvalidOperationException("At least one entity configuration must be registered using WithEntityConfiguration<TEntity> method.");

            if (!options.Silent && options.EngineCount == 0)
                throw new InvalidOperationException("At least one query engine must be registered using WithEngine or WithEngines method.");

            services.AddTransient<IQueryExecutor, QueryExecutor>();
        }
    }
}