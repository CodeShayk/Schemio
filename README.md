# <img src="https://github.com/CodeShayk/Schemio/blob/master/Images/ninja-icon-16.png" alt="ninja" style="width:30px;"/> Schemio v1.0 
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/CodeShayk/Schemio/blob/master/LICENSE.md) 
[![Master-Build](https://github.com/CodeShayk/Schemio/actions/workflows/Build-Master.yml/badge.svg)](https://github.com/CodeShayk/Schemio/actions/workflows/Build-Master.yml) 
[![GitHub Release](https://img.shields.io/github/v/release/CodeShayk/Schemio?logo=github&sort=semver)](https://github.com/CodeShayk/Schemio/releases/latest)
[![Master-CodeQL](https://github.com/CodeShayk/Schemio/actions/workflows/Master-CodeQL.yml/badge.svg)](https://github.com/CodeShayk/Schemio/actions/workflows/Master-CodeQL.yml) 
[![.Net 8.0](https://img.shields.io/badge/.Net-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
--
 #### Nuget Packages
| Latest  | Details | 
| -------- | --------|
| ![NuGet Version](https://img.shields.io/nuget/v/Schemio.Core?style=for-the-badge&label=Schemio.Core&labelColor=green) | Provides `core` functionality to configure nested queries and transformers. With ability to map schema paths (XPath/JSONPath) to entity's object graph. `No QueryEngine` provided and requires implementing IQueryEngine to execute IQuery instances. |
| ![NuGet Version](https://img.shields.io/nuget/v/Schemio.SQL?style=for-the-badge&label=Schemio.SQL&labelColor=green) | Provides schemio with query engine using Dapper to execute SQL queries. |
|![NuGet Version](https://img.shields.io/nuget/v/Schemio.EntityFramework?style=for-the-badge&label=Schemio.EntityFramework&labelColor=green) | Provides schemio with `Entity Framework` query engine to execute queries using DbContext. |

## Concept
### What is Schemio?
`Schemio` is a .net utility to hydrate an entity with data by specifying schema paths or sections of its object graph.
> Supports XPath & JsonPath for schema paths.

### When to use Schemio?
Schemio is perfect fit when you need to fetch parts of a large entity from given data storage. Ideally, you may not want all of the entity data but preferably only sections of the object graph depending on the context for fetch.

Few example schemio use cases that require the service tier to dynamically fetch data for high performance, availability and scalability are
> - Reporting
> - Document Generation ( with templated data)
> - Content Management Systems
> - Many more

## Getting Started?
### i. Installation
Install the latest nuget package as appropriate for Core, SQL using Dapper or EntityFramework. 

`Scemio.Core` - for installing schemio for `bespoke` implementation of query engine.
```
NuGet\Install-Package Schemio.Core
```
`Schemio.SQL` - for installing schemio for SQL with `Dapper` engine.
```
NuGet\Install-Package Schemio.SQL
```
`Schemio.EntityFramework` - for installing schemio for SQL with `EntityFramework` engine.
```
NuGet\Install-Package Schemio.EntityFramework
```

### ii. Developer Guide

Please see [Developer Guide](/DeveloperGuide.md) for details on how to implement schemio in your project.

## Support

If you are having problems, please let me know by [raising a new issue](https://github.com/CodeShayk/Schemio/issues/new/choose).

## License

This project is licensed with the [MIT license](LICENSE).

## Version History
The main branch is now on .NET 8.0. The following previous versions are available:
| Version  | Release Notes | Developer Guide |
| -------- | --------|--------|
| [`v1.0.0`](https://github.com/CodeShayk/Schemio/tree/v1.0.0) |  [Notes](https://github.com/CodeShayk/Schemio/releases/tag/v1.0.0) | [Guide](https://github.com/CodeShayk/Schemio/blob/v1.0.0/DeveloperGuide.md) |
| [`Pre-Release v2.0.0`](https://github.com/CodeShayk/Schemio/tree/v2.0.0) |  [Notes](https://github.com/CodeShayk/Schemio/releases/tag/v2.0.0) | [Guide](https://github.com/CodeShayk/Schemio/blob/v2.0.0/DeveloperGuide.md) |

## Credits
Thank you for reading. Please fork, explore, contribute and report. Happy Coding !! :)




