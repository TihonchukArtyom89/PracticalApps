# PracticalApps
## This my training project, where i work on some technologies list of which with explanation is below. 
## Stack: 
 - LINQ - Using query to objects in EF Core entities.
- EF Core - Database First. I use free database from Microsoft "Northwind.db".
 - ASP.NET Core
 - Razor Pages
 - Unit test
 - SQLite - Database provider.
 - ASP.NET Core MVC
 - WebApi/Minimal WebApi
 - Blazor Server/WebAssembly
## Projects in solution:
 - Northwind.Common.DataContext.Sqlite - Class library project for EF Core Entity models.
 - Northwind.Common.EntityModels.Sqlite - Class library project for the EF Core Entity database context with dependencies on database provider.
 - Northwind.Common.UnitTests - Unit Test project for testing base functionality of EF Core.
 - Northwind.Razor.Customers - Class library project for Razor pages to demonstrate list of Customers with clickable links to view full information of customer and they orders.
 - Northwind.Razor.Employees - Class library project for Razor pages to demonstrate list of Employees.
 - Northwind.Web - ASP.NET Core project for a website with mix of static HTML and dynamic Razor Pages.
 - Northwind.Common.PrimeFactors - Class library project with code from my early made console app (decomposition number on prime factors).
 - Northwind.Mvc - ASP.NET Core project with using MVC design pattern to create website with using two services (launch with Northwind.WebApi and Minimal.WebApi projects at the same time).
 - Northwind.WebApi - ASP.NET Core WebApi project.
 - Minimal.WebApi - ASP.NET Core WebApi project without selecting checkbox to use controllers and views.
 - Northwind.BlazorServer - Blazor Server project has CRUD for customers (get customers form db with help local service dependency) and routable page component (timestable).
 - Northwind.BlazorWasm(Client,Server,Shared) - Blazor WebAssembly App projects has CRUD for customers (get customers form db with help customers web api controller). 
