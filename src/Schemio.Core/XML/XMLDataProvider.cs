using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Schemio.Core.Helpers.Xml;

namespace Schemio.Core.XML
{
    internal class XMLDataProvider<T> where T : IEntity
    {
        private readonly IEntityContextValidator entityContextValidator;
        private IDataProvider<T> dataProvider;
        private readonly ILogger<XMLDataProvider<T>> logger;

        private const string AuthorNamespace = "http://www.intelligent-office.net/author/1.0";

        public XMLDataProvider(IEntityContextValidator entityContextValidator,
            IDataProvider<T> dataProvider,
            ILogger<XMLDataProvider<T>> logger)
        {
            this.logger = logger;
            this.dataProvider = dataProvider;
            this.entityContextValidator = entityContextValidator;
        }

        /// <summary>
        /// Gets the xml document for the data source for given xpaths and context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>XDocument</returns>
        public virtual XDocument GetData(IEntityRequest context)
        {
            // Log Request context.
            LogRequest(context);
            // Validate Request context
            ValidateRequest(context);
            // Get entity object for given context
            var entity = dataProvider.GetData(context);
            // Transform entity object to xml document
            var doc = TransformToXmlDocument(entity);
            return doc;
        }

        /// <summary>
        /// Gets the xml  document by serializing the data source object.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        private XDocument TransformToXmlDocument(T dataSource)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("a", AuthorNamespace);
            var strXml = XmlHelper.SerializeToXml(dataSource, namespaces, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                CheckCharacters = false
            });

            logger.LogInformation(strXml);
            var xml = XmlSanitizer.Sanitize(strXml);
            var doc = XDocument.Parse(xml);

            return doc;
        }

        private void LogRequest(IEntityRequest context) => logger.LogInformation(context.GetType().Name);

        private void ValidateRequest(IEntityRequest context) => entityContextValidator.Validate(context);
    }
}