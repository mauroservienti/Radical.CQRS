# Radical.CQRS

[![Build status](https://ci.appveyor.com/api/projects/status/wkv5fra1jdqjy2lp?svg=true)](https://ci.appveyor.com/project/radical-bot/radical-cqrs)

We (@nazarenomanco and I) have a dream: allow everyone to use CQRS to implement better architectures.

Radical.CQRS is our way to help people in the above direction.

### Samples

2 samples are included in the solution, one is a single tier WPF app that utilizes the domain model directly, the second one is a 2 tiers app composed of a WPF client using OData and HTTP to communicate to a self hosted OWIN server that hosts the domain model and all the endpoints.

In order to run the sample(s) the database schema must created upfront running `Entity Framework` DB migrations, issuing the following command at the Visual Studio `Package Manager Console`:

    Update-Database -ConfigurationTypeName "Sample.Migrations.SampleDomainContext.Configuration" -StartUpProjectName "Sample.Migrations" -ProjectName "Sample.Migrations" -Verbose

The connection string used by default is the following:

    Data Source=.\SqlExpress;Initial Catalog=CqrsSample;Integrated Security=True

In order to change where the database is located the following connection strings need to be updated:

* `app.config` connection string section in the `Sample.Migrations` project to update where the DB migrations are run;
* Both the connection strings in `app.config` of the `Sample.Server` project to update where the 2 tier sample will look for data;
* Both the connection strings in `app.config` of the `Sample.WpfClient` project, of the 1 tier sample, to update where the 1 tier sample will look for data;
