# Schemio.Object 
[![NuGet version](https://badge.fury.io/nu/Schemio.Object.svg)](https://badge.fury.io/nu/Schemio.Object) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/NinjaRocks/Schemio.Object/blob/master/License.md) [![CI](https://github.com/NinjaRocks/Data2Xml/actions/workflows/CI.yml/badge.svg)](https://github.com/NinjaRocks/Data2Xml/actions/workflows/CI.yml) [![GitHub Release](https://img.shields.io/github/v/release/ninjarocks/Data2Xml?logo=github&sort=semver)](https://github.com/ninjarocks/Data2Xml/releases/latest)
[![CodeQL](https://github.com/NinjaRocks/Schemio.Object/actions/workflows/CodeQL.yml/badge.svg)](https://github.com/NinjaRocks/Schemio.Object/actions/workflows/CodeQL.yml) [![.Net Stardard](https://img.shields.io/badge/.Net%20Standard-2.1-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
--
Schemio - is a .Net 6 utility to conditionally hydrate entities with data by given list of schema paths mapping to object graph of that entity. Supports JsonPath and XPath schema mappings.

## How to use Schemio?
> Step 1 - To mark the class as entity to hydrate data using schemio, derive the class from `IEntity` interface. Bear in mind this is the root entity.

Below is the Customer entity we want to conditionally hydrate with data, by passing in schema paths mapping to object graph of customer class.

```
 public class Customer : IEntity
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public Communication Communication { get; set; }
        public Order[] Orders { get; set; }
    }
```

> Step 2 - Define entity schema configuration to map query/transformer pairs to schema paths mapping to the object graph of the entity in context. 

Derive schema from `IEntitySchema<T>` where T is entity to hydrate. The query/transformer mappings can be nested to 5 levels down. You could map multiple schema paths to a given query/transformer pair. 


```
internal class CustomerSchema : IEntitySchema<Customer>
    {
        private IEnumerable<Mapping<Customer, IQueryResult>> mappings;

        private decimal version;

        public IEnumerable<Mapping<Customer, IQueryResult>> Mappings => mappings;
        public decimal Version => version;

        public CustomerSchema()
        {
            version = 1;

            // Create an object mapping graph of query and transformer pairs using xpaths.
            mappings = CreateSchema.For<Customer>()
                .Map<CustomerQuery, CustomerTransform>(For.Paths("customer/id", "customer/customercode", "customer/customername"),
                 customer => customer.Dependents
                    .Map<CustomerCommunicationQuery, CustomerCommunicationTransform>(For.Paths("customer/communication"))
                    .Map<CustomerOrdersQuery, CustomerOrdersTransform>(For.Paths("customer/orders"),
                        customerOrders => customerOrders.Dependents
                            .Map<CustomerOrderItemsQuery, CustomerOrderItemsTransform>(For.Paths("customer/orders/order/items")))
                ).Complete();
        }
    }
```

#### Query Class
The purpose of a query class is to execute to fetch data when mapped schema path is included in the context paramer of data provider.
- To define a query for a schema path, you need to implement the query by deriving from  `BaseQuery<TQueryParameter, TQueryResult> : IQuery where TQueryParameter : IQueryParameter  where TQueryResult : IQueryResult` 
- You may want to run the query in parent or child mode and define the relevant overrides to resovle the query parameter accordingly.
- In `parent` mode the query parameter is resolved using `context` parameter passed to data provider class.
- In `child` mode, the query parameter is resolved using the `query result` of the `parent query` to which the current query is a child. You could have a maximum of `5` levels of children query nesting when defining the Entity schema.

> See example Customer query as parent below. 
```
public class CustomerQuery : BaseQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveParameterInParentMode(IDataContext context)
        {
            // Executes as root or level 1 query.
            var customer = (CustomerContext)context;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.CustomerId
            };
        }

        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Does not execute as child to any query.
        }
    }
```
> see communication query as child to customer query below.
```
    internal class CustomerCommunicationQuery : BaseQuery<CustomerParameter, CommunicationResult>
    {
        public override void ResolveParameterInParentMode(IDataContext context)
        {
            // Does not execute as root or level 1 queries.
        }

        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }
    }
```
The parent/child relationship is achieved via entity schema configuration. see CustomerSchema above.
#### Tranformer Class
The purpose of the transformer is to map the data fetched by the linked query class to relevant schema section of the entity to be data hydrated.
- To define a transformer class for the schema path, you need to derive from `BaseTransformer<TD, T> : ITransformer
        where T : IEntity
        where TD : IQueryResult` 
- The output of the linked query serves as input to the transformer to map data to configured section of the entity in context.

> Step 3 - Use the DataProvider class to get the entity with hydrated data based on configuration and passed in context paratemer with relevant schema paths.
```
var 

```

## Extending Schemio
> coming soon
