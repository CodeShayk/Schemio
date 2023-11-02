# Schemio.Object 
[![NuGet version](https://badge.fury.io/nu/Schemio.Object.svg)](https://badge.fury.io/nu/Schemio.Object) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/NinjaRocks/Schemio.Object/blob/master/License.md) [![CI](https://github.com/NinjaRocks/Data2Xml/actions/workflows/CI.yml/badge.svg)](https://github.com/NinjaRocks/Data2Xml/actions/workflows/CI.yml) [![GitHub Release](https://img.shields.io/github/v/release/ninjarocks/Data2Xml?logo=github&sort=semver)](https://github.com/ninjarocks/Data2Xml/releases/latest)
[![CodeQL](https://github.com/NinjaRocks/Schemio.Object/actions/workflows/CodeQL.yml/badge.svg)](https://github.com/NinjaRocks/Schemio.Object/actions/workflows/CodeQL.yml) [![.Net Stardard](https://img.shields.io/badge/.Net%20Standard-2.1-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
--
## What is Schemio?
`Schemio` is a .Net utility to data hydrate an entity by given schema paths of the object graph.
> Supports XPath & JsonPath schema paths.

## When to use Schemio?
Schemio is a perfect utility when you need to fetch a large entity from data source. Ideally, you may not require all of the entity data but only sections of the object graph by varied fetches.
> Example  use case is document generation which may require only templated sections of client data to be fetched for a document template in context.

## How to use Schemio?
you could use Schemio out of the box or extend the utility to suit your custom needs.
> To use schemio you need to
> - Setup the entity to be fetched using DataProvider. 
> - Construct the DataProvider with required dependencies. 

### Entity Setup
* Define the `entity` to be fetched using `DataProvider` - which is basically a class with nested typed properties.
* Define the `entity schema` with `query` and `transformer` pairs mappings to entity's object graph. The relevant query and transformer pairs will execute in the order of their nesting when their mapped `schema paths` are included in the `request` parameter of the DataProvider. 
* `Query` is an implementation to fetch `data` for the entity object graph from the underlying data storage supported by the chosen `QueryEngine`.  QueryEngine is an implementation of `IQueryEngine` to execute queries against implemented data source.
* `Transformer` is an implementation to transform the data fetched by the associated query to mapped section of the object graph.
#### Entity
> Step 1 - To mark the class as Entity using schemio, implement the class from `IEntity` interface. Bear in mind this is the root entity to be fetched.

Below is an example `Customer` entity we want to fetch using schemio.

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
There are three levels of nesting in the object graph for customer class above.
- Level 1 with paths: `Customer/CustomerId`, `Customer/CustomerCode`, `Customer/CustomerName`
- Level 2 with paths: `Customer/Communication` and `Customer/Orders`
- Level 3 with paths: `Customer/Orders/Order/Items`

#### Entity Schema
> Step 2 - Define entity schema configuration which is basically a hierarchy of query/transformer pairs mapping to the object graph of the entity in context. 

To define Entity schema, implement `IEntitySchema<T>` interface where T is entity in context. The `query/transformer` mappings can be `nested` to `5 levels` down. You could map multiple schema paths to a given query/transformer pair. 

The above object graph with three levels is configured below with query and transformer pairs nested accordingly, mapping to object graph of customer as defined by the XPath schema.

```
internal class CustomerSchema : IEntitySchema<Customer>
    {
        private IEnumerable<Mapping<Customer, IQueryResult>> mappings;
       
        public IEnumerable<Mapping<Customer, IQueryResult>> Mappings => mappings;
      
        public CustomerSchema()
        {
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

### DataProvider Setup
> coming soon
