# <img src="https://github.com/CodeShayk/Schemio/blob/master/Images/ninja-icon-16.png" alt="ninja" style="width:30px;"/>  Schemio v 1.0 
[![NuGet version](https://badge.fury.io/nu/Schemio.svg)](https://badge.fury.io/nu/Schemio) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/CodeShayk/Schemio/blob/master/LICENSE.md) 
[![Master-Build](https://github.com/CodeShayk/Schemio/actions/workflows/Build-Master.yml/badge.svg)](https://github.com/CodeShayk/Schemio/actions/workflows/Build-Master.yml) 
[![GitHub Release](https://img.shields.io/github/v/release/CodeShayk/Schemio?logo=github&sort=semver)](https://github.com/CodeShayk/Schemio/releases/latest)
[![Master-CodeQL](https://github.com/CodeShayk/Schemio/actions/workflows/Master-CodeQL.yml/badge.svg)](https://github.com/CodeShayk/Schemio/actions/workflows/Master-CodeQL.yml) 
[![.Net 8.0](https://img.shields.io/badge/.Net-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
--
> #### Nuget Packages - Query Engines
>i. `Schemio.SQL` - Provides schemio with query engine using `Dapper` to execute SQL queries.
>
>ii. `Schemio.EntityFramework` - Provides schemio with `Entity Framework` query engine to execute queries using DbContext.

## What is Schemio?
`Schemio` is a .net utility to hydrate an entity with data by specifying schema paths or sections of its object graph.
> Supports XPath & JsonPath for schema paths.

## When to use Schemio?
Schemio is perfect fit when you need to fetch parts of a large entity from given data storage. Ideally, you may not want all of the entity data but preferably only sections of the object graph depending on the context for fetch.

Few example schemio use cases that require the service tier to dynamically fetch data for high performance, availability and scalability are
> - Reporting
> - Document Generation ( with templated data)
> - Content Management Systems
> - Many more

## How to use Schemio?
To use schemio you need to
> Step 1 - Setup the entity to be fetched.
> 
> Step 2 - Construct the `DataProvider` with required dependencies. 

### Step 1. Entity Setup
Setting up an entity includes the following. 
* Define the `entity` to be fetched using `DataProvider` - which is basically a class with entire object graph (with nested typed properties).
* Define the `entity schema` which is schema path mapping of the entire entity object graph. Each mapping consists of a `query` and `transformer` pair mapped to a sections of object graph (using XPaths or JsonPath for schema paths)

#### 1.1 Entity
To mark the class as Entity, implement the class from `IEntity` interface.
Bear in mind this is the root entity to be fetched.

>Below is an example `Customer` entity.
>
> ```
> public class Customer : IEntity
>    {
>        public int CustomerId { get; set; }
>        public string CustomerCode { get; set; }
>        public string CustomerName { get; set; }
>        public Communication Communication { get; set; }
>        public Order[] Orders { get; set; }
>    }
> ```

For the customer class, we can see there are three levels of nesting in the object graph.
- Level 1 with paths: `Customer`
- Level 2 with paths: `Customer.Communication` and `Customer.Orders`
- Level 3 with paths: `Customer.Orders.Items`

If we choose XML Schema Definition (XSD) for the object schema of the above Customer class fo mapping with XPATHs
then below is the Customer XSD and XPaths for different nesting levels.

> Customer XSD is 
> ```
> Coming soon...
> ```

> Schema mappings using XPaths are
> ```
> - Level 1 with XPath: Customer
> - Level 2 with XPaths: Customer/Communication and Customer/Orders
> - Level 3 with XPath: Customer/Orders/Order/Items/Item

#### 1.2 Entity Schema Definition
Define entity schema definition for the entity in context.

* `Entity schema definition` is basically a configuration with hierarchy of `query/transformer` pairs mapped to the schema paths pointing to different levels of the entity's object graph.
* `Query` is an implementation to fetch data for a certain section of object graph from an underlying data storage. 
* `Transformer` is an implementation to map the data fetched by the linked query to the relevant sections of the entity's object graph.

To define Entity schema, implement `BaseEntitySchema<T>` interface where T is entity in context.

>
Example Entity Schema Definition (using XPaths)
> The `Customer` entity with `three` levels of `nesting` is configured below in `CustomerSchema` definition to show `query/transformer` pairs nested accordingly mapping to object graph using the XPath definitions.
>
> ```
> internal class CustomerSchema : BaseEntitySchema<Customer>
>    {
>       public override IEnumerable<Mapping<Customer, IQueryResult>> GetSchema()
>       {
>           return CreateSchema.For<Customer>()
>               .Map<CustomerQuery, CustomerTransform>(For.Paths("customer"),
>                customer => customer.Dependents
>                   .Map<CustomerCommunicationQuery, CustomerCommunicationTransform>(For.Paths("customer/communication"))
>                   .Map<CustomerOrdersQuery, CustomerOrdersTransform>(For.Paths("customer/orders"),
>                       customerOrders => customerOrders.Dependents
>                           .Map<CustomerOrderItemsQuery, CustomerOrderItemsTransform>(For.Paths("customer/orders/order/items")))
>               ).Create();
>       }
>   }
>```

##### i. Query/Transformer Mapping
Every `Query` type in the `EntitySchema` definition should have a complementing `Transformer` type.
You could map multiple `schema paths` to a given query/transformer pair. Currently, `XPath` and `JSONPath` schema languages are supported.

>Below is the snippet from `CustomerSchema` definition.
>```
>   .Map<CustomerQuery, CustomerTransform>(For.Paths("customer", "customer/code", "customer/name"))
>```

##### ii. Nested Query/Transformer Mappings
* You could nest query/transformer pairs in a `parent/child` hierarchy. In which case the output of the parent query will serve as the input to the child query to resolve its query paramter.
* The query/transformer mappings can be `nested` to `5` levels down.
* When certain `schema paths` are included in the DataProvider `request` to fetch the Entity, the relevant query and transformer pairs get executed in the order of their nesting to hydrate the entity. 

>Example nesting of Communication query under Customer query. 
>```
>     .Map<CustomerQuery, CustomerTransform>(For.Paths("customer"), -- Parent
>           customer => customer.Dependents
>               .Map<CustomerCommunicationQuery, CustomerCommunicationTransform>(For.Paths("customer/communication")) -- Child
>```


Please see the execution sequence below for queries and transformers nested in CustomerSchema implemented above.

<img width="1202" alt="image" src="https://github.com/CodeShayk/Schemio/blob/master/Images/EntitySchemaDefinition.png">


`Please Note:` If you need to support custom schema language for mapping the object graph, then see extending schemio section below.


#### 1.2.1 Query Class
The purpose of a query class is to execute with supported QueryEngine to fetch data from data storage.

QueryEngine is an implementation of `IQueryEngine` to execute queries against a supported data storage to return a collection of query results (ie. of type IQueryResult).

As explained above, You can configure a query in `Parent` or `Child` (nested) mode in nested hierarchies.

i. Parent Query

To define a `parent` or `root` query which is usually configured at level 1 to query the root entity, derive from `aseRootQuery<TQueryParameter, TQueryResult>`
* `TQueryParameter` is basically the class that holds the `inputs` required by the root query for execution. It is an implementation of `IQueryParameter` type.
* `TQueryResult` is the result that will be returned from executing the root query.  It is an implementation of `IQueryResult` type.

The query parameter needs to be resolved before executing the query with QueryEngine.

In `parent` mode, the query parameter is resolved using the `IDataContext` parameter passed to data provider class.


> See example `CustomerQuery` implemented to be configured and run in parent mode below. 
> ```
>internal class CustomerQuery : BaseRootQuery<CustomerParameter, CustomerResult>
>    {
>        public override void ResolveRootQueryParameter(IDataContext context)
>        {
>            // Executes as Parent or Level 1 query.
>            // The query parameter is resolved using IDataContext parameter of data provider class.
>
>            var customer = (CustomerContext)context;
>            QueryParameter = new CustomerParameter
>            {
>                CustomerId = customer.CustomerId
>            };
>        }
>    }
>```

ii. Child Query

To define a `child` or `dependant` query which is usually configured as child at level below the root query to query, derive from `BaseChildQuery<TQueryParameter, TQueryResult>`
* `TQueryParameter` is basically the class that holds the `inputs` required by the child query for execution. It is an implementation of `IQueryParameter` type.
* `TQueryResult` is the result that will be returned by executing the child query.  It is an implementation of `IQueryResult` type.

Similar to Root query, the query parameter of child query needs to be resolved before executing with QueryEngine.

In `child` mode, the query parameter is resolved using the `query result` of the `parent` query. You can have a maximum of `5` levels of query nestings.

> See example `CustomerCommunicationQuery` implemented to be configured and run as child or nested query to customer query below. Please see `CustomerSchema` definition above for parent/child configuration setup.
>```
>    internal class CustomerCommunicationQuery : BaseChildQuery<CustomerParameter, CommunicationResult>
>    {
>        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
>        {
>            // Execute as child to customer query.
>            // The result from parent customer query is used to resolve the query parameter of the nested communication query.
>
>            var customer = (CustomerResult)parentQueryResult;
>            QueryParameter = new CustomerParameter
>            {
>                CustomerId = customer.Id
>            };
>        }
>    }
>```

#### Query Engines

`Please Note:` The above query implementation examples are with respect to parent/child configuration. The actual storage specific query definition should vary with specific implementation of the QueryEngine.
> Please see supported Query engine implementations below.
- `Schemio.SQL` - provides the implementation of IQueryEngine to execute SQL queries. Uses `Dapper` for SQL data acess.
- `Schemio.EntityFramework` - provides implementation of IQueryEngine to execute `Entity Framework` queries.

`Query using Schemio.SQL`
The SQL query needs to implement `BaseSQLRootQuery<TQueryParameter, TQueryResult>` or `BaseSQLChildQuery<TQueryParameter, TQueryResult>` based on parent or child implementation.
And, requires implementing  `public abstract CommandDefinition GetCommandDefinition()` method to return `command definition` for query to be executed with `Dapper` supported QueryEngine.

See below example `CustomerQuery` implemented as Root SQL query
>```
> internal class CustomerQuery : BaseSQLRootQuery<CustomerParameter, CustomerResult>
>    {
>        public override void ResolveRootQueryParameter(IDataContext context)
>        {
>            // Executes as root or level 1 query.
>            var customer = (CustomerContext)context.Entity;
>            QueryParameter = new CustomerParameter
>            {
>                CustomerId = (int)customer.CustomerId
>            };
>        }
>
>       public override IEnumerable<CustomerResult> Execute(IDbConnection conn)
>        {
>            return conn.Query<CustomerResult>(new CommandDefinition
>            (
>                "select CustomerId as Id, " +
>                       "Customer_Name as Name," +
>                       "Customer_Code as Code " +
>                $"from TCustomer where customerId={QueryParameter.CustomerId}"
>           ));
>        }
>    }
>```
>
See below example `CustomerOrderItemsQuery` implemented as child SQL query.
>```
>internal class CustomerOrderItemsQuery : BaseSQLChildQuery<OrderItemParameter, OrderItemResult>
>    {
>        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
>        {
>            // Execute as child query to order query taking OrderResult to resolve query parameter.
>            var ordersResult = (OrderResult)parentQueryResult;
>
>            QueryParameter ??= new OrderItemParameter();
>            QueryParameter.OrderIds.Add(ordersResult.OrderId);
>        }
>
>        public override IEnumerable<OrderItemResult> Execute(IDbConnection conn)
>        {
>            return conn.Query<OrderItemResult>(new CommandDefinition
>            (
>                "select OrderId, " +
>                       "OrderItemId as ItemId, " +
>                       "Name, " +
>                       "Cost " +
>                $"from TOrderItem where OrderId in ({QueryParameter.ToCsv()})"
>           ));
>        }
>    }
>```

`Query using Schemio.EntityFramework`
The SQL query needs to implement `BaseSQLRootQuery<TQueryParameter, TQueryResult>` or `BaseSQLChildQuery<TQueryParameter, TQueryResult>` based on parent or child implementation.
And, requires implementing  `public abstract IEnumerable<IQueryResult> Run(DbContext dbContext)` method to implement query using `DbContext` using entity framework.

See below example `CustomerQuery` implemented as Root Entity framework query
>```
> internal class CustomerQuery : BaseSQLRootQuery<CustomerParameter, CustomerResult>
>    {
>        public override void ResolveRootQueryParameter(IDataContext context)
>        {
>            // Executes as root or level 1 query.
>            var customer = (CustomerContext)context.Entity;
>            QueryParameter = new CustomerParameter
>            {
>                CustomerId = (int)customer.CustomerId
>            };
>        }
>
>        public override IEnumerable<IQueryResult> Run(DbContext dbContext)
>        {
>            return dbContext.Set<Customer>()
>                        .Where(c => c.Id == QueryParameter.CustomerId)
>                        .Select(c => new CustomerResult
>                        {
>                            Id = c.Id,
>                            Name = c.Name,
>                            Code = c.Code
>                        });
>        }
>    }
>```
>
See below example `CustomerOrderItemsQuery` implemented as child Entity framework query.
>```
>internal class CustomerOrderItemsQuery : BaseSQLChildQuery<OrderItemParameter, OrderItemResult>
>    {
>        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
>        {
>            // Execute as child query to order query taking OrderResult to resolve query parameter.
>            var ordersResult = (CustomerOrderResult)parentQueryResult;
>
>            QueryParameter ??= new OrderItemParameter();
>            QueryParameter.OrderIds.Add(ordersResult.OrderId);
>        }
>
>        public override IEnumerable<IQueryResult> Run(DbContext dbContext)
>        {
>            return dbContext.Set<OrderItem>()
>                .Where(p => QueryParameter.OrderIds.Contains(p.Order.OrderId))
>                .Select(c => new OrderItemResult
>                {
>                    ItemId = c.ItemId,
>                    Name = c.Name,
>                    Cost = c.Cost,
>                    OrderId = c.Order.OrderId
>                });
>            ;
>        }
>    }
>```


#### 2.2 Tranformer Class
The purpose of the transformer class is to transform the data fetched by the linked query class and mapp to the configured object graph of the entity.

To define a transformer class, you need to implement `BaseTransformer<TQueryResult, TEntity>`
- where TEntity is Entity implementing `IEntity`. eg. Customer. 
- where TQueryResult is Query Result from associated Query. It is an implementation of `IQueryResult` interface. 

Note: It is `important` that the transformer should map data only to the `schema path(s)` pointing `section(s)` of the object graph.

For the example query/transformer mapping
>```
>   .Map<CustomerQuery, CustomerTransform>(For.Paths("customer"))
>```

The customer transformer maps data only to the `customer` xpath mapped object graph of customer class.
ie. - `customer/id`, `customer/customercode`, `customer/customername`

In below transformer example, `CustomerTransformer` is implemented to transform entity `Customer` with `CustomerResult` query result obtained from `CustomerQuery` execution.

>
>```
>internal class CustomerTransform : BaseTransformer<CustomerResult, Customer>
>    {
>        public override Customer Transform(CustomerResult queryResult, Customer entity)
>        {
>            var customer = entity ?? new Customer();
>            customer.CustomerId = queryResult.Id;
>            customer.CustomerName = queryResult.CustomerName;
>            customer.CustomerCode = queryResult.CustomerCode;
>            return customer;
>        }
>    }
>```

### DataProvider Setup
Data provider needs to setup with required dependencies. Provide implementations of below dependencies to construct the data provider.

- `ILogger<DataProvider<TEntity>>` - logger implementation. default no logger.
- `IEntitySchema<TEntity>` - mandatory entity schema definition for entity's object graph. 
- `IQueryEngine` - implementation of query engine to execute queries (of type IQuery) with supported data storage.
- `ISchemaPathMatcher` - implementation of schema path matcher to use custom schema paths with entity schema definition.

Example constructors:

i. With `EntitySchema` and `QueryEngine` implementations.

```
    public DataProvider(IEntitySchema<TEntity> entitySchema, params IQueryEngine[] queryEngines)
```      
ii. With `Logger`, `EntitySchema`, `QueryEngine`, and `SchemaPathmMatcher` for custom schema paths mapping in entity schema definition.
```
    public DataProvider(ILogger<DataProvider<TEntity>> logger, IEntitySchema<TEntity> entitySchema, ISchemaPathMatcher schemaPathMatcher, params IQueryEngine[] queryEngines)
           
```
#### Schemio.SQL
Construct DataProvider using `Schemio.SQL.QueryEngine` query engine.

```
var provider = new DataProvider(new CustomerSchema(), new Schemio.SQL.QueryEngine(new SQLConfiguration()));
```

#### Schemio.EntityFramework
Construct DataProvider using `Schemio.EntityFramework.QueryEngine` query engine.

```
var provider = new DataProvider(new CustomerSchema(), Schemio.EntityFramework.QueryEngine());
```

### Using IOC for registrations

With ServiceCollection, you should call the `services.UseSchemio()` method for IoC registration.

To configure Data provider with SQL Query engine, use fluent registration apis as shown below - 
 ```
   services.UseSchemio<Customer>(With.Schema<Customer>(c => new CustomerSchema())
                .AddEngine(c => new QueryEngine(new SQLConfiguration {  ConnectionSettings = new ConnectionSettings {
                        Providername = "System.Data.SqlClient", 
                        ConnectionString ="Data Source=Powerstation; Initial Catalog=Customer; Integrated Security=SSPI;"            
                    }}))
                .LogWith(c => new Logger<IDataProvider<Customer>>(c.GetService<ILoggerFactory>())));
```

To configure Data provider with Entity Framework Query engine, use fluent registration apis shown as below - 
 ```
   services.AddDbContextFactory<CustomerDbContext>(options => options.UseSqlServer(YourSqlConnection), ServiceLifetime.Scoped);

   services.AddLogging();

   services.UseSchemio<Customer>(With.Schema<Customer>(c => new CustomerSchema())
     .AddEngine(c => new QueryEngine<CustomerDbContext>(c.GetService<IDbContextFactory<CustomerDbContext>>()))
     .LogWith(c => new Logger<IDataProvider<Customer>>(c.GetService<ILoggerFactory>())));

```

`Please Note:` You can combine multiple query engines and implement different types of queries to execute on different supported platforms.

To use Data provider, Inject IDataProvider<T> using constructor & property injection method or explicity Resolve using service provider ie. `IServiceProvider.GetService(typeof(IDataProvider<>))`

## Extend Schemio
### Custom Query Engine
To provide custom query engine and query implementations, you need to extend the base interfaces as depicted below
- IQueryEngine interface to implement the custom query engine to be used with schemio.
```
public interface IQueryEngine
    {
        /// <summary>
        /// Detrmines whether an instance of query can be executed with this engine.
        /// </summary>
        /// <param name="query">instance of IQuery.</param>
        /// <returns>Boolean; True when supported.</returns>
        bool CanExecute(IQuery query);

        /// <summary>
        /// Executes a list of queries returning a list of aggregated results.
        /// </summary>
        /// <param name="queries">List of IQuery instances.</param>
        /// <returns>List of query results. Instances of IQueryResult.</returns>
        IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries);
    }
```
Example Entity Framework implementation is below
```
public class QueryEngine<T> : IQueryEngine where T : DbContext
    {
        private readonly IDbContextFactory<T> _dbContextFactory;

        public QueryEngine(IDbContextFactory<T> _dbContextFactory)
        {
            this._dbContextFactory = _dbContextFactory;
        }

        public bool CanExecute(IQuery query) => query != null && query is ISQLQuery;

        public IEnumerable<IQueryResult> Execute(IEnumerable<IQuery> queries)
        {
            var output = new List<IQueryResult>();

            using (var dbcontext = _dbContextFactory.CreateDbContext())
            {
                foreach (var query in queries)
                {
                    var results = ((ISQLQuery)query).Run(dbcontext);

                    if (results == null)
                        continue;

                    output.AddRange(results);
                }

                return output.ToArray();
            }
        }
    }
```
- Provide base implementation supporting IQuery, IRootQuery & IChildQuery interfaces. 
- You can implement the parent and child base class implementations to construct for queries to be executed with custom engine implementation above. 

For Parent Query base implementation, see example below.
```
public abstract class BaseSQLRootQuery<TQueryParameter, TQueryResult>
        : BaseRootQuery<TQueryParameter, TQueryResult>, ISQLQuery
       where TQueryParameter : IQueryParameter
       where TQueryResult : IQueryResult
    {
        /// <summary>
        /// Get query delegate with implementation to return query result.
        /// Delegate returns a collection from db.
        /// </summary>
        /// <returns>Func<DbContext, IEnumerable<IQueryResult>></returns>
        public abstract IEnumerable<IQueryResult> Run(DbContext dbContext);
    }
```
For Child Query implementation, see example below.
```
public abstract class BaseSQLChildQuery<TQueryParameter, TQueryResult>
        : BaseChildQuery<TQueryParameter, TQueryResult>, ISQLQuery
       where TQueryParameter : IQueryParameter
       where TQueryResult : IQueryResult
    {
        /// <summary>
        /// Get query delegate with implementation to return query result.
        /// Delegate returns a collection from db.
        /// </summary>
        /// <returns>Func<DbContext, IEnumerable<IQueryResult>></returns>
        public abstract IEnumerable<IQueryResult> Run(DbContext dbContext);
    }
```
### Custom Schema Language
You can provide your own schema language support for use in entity schema definition to map sections of object graph.

To do this you need to follow the below steps
* Provide entity schema definition with query/transformer pairs using custom schema language paths
* Provide implementation of `ISchemaPathMatcher` interface and implement `IsMatch()` method to provide logic for matching custom paths. This matcher is used by query builder to pick queries for matched paths against the configured p in Entity schema definition. 
```
public interface ISchemaPathMatcher
    {
        bool IsMatch(string inputPath, ISchemaPaths configuredPaths);
    }
```
Example implementation of XPath matcher is below.
```
public class XPathMatcher : ISchemaPathMatcher
    {
        private static readonly Regex ancestorRegex = new Regex(@"=ancestor::(?'path'.*?)(/@|\[.*\]/@)", RegexOptions.Compiled);

        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths)
        {
            if (inputXPath == null)
                return false;

            if (configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower())))
                return true;

            if (configuredXPaths.Paths.Any(x => inputXPath.Contains("ancestor::")
                    && ancestorRegex.Matches(inputXPath).Select(match => match.Groups["path"].Value).Distinct().Any(match => x.EndsWith(match))))
                return true;

            return false;
        }
    }
```

## Credits
Thank you for reading. Please fork, explore, contribute and report. Happy Coding !! :)




