# Developer Guide

## i. Installation

`Schemio` allows you to aggregate data from heterogeneous data stores offering `SQL` & `API` packages out of the box below. SQL queries are supported by `Dapper` and `EntityFramework` engines. You could also `extend` Schemio to provide your own implementation(s) of `Query` and supporting `Query Engine` to retrieve data from `custom` data store(s). 

Below are the Nuget packages available. 

`Scemio.Core` - Install  to extend schemio to implement `custom` querying engine. 
```
NuGet\Install-Package Schemio.Core
```
`Schemio.SQL` - Install when you would like to include SQL `Dapper` queries to access SQL database.
```
NuGet\Install-Package Schemio.SQL 
```
`Schemio.EntityFramework` - Install when you would like to include SQL `EntityFramework` queries to access SQL database.
```
NuGet\Install-Package Schemio.EntityFramework
```
`Schemio.Api` - Install when you would like to include web api queries with `HttpClient` query engine.
```
NuGet\Install-Package Schemio.Api
```
## ii. Implementation: Using Schemio

To use **Schemio** you need to do the below steps
- **Step 1**: Define the aggregated `Entity`.
- **Step 2**: Setup the aggregate `Configuration` comprising of `Query`/`Transformer` hierarchical nested mappings.
- **Step 3**: Construct the `DataProvider` with required dependencies. 

### Step 1. Define Aggregate Entity.
To create an aggregate `Entity`, implement the class from `IEntity` interface. This is the entity that will be returned as aggregated result from multiple queries assembled to execute against homogeneous or heterogeneous data storage's.

Below is an example `Customer` entity.

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

For the customer class, we can see there are three levels of nesting in the object graph.
- Level 1 with paths: `Customer`
- Level 2 with paths: `Customer.Communication` and `Customer.Orders`
- Level 3 with paths: `Customer.Orders.Items`

If we choose XML Schema Definition (XSD) for the Customer entity then XPaths for nesting levels should be.
```
- Level 1 with XPath: Customer
- Level 2 with XPaths: Customer/Communication and Customer/Orders
- Level 3 with XPath: Customer/Orders/Order/Items/Item
```
### Step 2: Setup Entity Aggregate Configuration
To define `Entity Configuration`, derive from `EntityConfiguration<TEntity>` class where `TEntity` is aggregate entity in context (ie. `IEntity`).

The `Entity Configuration` is basically `hierarchies` of `Query` & `Transformer` pairs mapped to the schema `paths` pointing to various `nesting` levels in the entity's object graph.
* `Query` is an implementation to `fetch` data for mapped sections of object graph. 
* `Transformer` is an implementation to `map` data fetched by the associated query to the relevant sections of the entity's object graph.

Below is an example Entity Configuration for the Customer Entity.

```
internal class CustomerConfiguration : EntityConfiguration<Customer>
{
   public override IEnumerable<Mapping<Customer, IQueryResult>> GetSchema()
   {
       return CreateSchema.For<Customer>()
           .Map<CustomerQuery, CustomerTransform>(For.Paths("customer"),
            customer => customer.Dependents
               .Map<CommunicationQuery, CommunicationTransform>(For.Paths("customer/communication"))
               .Map<OrdersQuery, OrdersTransform>(For.Paths("customer/orders"),
                   customerOrders => customerOrders.Dependents
                       .Map<OrderItemsQuery, OrderItemsTransform>(For.Paths("customer/orders/order/items"))))
           .End();
   }
}
```
`CustomerConfiguration` shows `query/transformer` pairs mapped at three levels of nesting as per the `Customer` entity object graph.
`XPaths` are used to identify the schema paths in the object graph. Alternately, you could use your own representation to name the pairs or map the object graph. However, you would need to provide the `ISchemaPathmatcher` implementation to managing path matching.

#### i. Query/Transformer Mapping
Every `Query` type in the Entity `Configuration` definition should have a complementing `Transformer` type.
You could map multiple `schema paths` to a given query/transformer pair. Currently, `XPath` and `JSONPath` schema languages are supported.

Below is the snippet from `CustomerConfiguration` definition shows that `CustomerQuery` has associated `CustomerTransform` and the pair is mapped to the root `Customer` object. 
```
  .Map<CustomerQuery, CustomerTransform>(For.Paths("customer"))
```

#### ii. Nested Query/Transformer Mappings
You could nest query/transformer pairs in a `parent/child` hierarchy. In which case the output of the parent query will serve as the input to the child query to resolve its query context.

The query/transformer mappings can be `nested` to `5` levels down.
 
Below is snippet to show nesting of `CommunicationQuery` as child to `CustomerQuery`. 
```
.Map<CustomerQuery, CustomerTransform>(For.Paths("customer"), -- Parent
      customer => customer.Dependents
         .Map<CommunicationQuery, CommunicationTransform>(For.Paths("customer/communication")) -- Child
```

Execution Flow
* In parent/child hierarchy, the first parent query executes first, followed by its immediate children. The execution flows in sequence to the last child query in order of its nesting.
* While executing the output of the parent is passed in to the child query to resolve query context and get it ready for execution. 
* Transformers are also executed in the same sequence to map data to the Aggregate Entity.
* When a query path for nested query is included for execution, all the parent queries involved in that object graph get included for execution  in order of its nesting.

Please see the execution sequence below for queries and transformers nested in `CustomerConfiguration` implemented above.

<img width="1202" alt="image" src="https://github.com/CodeShayk/Schemio/blob/master/Images/EntitySchemaDefinition.png">

#### iii. Query Class
`Query` - The purpose of a query class is to execute with supported QueryEngine to fetch data from data storage.

`QueryEngine` is an implementation of `IQueryEngine` to execute queries with supported data storage to return query result (ie. Result instance of `IQueryResult`).

Depending on the Nuget package(s) installed, you could implement `SQL` and `API` queries.
* `SQL` queries execute to get data from SQL database using `Dapper` or `EntityFramework` engines.
* `API` query executes web api to call an `endpoint` using `HTTPClient` supported engine to get data.

**Important**: You can combine heterogeneous queries in the Entity configuration to target different data stores.

Example of SQL & API queries are below. 
You need to override the `GetQuery(IDataContext context, IQueryResult parentQueryResult)` method to return query delegate (package specific implementation). 
* `IDataContext` is the context parameter passed to DataProvider to get aggregated results (. Aggregated Entity). This parameter is always available for both parent and child queries.
* `IQueryResult` parameter is only available when query is configured in child mode, else will be null.

##### `Schemio.SQL` - with `Dapper` Query implementation. 

1. Example Parent Query - CustomerQuery
```
public class CustomerQuery : BaseSQLQuery<CustomerResult>
{
    protected override Func<IDbConnection, Task<CustomerResult>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
    {
        // Executes as root or level 1 query.
        var customer = (CustomerContext)context.Entity;

        return connection => connection.QueryFirstOrDefaultAsync<CustomerResult>(new CommandDefinition
        (
            "select CustomerId as Id, " +
                   "Customer_Name as Name," +
                   "Customer_Code as Code " +
            $"from TCustomer where customerId={customer.CustomerId}"
       ));
    }
}
```
2. Example Child Query - OrdersQuery
```
internal class OrdersQuery : BaseSQLQuery<CollectionResult<OrderResult>>
{
    protected override Func<IDbConnection, Task<CollectionResult<OrderResult>>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
    {
        // Execute as child to customer query.
        var customer = (CustomerResult)parentQueryResult;

        return async connection =>
        {
            var items = await connection.QueryAsync<OrderResult>(new CommandDefinition
            (
                "select OrderId, " +
                        "OrderNo, " +
                        "OrderDate " +
                    "from TOrder " +
                $"where customerId={customer.Id}"
            ));

            return new CollectionResult<OrderResult>(items);
        };
    }
}
```

##### `Schemio.EntityFramework` - with `EntityFramework` Query implementation

1. Example Parent Query - CustomerQuery
```
public class CustomerQuery : BaseSQLQuery<CustomerResult>
{
    protected override Func<DbContext, Task<CustomerResult>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
    {
        // Executes as root or level 1 query. parentQueryResult will be null.
        var customer = (CustomerContext)context.Entity;

        return async dbContext =>
        {
            var result = await dbContext.Set<Customer>()
                    .Where(c => c.Id == customer.CustomerId)
                    .Select(c => new CustomerResult
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Code = c.Code
                    })
                    .FirstOrDefaultAsync();

            return result;
        };
    }
}
```
2. Example Child Query - OrdersQuery
```
 internal class OrdersQuery : BaseSQLQuery<CollectionResult<OrderResult>>
 {
     protected override Func<DbContext, Task<CollectionResult<OrderResult>>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
     {
         // Execute as child to customer query.
         var customer = (CustomerResult)parentQueryResult;

         return async dbContext =>
         {
             var items = await dbContext.Set<Order>()
             .Where(p => p.Customer.Id == customer.Id)
             .Select(c => new OrderResult
             {
                 CustomerId = c.CustomerId,
                 OrderId = c.OrderId,
                 Date = c.Date,
                 OrderNo = c.OrderNo
             })
             .ToListAsync();

             return new CollectionResult<OrderResult>(items);
         };
     }
 }
```
##### `Schemio.Api` - with `HttpClient` Query implementation
1. Example Parent Query - CustomerQuery
```
public class CustomerQuery : BaseApiQuery<CustomerResult>
{
     public CustomerQuery() : base(Endpoints.BaseAddress)
     {
     }
    protected override Func<Uri> GetQuery(IDataContext context, IQueryResult parentQueryResult)
    {
        // Executes as root or level 1 query.
        var customer = (CustomerContext)context.Entity;
    
        return ()=> new Uri(string.Format($"v2/customers/{customer.CustomerId});
    }
}
```
2. Example Child Query - OrdersQuery
```
internal class OrdersQuery : BaseApiQuery<CollectionResult<OrderResult>>
{
     public OrdersQuery() : base(Endpoints.BaseAddress)
     {
     }
    protected override Func<Uri> GetQuery(IDataContext context, IQueryResult parentQueryResult)
    {
         // Execute as child to customer api.
        var customer = (CustomerResult)parentApiResult;
    
        return ()=> new Uri(string.Format($"v2/customers/{customer.Id}/orders);
    }
}
```

#### iv. Transformer Class
The purpose of the transformer class is to transform the data fetched by the linked query class and map to the configured object graph of the entity.

To define a transformer class, you need to implement `BaseTransformer<TQueryResult, TEntity>`
- where TEntity is Aggregate Entity implementing `IEntity`. eg. Customer. 
- where TQueryResult is Query Result from associated Query. It is an implementation of `IQueryResult` interface. 

Example transformer - Customer Transformer
```
internal class CustomerTransform : BaseTransformer<CustomerResult, Customer>
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

**Note**: It is `important` that the transformer should map data only to the `schema path(s)` pointing `section(s)` of the object graph.

For the example query/transformer mapping
```
.Map<CommunicationQuery, CommunicationTransform>(For.Paths("customer/communication"))
```
The Communication transformer should map data only to the `customer/communication` xpath mapped object graph of customer class.

### Step 3. DataProvider Setup
Data provider needs to be setup with required dependencies. Provide implementations of below dependencies to construct the data provider.

#### Container Registrations

With ServiceCollection, you need to register the below dependencies.

```
    // Register core services
    services.AddTransient(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));
    services.AddTransient(typeof(IEntityBuilder<>), typeof(EntityBuilder<>));
    services.AddTransient(typeof(IDataProvider<>), typeof(DataProvider<>));
    services.AddTransient<IQueryExecutor, QueryExecutor>();

    // Register instance of ISchemaPathMatcher - Json, XPath or Custom.
    services.AddTransient(c => new XPathMatcher());

    // Enable logging
    services.AddLogging();

    //For Dapper SQL engine - Schemio.SQL
    services.AddTransient<IQueryEngine>(c => new QueryEngine(new SQLConfiguration {  ConnectionSettings = new ConnectionSettings {
                Providername = "System.Data.SqlClient", 
                ConnectionString ="Data Source=Powerstation; Initial Catalog=Customer; Integrated Security=SSPI;"            
            }}); 

    // For entity framework engine - Schemio.EntityFramework
    services.AddDbContextFactory<CustomerDbContext>(options => options.UseSqlServer(YourSqlConnection), ServiceLifetime.Scoped);
    services.AddTransient<IQueryEngine>(c => new QueryEngine<CustomerDbContext>(c.GetService<IDbContextFactory<CustomerDbContext>>());
               
    // For HTTPClient Engine for web APIs - Schemio.API
    
    // Enable HttpClient
    services.AddHttpClient();
    services.AddTransient<IQueryEngine, QueryEngine>();

    // Register each entity configuration. eg CustomerConfiguration
    services.AddTransient<IEntityConfiguration<Customer>, CustomerConfiguration>();
    
 ```

`Please Note:` You can combine multiple query engines and implement supporting types of queries to execute on target data platforms.


#### Using Fluent interface for registrations

i. Example registration: Schemio.SQL

```
// Enable DbProviderFactory.
 DbProviderFactories.RegisterFactory(DbProviderName, SqliteFactory.Instance);
 
 var connectionString = $"DataSource={Environment.CurrentDirectory}//Customer.db;mode=readonly;cache=shared";
 
 var configuration = new SQLConfiguration { ConnectionSettings = new ConnectionSettings { ConnectionString = connectionString, ProviderName = DbProviderName } };

// Enable logging
 services.AddLogging();

 services.UseSchemio()
     .WithEngine(c => new QueryEngine(configuration))
     .WithPathMatcher(c => new XPathMatcher())
        .WithEntityConfiguration<Customer>(c => new CustomerConfiguration());
```

ii. Example registration: Schemio.EntityFramework

```
 var connectionString = $"DataSource={Environment.CurrentDirectory}//Customer.db;mode=readonly;cache=shared";

// Enable DBContext Factory
 services.AddDbContextFactory<CustomerDbContext>(options =>
         options.UseSqlite(connectionString));

// Enable logging
 services.AddLogging();
 
  services.UseSchemio()
      .WithEngine(c => new QueryEngine<CustomerDbContext>(c.GetService<IDbContextFactory<CustomerDbContext>>()))
      .WithPathMatcher(c => new XPathMatcher())
      .WithEntityConfiguration<Customer>(c => new CustomerConfiguration());
           
```
iii. Example registration: Schemio.API
```
 // Enable logging
 services.AddLogging();

 // Enable HttpClient
  services.AddHttpClient();
  
  services.UseSchemio()
      .WithEngine<QueryEngine>()
      .WithPathMatcher(c => new XPathMatcher())
      .WithEntityConfiguration<Customer>(c => new CustomerConfiguration());
  
```
iv. Example registration: Multiple Engines
```
 // Enable logging
 services.AddLogging();

 // Enable HttpClient
  services.AddHttpClient();
  
 var connectionString = $"DataSource={Environment.CurrentDirectory}//Customer.db;mode=readonly;cache=shared";

// Enable DBContext Factory
 services.AddDbContextFactory<CustomerDbContext>(options =>
         options.UseSqlite(connectionString));
  
  services.UseSchemio()
      .WithEngine<QueryEngine>()
      .WithEngine(c => new QueryEngine<CustomerDbContext>(c.GetService<IDbContextFactory<CustomerDbContext>>()))
      .WithPathMatcher(c => new XPathMatcher())
      .WithEntityConfiguration<Customer>(c => new CustomerConfiguration());
```

#### Use Data Provider 

##### i. Dependency Inject - IDataProvider

To use Data provider, Inject `IDataProvider<T>` where T is IEntity, using constructor & property injection method or explicitly Resolve using service provider ie. `IServiceProvider.GetService(typeof(IDataProvider<Customer>))`

##### ii. Call DataProvider.GetData(IEntityContext context) method.
You need to call the `GetData()` method with an instance of parameter class derived from `IEntityContext` interface.

The `IEntityContext` provides a `SchemaPaths` property, which is a list of schema paths to include for the given request to fetch aggregated data.
- When `no` paths are passed in the parameter then entire aggregated entity for all configured queries is returned.
- When list of schema paths are included in the request then the returned aggregated data entity only includes query results from included queries.

When nested path for a nested query is included (eg. customer/orders/order/items) then all parent queries in the respective parent paths also get included for execution..
Example - Control Flow 
TBC

## Extending Schemio

You could extend Schemio by providing your own custom implementation of the query engine (`IQueryEngine`) and query (`IQuery`) to execute queries on custom target data platform. 

To do this, you need to extend the base interfaces as depicted below.
### i. IQueryEngine
Implement `IQueryEngine` interface to provide the custom query engine to be used with schemio.
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
    /// Executes a given query returning query result.
    /// </summary>
    /// <param name="queries">Custom instance of IQuery.</param>
    /// <returns>Task of IQueryResult.</returns>
    Task<IQueryResult> Execute(IQuery> query);
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

    public Task<IQueryResult> Execute(IQuery query)
    {
        using (var dbcontext = _dbContextFactory.CreateDbContext())
        {
            var result = ((ISQLQuery)query).Run(dbcontext);
            return result;
        }
    }
}
```
### ii. IQuery
With the Query Engine implementation, you also need to provide custom implementation of `IQuery` for executing the query custom query engine.

To do this, you need to extend `BaseQuery<TResult>` where TQueryResult is `IQueryResult`. And, provide overrides for below methods.
- `bool IsContextResolved()`
Engine calls this method to confirm whether the query is ready for execution. Return true when query context is resolved.
- `void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)`
This method is invoked by schemio to resolve the query context required for execution ith supporting query engine. `IQueryResult` parameter is only available when the custom query is configured in nested or child mode.


Example - EntityFramework Supported query implementation is shown below.
```
 public abstract class BaseSQLQuery<TQueryResult>
     : BaseQuery<TQueryResult>, ISQLQuery
    where TQueryResult : IQueryResult
 {
     private Func<DbContext, Task<TQueryResult>> QueryDelegate = null;

     public override bool IsContextResolved() => QueryDelegate != null;

     public override void ResolveQuery(IDataContext context, IQueryResult parentQueryResult)
     {
         QueryDelegate = GetQuery(context, parentQueryResult);
     }

     async Task<IQueryResult> ISQLQuery.Run(DbContext dbContext)
     {
         return await QueryDelegate(dbContext);
     }

     /// <summary>
     /// Get query delegate to return query result.
     /// </summary>
     /// <param name="context"></param>
     /// <param name="parentQueryResult"></param>
     /// <returns></returns>
     protected abstract Func<DbContext, Task<TQueryResult>> GetQuery(IDataContext context, IQueryResult parentQueryResult);
 }
```

### iii. Custom Schema Paths

Additionally, You can use your own schema language instead of XPath/JSONPath to map aggregated entity's object graph, and register with schemio.

To do this you need to follow the below steps:
#### i. Use schema paths in Entity Configuration.

Provide entity schema definition with query/transformer pairs using custom schema language paths. 

Example - with Dummy schema mapping
```
.Map<OrderQuery, OrderTransform>(For.Paths("customer$orders"))
```
#### ii. ISchemaPathMatcher 
Provide implementation of `ISchemaPathMatcher` interface and implement `IsMatch()` method to provide logic for matching custom paths. 

`Important`: This matcher is used by query builder to filter queries based matched paths, to include only required queries for execution to optimize performance.
```
public interface ISchemaPathMatcher
{
    bool IsMatch(string inputPath, ISchemaPaths configuredPaths);
}
```
Example implementation of XPathMatcher is below.
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
