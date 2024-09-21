# <img src="https://github.com/CodeShayk/schemio/blob/master/images/ninja-icon-16.png" alt="ninja" style="width:30px;"/>  Schemio v 1.0 
[![NuGet version](https://badge.fury.io/nu/Schemio.Object.svg)](https://badge.fury.io/nu/Schemio.Object) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/NinjaRocks/Schemio.Object/blob/master/License.md) [![CI](https://github.com/CodeShayk/Schemio/actions/workflows/CI.yml/badge.svg)](https://github.com/CodeShayk/Schemio/actions/workflows/CI.yml) [![GitHub Release](https://img.shields.io/github/v/release/CodeShayk/Schemio?logo=github&sort=semver)](https://github.com/CodeShayk/Schemio/releases/latest)
[![CodeQL](https://github.com/CodeShayk/Schemio/actions/workflows/CodeQL.yml/badge.svg)](https://github.com/CodeShayk/Schemio/actions/workflows/CodeQL.yml) [![.Net 8.0](https://img.shields.io/badge/.Net-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
--
## What is Schemio?
`Schemio` is a .Net utility that can be used to fetch entities by specifying sections of object graph for hydrating data.
> Supports XPath & JsonPath for object schema paths.

## When to use Schemio?
Schemio is a perfect utility when you need to fetch a large entity from data source. Ideally, you may not require all of the entity data but only sections of the object graph for different fetches.
> Example  use case is document generation which may require only templated sections of client data to be fetched for different document templates in context.

## How to use Schemio?
You could use Schemio out of the box or extend the utility in order to suit your custom needs.
> To use schemio you need to
> - Setup the entity to be fetched using DataProvider. 
> - Construct the DataProvider with required dependencies. 

### Entity Setup
* Define the `entity` to be fetched using `DataProvider` - which is basically a class with nested typed properties.
* Define the `entity schema` with `query` and `transformer` pairs mappings to entity's object graph. The relevant query and transformer pairs will execute in the order of their nesting when their mapped `schema paths` are included in the `request` parameter of the DataProvider. 
* `Query` is an implementation to fetch `data` for the entity object graph from the underlying data storage supported by the chosen `QueryEngine`.  QueryEngine is an implementation of `IQueryEngine` to execute queries against supported data source.
* `Transformer` is an implementation to transform the data fetched by the associated query to mapped section of the object graph.
#### 1. Entity
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

#### 2. Entity Schema
> Step 2 - Define entity schema configuration which is basically a hierarchy of query/transformer pairs mapping to the object graph of the entity in context. 

To define Entity schema, implement `IEntitySchema<T>` interface where T is entity in context. The `query/transformer` mappings can be `nested` to `5` levels down.

You could map multiple schema paths to a given query/transformer pair. Currently, XPath and JSONPath schema paths are supported.

If you need to support custom schema language for mapping to object graph, then use the custom paths in entity schema definition however you may need to provide custom implementation of `ISchemaPathMatcher` interface.
```
public interface ISchemaPathMatcher
    {
        bool IsMatch(string inputPath, ISchemaPaths configuredPaths);
    }
```

Example Entity Schema Definition
> The `Customer` entity with `three` levels of `nesting` is configured below in `CustomerSchema` definition to show `query/transformer` pairs nested accordingly mapping to object graph using the XPath definitions.

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

#### 2.1 Query Class
The purpose of a query class is to execute to fetch data from data source when mapped schema path(s) are included in the request parameter of data provider.
- To define a query you need to implement from `BaseQuery<TQueryParameter, TQueryResult>` where `TQueryParameter` is the query parameter and `TQueryResult` is the query result. 
- `TQueryParameter` is basically the class that holds the `inputs` required by the query for execution. 
- `TQueryResult` is the result that will be obtained from executing the query. 
- You can run the query in `Parent` or `Child` (nested) mode. The parent/child relationship is achieved by configuring the query accordingly in entity schema definition. See `CustomerSchema` above. 
- The query parameter needs to be resolved before executing the query with QueryEngine.
  - In `parent` mode, the query parameter is resolved using the `request/context` parameter passed to data provider class.
  - In `child` mode, the query parameter is resolved using the `query result` of the `parent query` as stiched in the entity schema configuration. You could have a maximum of `5` levels of children query nestings.

See example `CustomerQuery` implemented to run in parent mode below. 
```
public class CustomerQuery : BaseQuery<CustomerParameter, CustomerResult>
    {
        public override void ResolveParameterInParentMode(IDataContext context)
        {
            // Executes as Parent or Level 1 query.
            // The query parameter is resolved using context parameter of data provider class.

            var customer = (CustomerContext)context;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.CustomerId
            };
        }

        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Does not execute as child to any query. Hence has no implementation provided.
        }
    }
```
See example `CustomerCommunicationQuery` implemented to run as child or nested query to customer query below. Please see `CustomerSchema` definition above for parent/child configuration setup.
```
    internal class CustomerCommunicationQuery : BaseQuery<CustomerParameter, CommunicationResult>
    {
        public override void ResolveParameterInParentMode(IDataContext context)
        {
            // Does not execute as Parent or Level 1 query. Hence has no implementation provided.
        }

        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            // The result from parent customer query is used to resolve the query parameter of the nested communication query.

            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }
    }
```
Please Note: The above query implementation is basic and could vary with different implementations of the QueryEngine.
> Please see Query engine provider specific implementation of queries below.


#### 2.2 Tranformer Class
The purpose of the transformer class is to transform the data fetched by the linked query class to mapped object graph of the entity.

To define a transformer class, you need to implement `BaseTransformer<TD, T>`
- where T is Entity implementing `IEntity`. eg. Customer. 
- where TD is Query Result from associated Query implementing `IQueryResult` in EntitySchema definition. This is the query result obtained from the query, the transformer will consume to map to the relevant object graph of the Entity. 

> Please Note: Every `Query` type in the `EntitySchema` definition should have a complementing `Transformer` type.

Below is the snippet from `CustomerSchema` definition.
> .Map<CustomerQuery, CustomerTransform>(For.Paths("customer/id", "customer/customercode", "customer/customername"))

The transformer should `map` `data` to only the `schema` mapped `sections` of the `object graph`.
           

In below example, `CustomerTransformer` (transformer) is implemented to transform `Customer` (entity) with `CustomerResult` (query result) obtained from `CustomerQuery` (query) execution.

The transformer maps data to only `XPath` mapped sections of Customer object grapgh - `customer/id`, `customer/customercode`, `customer/customername` 

```
public class CustomerTransform : BaseTransformer<CustomerResult, Customer>
    {
        public override Customer Transform(CustomerResult queryResult, Customer entity)
        {
            var customer = entity ?? new Customer();
            customer.CustomerId = queryResult.Id;
            customer.CustomerName = queryResult.CustomerName;
            customer.CustomerCode = queryResult.CustomerCode;
            return customer;
        }
    }
```

### DataProvider Setup
> coming soon
