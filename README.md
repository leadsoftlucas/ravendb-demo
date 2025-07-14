
[![RavenDB](https://ravendb.net/wp-content/uploads/2023/08/raven-logo-blue.svg)
](https://ravendb.net) 

This repository purpose is to highlight three of RavenDB's most impressive and differentiating features rather than other database products.

## **Intro**

Welcome to this repository that will demonstrate how easy and amazing is to connect and use **RavenDB** into a .net 9.0 c# Rest Web API application and a short comparison with **PostgreSQL**.
My goal here, is to bring the ease of use to achieve fantastic results on application features in an effortless code base.

You'll be able to experience how fast we can create an application with high performance and efficiency, both on coding and runtime results.

## **Resume**

Before diving into the key features I will demonstrate a change of mindset when we discuss data modeling and why it makes all the difference on our database and code style choice.

A great question to ask youself before building a new application or solution is: `"Where do I want to spend my time?"`. And a very quick response for that, is to focus on the core business, focus on true valued code, so the best approach would be modeling how you'll persist you data, right? Well... sort of! Maybe the business doesn't demand a very clear data model, and how would you define that and keep you code synced with your database? Seems like a mess if you're designing a SQL data model.
Before I go deeper, remember one thing. We're in the beginning of the AI age, so I presume you don't want to maintain tables that might fit all possibilities of an AI response, that would be insane. What would be you idea? Please, don't say that you would store tons of json data into a table column. Please, don't. We have a more acurate solution for that.

Let's get back into the main problem: We want to store data, that fit's our business needs language (glossary) and we don't know exacly what kind of fields we might use. Well, we must track the `Resources` of this business and map the relationship between them, at least. ([Domain Driven Design](https://fullcycle.com.br/domain-driven-design/), have you ever heard about it?) But for dynamic contents, we might need dynamic fields. So I guess we aggree that a multimodel database is a great option here.

> But, **"how would we select this data?"**, or even more, **how would we search and find data without killing my server cluster or my cloud fundings?**.

Wait, you won't need NASA computers for that, you just need a proper database with incredible fast queries, that you won't need to preocupy to setup indexes. Well, have you ever heard about **[RavenDB](https://ravendb.net)**?

> Let's fly high with Rook, we'll show you.

## **Key features**

### 1. **Full-text search, auto-indexing and Counters**

RavenDB provides built-in support for full-text search, allowing you to perform advanced searches on your data without the need for complex configurations or additional libraries. And differently from other databases that uses LIKE based search, your server won't go down slow if you full text search on a very large collection.

You can chose several fields and combine how you want to interpretate the search terms, and apply where clauses to filter the results. The magic here are the auto-index amazing feature in RavenDB, that will create the indexes for you based on the queries you perform. This means you don't have to worry about creating and maintaining indexes manually, as RavenDB will handle it for you automatically. What is a nightmare on MongoDB...
And the indexes are queried automatically when you search a collection, so you can focus on building your application without worrying about performance issues. On MongoDB, you have to create indexes manually and maintain them, and if you were quering a collection and needs to create an index, you'll have to change or code to search the index instead of the collection, what can induce mistakes and waste your developers trying to find out why is it still slow.

#### 1.1 **Counters**

Another interesting thing is the usage of **Counters** in RavenDB. Counters are a powerful feature that allows you to track and aggregate data efficiently without the need for complex queries or joins.
They are particularly useful for scenarios where you need to maintain counts, statistics, or other aggregated values in simple endpoints or when you need to track changes over time or in the middle of a process.
Imagine that you might need to register an interaction count of a AI agent activation by user: Would you create a table to store integers and User Ids? It would increase your model complexity.

| Feature              | RavenDB Counters      | PostgreSQL Equivalent             |
| -------------------- | --------------------- | --------------------------------- |
| Server-side counters | ✅ Native & automatic | ❌ Not built-in, but **emulated** |
| Stored separately    | ✅ Yes                | ✅ With separate table            |
| No schema needed     | ✅ Yes                | ⚠️ Only with JSONB (not ideal)    |
| Independent from doc | ✅ Yes                | ✅ With normalized model          |
| Atomic updates       | ✅ Yes                | ✅ Supported via `UPDATE`         |

> Trust me, if you have a very complex data model with several entities and relations, you don't want to use counters in PostgreSQL, because you'll have to create a table for each counter and manage them manually.
> In RavenDB, you just need to use the `Counters` API and you're done. You can increment or decrement counters without worrying about the underlying data structure.

I've created a demonstration on YouTube, you can watch below:
> [![RavenDB vs PostgreSQL - Part 2: Bulk Insert, Fulltext Search and Counters](https://img.youtube.com/vi/Y8hXso9qCf4/hqdefault.jpg)
<br> RavenDB vs PostgreSQL - Part 2: Bulk Insert, Fulltext Search and Counters](https://www.youtube.com/watch?v=Y8hXso9qCf4)

### 2. **Semantic search with Vector search and embeddings**

The experience of using semantic search with vector search and embeddings in RavenDB is truly remarkable. It allows you to perform advanced searches that go beyond simple keyword matching, enabling you to find relevant documents based on their meaning and context.
This is particularly useful in scenarios where you need to search for documents that are semantically similar, even if they don't contain the exact same keywords. RavenDB 's vector search capabilities leverage embeddings to represent documents in a high-dimensional space, allowing for efficient and accurate similarity searches.

And there is more, RavenDB provides built-in support for vector search, making it easy to integrate into your application without the need for complex configurations or additional libraries. But if you want to use an external library, you can do that too, RavenDB is flexible enough to allow you to use your preferred libraries for vector search and embeddings.
Of course it depends on your license, but you can enable it and plug OpenAI, Google AI, Ollama, and let them generate embeddings for your documents, and then use those embeddings to perform semantic searches in your RavenDB database.

What? A database that UNDERSTANDs the meaning of your data and can search for it based on your wish context? Yes, that's right!
Once you search by vector, it will generate vector indexes for you, so you'll be able to search using high speed semantics results. And the acuracy is customizable, so you can adjust the search results to fit your needs.

> I did tests on PosgreSQL with Entity Framework. It was easy and seems to work, but did not bring results as I expected. Words and phrases that were semantically similar to database content, that worked on RavenDB Vector Seach, did not return results in PostgreSQL.

> *There is a demonstration video right after item 3.**

### 3. **Dynamic data model with AI**

An interesting detail on RavenDB data modeling experience, is that you can apply DDD (Domain Driven Design) principles to your data model. You can create your entities with calculated properties and use them as filters to query your data.
This is a powerful feature that allows you to create complex queries without the need for complex joins or aggregations. And it simplifies a lot the queries and data display in the application.

You can bring sums, lists, averages, currency calculations, and even complex data structures without worrying about the underlying data structure. This is particularly useful in scenarios where you need to display data in a user-friendly way, such as in dashboards or reports: direct from you class.

I've created some interesting interactive OpenAI integration to register dinamically a Guest into database. I just did it to RavenDB during my deadline for this test project. To use it, go the the `LeadSoft.Adapter.OpenAI_Bridge.OpenAI_Credential.resx` resource file and set your OpenAI API credentials.
Then, start the chat, and complete the messages to fill your registration by the AI. After the AI informs that you are free to go, you can check the quality of the dynamic data generated into RavenDB or retrieving the registration by getting it by `GuestId` Guid generated on return object.

The first key feature of RavenDB is its ability to handle dynamic data models. This means you can easily adapt your data structure as your application evolves without the need for complex migrations or schema changes.
This is particularly useful in scenarios where the data requirements are not fully defined upfront, such as when working with AI-generated content or rapidly changing business needs.

In a world of AI, it is incredible if we can store data without worrying about the exact structure beforehand. RavenDB allows you to do just that, enabling you to focus on building your application without getting bogged down in database schema management.
So you can populate data that the AI generates and the AI can also read in the future, and might change the model structure along the way. This flexibility is a game-changer for developers working with AI and dynamic data, that can grow dynamically as the feature evolves.

It's possible to store the whole AI context objects for further analisys and context recreation. And this is amazing, because ou can cross and query it very easly. And even more, you can *READ* it from RavenDB Studio in a very simple and non technical way. You don't need to be technical to navigate into data in RavenDB.

I've created a demonstration on YouTube, you can watch below:
> [![RavenDB vs PostgreSQL - Part 3: VectorSearch and AI agent registration with dynamic model](https://img.youtube.com/vi/hOlWLPmd1RU/hqdefault.jpg)
<br> RavenDB vs PostgreSQL - Part 3: VectorSearch and AI agent registration with dynamic model](https://www.youtube.com/watch?v=hOlWLPmd1RU)

## **Project setup**

### 1. **Clone the repository**

To get started, clone the repository to your local machine using the following command:

```bash
git clone git@github.com:leadsoftlucas/ravendb-demo.git
```
You can open the solution in Visual Studio or any other IDE of your choice. Before running the application, make sure you have the required dependencies installed.
You must configure the environement variables for the application to run properly, the most important ones are the database scope you want to connect, as **RavenDB** or **PostgreSQL** and the RavenDB password.

`LucasRT.RavenDB.Demo.RestAPI/Properties/launchSettings.json`

```json
    "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "DATABASE_SCOPE": "RavenDB",
        //"DATABASE_SCOPE": "PostgreSQL",
        "RAVENDB_PASSWORD": "Publisher_Local_X1"
      }
```

### 2. **RavenDB Scope**

Setup the environment for the RavenDB scope. This section will guide you through the steps to run RavenDB locally and configure it in your project.
`"DATABASE_SCOPE": "RavenDB"`.

#### 2.1 **Download and run RavenDB if you want to run it locally**
You can download the latest version of RavenDB from the [official website](https://ravendb.net/downloads). Follow the instructions to install and run it on your local machine.
- If you are using Windows, go to the RavenDB folder, right-click on `run.ps1`, follow the quick terminal instructions and it will start the RavenDB server for you. **Easy peasy!**.
- Your browser will open automatically, and you can access the RavenDB Studio at the port you configured and you can follow the wizard from the web. No extra installation needed.
- Create a new database and name it as you wish using the wizard. You can use the "Quick Create" option to create a new database quickly.

#### 2.2 **Create a certificate for the RavenDB connection and configure it into the project**
- Create your certificate, and set the password to `Publisher_Local_X1` (or whatever you want, but remember to change it in the `launchSettings.json` file).
- Configure this certificate in the RavenDB Studio and allow the database you want to connect to use it.
- Paste the certificate `.pfx` file to the `LucasRT.RavenDB.Demo.RestAPI/Certificates` folder and set it as `EmbeddedResource` on right-click: Properties.
- Open the Development AppSettings file `LucasRT.RavenDB.Demo.RestAPI/appsettings.development.json` and set the nodes `Urls` of your server, the database `name` you created and the `RavenDB:ResourceName` to link your `Embedded Resource` certificate file, like this:

```json
{
  "RavenSettings": {
    "Urls": [
      "https://a.leadsoftx1.ravendb.community"
    ],
    "DatabaseName": "LucasTavares",
    "ResourceName": "RavenDB_Development"
  }
}
```

### 3. **PostgreSQL Scope**

Setup the environment for the PostgreSQL scope. This section will guide you through the steps to run PostgreSQL locally and configure it in your project.
`"DATABASE_SCOPE": "PostgreSQL"`.

I've used Entity Framework to create a fair comparison considered the database sync and the sabe semantics on queries on service layer. I did't want to create a Raw SQL or Dapper perspective because the code would be too diferent to a quick comparison.

> Although, I do prefer Raw SQL and Dapper if i need to use relational database.

#### 3.1 **Download and run PostgreSQL if you want to run it locally**

Download and run PostgreSQL from [official website](https://www.postgresql.org/download/) if you want to run it locally. Run the service. Note that here you'll begin to see the difference between a multimodel database made to be simple and a typical relational database that requires several steps to setup, as you'll need to create the database and configure it before running the application.

You can watch the video below to see how the environment setup comparison.
> [![RavenDB vs PostgreSQL - Part 1: Server setup experience](https://img.youtube.com/vi/ZsEvm4Dl1jI/hqdefault.jpg)
<br> RavenDB vs PostgreSQL - Part 1: Server setup experience](https://youtu.be/ZsEvm4Dl1jI)

#### 3.2 **Start the PostgreSQL service**

After all installation steps and computer restart, you need to start the PostgreSQL service. You can do this by running the following command in your terminal (make sure you run it as an administrator):

```bash
net start postgresql-x64-13
```

Then open de pgAdmin tool and create a new database named `RavenDBDemo` or whatever you prefer. You can use the following SQL command to create the database on the server you configured.

#### 2.2.2 **Setup Entity Framework Core in your project and setup the migrations**

Install the Entity Framework Core tools in your project to manage migrations and database updates. You can do this by running the following command in your terminal:

```bash
dotnet tool install --global dotnet-ef
```

#### 2.2.3 **Connect to your database**

Open the Development AppSettings file `LucasRT.RavenDB.Demo.RestAPI/appsettings.development.json` and set the `ConnectionStrings` node with the information ou set up on PostgreSQL:

```json
"ConnectionStrings": {
  "PostgreSQL": "Host=localhost;Port=5432;Database=RavenDBDemo;Username=postgres;Password=admin;"
}
```

#### 2.2.4 **Setting up database environment for the application**

Run the following command to create the initial migration that will create the tables of for the Entities in your Domain layer:

```bash	
 dotnet ef migrations add InitialMigration -p LucasRT.RavenDB.Demo.Application
 dotnet ef database update -p LucasRT.RavenDB.Demo.Application
```

### 4. **Run the application**
Use the following command to run the application or your IDE's run command:

```bash
dotnet run
```

Swagger must be showing up at `https://localhost:5001/swagger/index.html` or `http://localhost:5000/swagger/index.html` depending on your configuration.

> The database scope is informed in Swagger Title, so you can easily identify which database is being used.

## **Overall comparison**

The experience I had developing the services is listed below. And this can seems like a small diference in a short example, but if you multiply this into hundreds of relational entities and resource services, it might create a huge number of risks to your application when you use a typical relational database that depends on migrations and manual work sync between you application domain and your database. In resume, on RavenDB, you simply don't have to worry about it at all, focus on the integrity of your entity domain classes and the Database will reflect it to you.

### 1. **Developer experience**
Does the developer experience counts? *"They're technical, they enjoy complex things. What's the difference to him to setup the environment?"*. Well, human effort costs by minute. If you have one team member, this could be irrelevant, but if you have several teams with a good amout of microservices... it can make a team cost difference.

RavenDB was designed to be simple to setup and use as you could see on video comparison, starting from service installation and initialization. In the application, the setup keeps multiple times faster than any other database to setup and start saving and reading data. And setting up the environment when you're hurry is teasing to everyone.

  - **RavenDB:** Fast, simple and no extra installations needed.
     > It's complete in a level that RavenDB is embedded in the service as a web application that runs with the server. You don't have to install a SGBD or other tools to access it. You can access and watch or maintain your database from your phone on mobile connections if you need or want to, without installations. Strict to the point!
     >
     > Do you want to delete that? `shift+del` in RavenDB folder. That´s it. your machine is clean again.

  - **PosgreSQL:** Classic installation wizard that requires "nnf" technic (Next-Next-Finish) that after the installation, you'll be guided to other three or four embedded wizards to install a list o things that starts automatically in your machine that will not be quick to remove if you give up.
    > To access your database you'll need to use console to maintain it, or an SGBD that use to be heavy and full of unecessary features, or you'll have to run a thirdy party limitated web interface SGBD.
    >
    > Now prepare yourself if you want to uninstall everything.

*Of course there is docker for both cases that simplifies that, but come on, we are comparing root structures. Docker is already something extra to configure, but to access data in docker, you have the build in RavenDB Studio Web or to install a SGBD to access your docker hosted service. RavenDB still wins.*

### 2. **Data structure difference**
  - **RavenDB:** Resources entities will become a main Collection, with Valued Objectcs inside the same data scope.
     > One single element in database to represent a single resource.
     ```mermaid
    classDiagram
    class Guest~Collection~{
        string Id
        string Name
        string Email
        string Nationality
        List~Address~ SocialNetworks
    }
    class SocialNetwork~Valued Object~{
        string Name
        string Url
    }
    Guest <|-- SocialNetwork
    ```

  - **PosgreSQL:** Resources entities will become a combination of tables that relates to each Valued Objectcs linked by Ids.
    > Several elements in database linked to each other to represent a single resource.
    ```mermaid
    erDiagram
        GUEST ||--o{ SOCIAL_NETWORKS : has
        GUEST {
            string Id
            string Name
            string Email
            string Nationality
        }
        SOCIAL_NETWORKS {
            string Id
            string GuestId
            string Name
            string Url
        }
    ```
    Basically, if your business demands 10 entities to represent your data, and each entity has 3 valued objects (address, prices, etc) inside it and one of the valued object has other 2 valued objects. You'll have differences between database complexities. Entity framework can try to automate that, but the complexity will remain existing and you will still need to keep migrations updated. *On RavenDB you can just update your class and you're free do go live.*
    - **RavenDB:** 10 collections to maintain
    - **PostgreSQL:** *10 x 3 x 2 =* **60** tables to maintain

> From Leλd∫oft, I've created a AsValidCollection that inherits CollectionsBase, both to optimize some base functionalities focused on entity class, including Ids, enabled, created and updated dates, and model validation features to provide a simple way to create collections that will be used in the application. It may include some Counters and Time Series structures based on RavenDB context. This is a great way to simplify the code and make it more readable, while still maintaining the flexibility of the data model.
> If the model is not properly filled, you'll be able to see the dynamic validation model into the class.
> Available in [NuGet Gallery | LeadSoft.Common.GlobalDomain](https://www.nuget.org/packages/LeadSoft.Common.GlobalDomain).

> Checkout the video below to see the data structure comparison in action, live in TDC SP in 2024 with RavenDB: "TDC SP 2024 - NoSQL: A small step in modeling, a big leap in efficiency RavenDB & LeadSoft"
> [![TDC SP 2024 - NoSQL: Um pequeno passo na modelagem um grande salto na eficiência RavenDB & LeadSoft](https://img.youtube.com/vi/shoX67MMtLc/hqdefault.jpg)
<br> TDC SP 2024 - NoSQL: Um pequeno passo na modelagem um grande salto na eficiência RavenDB & LeadSoft](https://www.youtube.com/watch?v=shoX67MMtLc)

### 3. **Data validation difference**
  - **RavenDB:** Using AsValidCollection as entity inheritance, I've created some interesting fields validations that will respect DataAnnotation settings in the main class, object valued classes and properties to create a validation that will store dinamically in you class, validation errors and status to your model if necessary. If you change the rules, it won't affect data in database and the validation will be just updated when you load the document.
    - You'll never have to worry about data truncation because you moved your rule from 255 character limit in your front/backend to 511 and didn't update your column size.
     > If data does not fit the rules, you can still save it in your database and flag it to avoid data loss if you want to, or you can throw an exception if you wan't. But if you keep this into your database, you can recover that to fix the data safely in the future or create a report about it.

  - **PosgreSQL:** The restrictions and rules in DataAnnotation will create a database model based on that. So a property column set to 255 character limit, will neet migration modification if you want to change your class size. This can be seen as good if you look into the perspective: *It will be impossible to save invalid data in my database.*. But, should it be a database responsability or a service layer responsability? Needs changes along the years... If you need to force a reduce in your data model, you'll have data truncation in existant data in your database and lose information.

    ⚠️ That's DANGEROUS!
 
     > If data does not fit the rules, if will not be saved in a relational database until you fix it. Imagine that in a huge cascade object and on the last node, you have an inconsistent field? You'd have to perform a rollback in all modifications because a single column failed to store your data. What a waste of effort on database, your application should avoid that in business rules, before sending it to database, don't you think?

Why would we chose a strict data model in a evolutive business? It seems more logical and pratical to create a strong backend code that treat data before saving and if you want to save it as flagged, you can do that to a logical repair in other moment.

### 4. **Project setup and code structure to support the database**

Here I'll provide a project dependency and class structures to compare what does you application need to run the same features with both databases to reach similar architectural results.

#### 4.1. **Packages required**
- **RavenDB**
	- [NuGet Gallery | RavenDB.Client 7.1.1](https://www.nuget.org/packages/RavenDB.Client)
	- [NuGet Gallery | RavenDB.DependencyInjection 5.0.1](https://www.nuget.org/packages/RavenDB.DependencyInjection)

- **PostgreSQL + Entity Framework**
	- [NuGet Gallery | EFCore.BulkExtensions 9.0.1](https://www.nuget.org/packages/EFCore.BulkExtensions)
	- [NuGet Gallery | Microsoft.EntityFrameworkCore 9.0.7](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
	- [NuGet Gallery | Microsoft.EntityFrameworkCore.Design 9.0.7](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
	- [NuGet Gallery | Microsoft.EntityFrameworkCore.Relational 9.0.7](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational)
	- [NuGet Gallery | Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL)

#### 4.2. **Startup settings**

- **RavenDB**
    While using the `RavenDB.DependencyInjection` package, you can easily configure the RavenDB client in your `Program.cs` file. The code below shows how to set up the RavenDB client with a certificate and the database name.
    Then you can inject the `IDocumentStore` into your services and repositories to access the database, you're already ready to go in tons of funcionalities and good to go.

- **PostgreSQL**
    You need to configure the Entity Framework Core in your `Program.cs` file, and you have to create a DbContext class that will represent your database context. The code below shows how to set up the DbContext with the connection string and the database name.
    After that, you must set up when the migrations will be applied, from the CI/CD pipeline or from the application startup.
    Then you can inject the DbContext into your services and repositories to access the database, but you have to be careful with migrations and schema changes. Add manually each entity to the DbContext for mapping it to the database.
    
    Remember that you'll have to configure properly several kinds of options to each table, like some specific field types, relationships, enumerators behaviour, etc. This can be a nightmare if you have several entities and relations to maintain. Again, imagine the merge process in auto generated classes...

#### 4.3. **Data model update**

- **RavenDB**
  - The data model is updated automatically when you change your classes. All you have to do is to create the properties inside the class and RavenDB will handle the rest for you.
  - Another interesting thing is that you can transform a `Valued Object` into a collection/resource in the future, if you need to, just add Id field and store it into RavenDB.
  - You can create new properties on the fly and save them without worrying about migrations or schema changes. Easy peasy to merge branches and go live with new features.

- **PostgreSQL**
  - Any change in the data model requires a migration to be created and applied to the database. Once you run the migration, the database will be updated with the new structure, and there are some effort to undo migrations if you need to revert changes.
  - It's always a big risk to change the data model in a relational database, because you might lose data if you don't handle it properly. You have to be careful with migrations and ensure that the data is consistent after the changes.
  - Merge between branches can be a nightmare if you have migrations that are not compatible with each other. You might end up with conflicts that are hard to resolve, and you have to be careful not to lose data in the process.

### 5. **My application is not a CRUD, can I use RavenDB?**

YES! RavenDB is not just for CRUD operations, it is a powerful multimodel ACID database that can handle complex data structures and relationships. You can use it to build applications with advanced features, such as full-text search, semantic search, and AI integration.
YES, a NoSQL database that can be the single and main database for your application. RavenDB is designed to be flexible and adaptable, so you can use it for a wide range of applications, from simple CRUD operations to complex data processing and analysis.

It is a great choice for applications that require high performance, scalability, and ease of use.

### 6. **My application is already built with PostgreSQL, can I migrate to RavenDB?**

Yes, you can migrate your application from PostgreSQL to RavenDB. The process involves a few steps:
From RavenDB Studio, select your database and go to `Tasks > Import Data` menu and click in `From SQL` tab. Select your previous database connector, fill the connection string and as a **touch of magic**, you'll be able to import your data from PostgreSQL to RavenDB.
Want to know more? It will convert the relational tables and suggest document structure to reduce the amount of collections and keep related data into a single object. THIS IS REALLY AMAZING!

Can you imagine the time you would spend to create a migration script to convert your data from PostgreSQL to RavenDB? And the risk of losing data in the process? RavenDB makes it easy to import data from SQL databases, so you can focus on building your application without worrying about data migration.
And as a bonus, it will create the indexes for you, so you don't have to worry about performance issues when querying your data.

Not satisfied? You can export the classes files from RavenDB Studio and use them in your application to create the data model. This way, you can ensure that your application is using the same data structure as the database, and you can avoid issues with data consistency and integrity.

Yes, we can call it a Plug & Play database.

> [Check this documentation](https://ravendb.net/docs/article-page/7.1/csharp/client-api/importing-data-from-sql).
 
## **Final considerations**

RavenDB is the most amazing database I've ever worked with. Everytime I see that an application is using a relational database, I feel like I'm missing the opportunity of creating a more simple and efficient solution to the business.

I can fell my spine hurting when I see that I'll have to set a bunch of things just to compile and run the application, and then I'll have to maintain a lot of migrations and schema changes to keep the database in sync with the application code.
This is a nightmare that I don't want to deal with anymore.

I hope this repository can help you to see the power of RavenDB and how it can simplify your application development process.

## **RavenDB License**

It all depends on how or where you want to host the database, the most recommended is to use the [RavenDB Cloud](https://ravendb.net/cloud) that is a fully managed service that will take care of everything for you, so you can focus on building your application without worrying about database management.
You can chose where it will be hosted, on AWS, Google Cloud or Azure, for example, with several replication ypes and regions to choose from.

Before looking to the license price itself, I recommend you to calculate the cost of your team time to maintain a relational database, the cost of the infrastructure to run it, and the cost of the cloud services to host and secure it. You might be surprised by how much you can save by using RavenDB, even if you compare directly the database cost.

You're probably thinking: *"But I can use PostgreSQL for free, why would I pay for a database?"*.
You're not buying a database, you're buying a solution that will make your life easier, your application more efficient, and your team more productive.
Less risks, less time wasted, and more focus on delivering value to your business.

> If you want to know more about the [RavenDB License](https://cloud.ravendb.net/pricing?config=eyJpb3BzIjozMDAwLCJjbG91ZFByb3ZpZGVyIjoiQXdzIiwiYXdzUmVnaW9uIjoidXMtZWFzdC0xIiwiYXp1cmVSZWdpb24iOiJlYXN0dXMiLCJnY3BSZWdpb24iOiJ1cy1lYXN0MSIsImFkZGl0aW9uYWxOb2Rlc0NvdW50IjowLCJ0aWVyIjoiRnJlZSIsImNvbnRyYWN0VGVybSI6Ik9uRGVtYW5kIiwicGF5bWVudE9wdGlvbiI6Ik1vbnRobHkiLCJjcHVQcmlvcml0eSI6IkJhc2ljIiwiaW5zdGFuY2VUeXBlIjoiRnJlZSIsInN0b3JhZ2VUeXBlIjoiU3NkU3RhbmRhcmQiLCJzdG9yYWdlU2l6ZSI6MTAsInRocm91Z2hwdXQiOjEyNSwicHJvZHVjdFR5cGVJZCI6IkF3cy9GcmVlL3VzLWVhc3QtMSJ9) on they're website dynamic calculator.
> You'll be impressed about how much you can DO using the entrance level plan. Scale is a consequence.

Worried about peak times? RavenDB has a great solution for that, you can scale up and down your database resources as needed, so you can handle peak times without worrying about performance issues or downtime.

## **Pending improvements**

This repository can be improved with time. I just did it for initial tests purpose.

## **Contact**

If you have any questions, suggestions or just want to chat about this repository, feel free to reach out to me:

[![Lucas Tavares](https://media.licdn.com/dms/image/v2/D4D03AQHYR5rdsE7k8A/profile-displayphoto-shrink_800_800/B4DZar23PXGwAg-/0/1746640008554?e=1757548800&v=beta&t=CNznOBNIDpYpe6UnAddZrAlIeQ4Y2JIdZ4B-twJW6Ng)
](https://www.linkedin.com/in/lucasrtavares/) 


### Lucas Tavares

**Born:** 1991, Minas Gerais, Brazil

**Education:** Bachelor's Degree in Information Systems from Cotemig (Minas Gerais, Brazil)

**Location:** Curitiba, Brazil

**Contact:** [LinkedIn](https://www.linkedin.com/in/lucasrtavares/) | [GitHub](https://github.com/leadsoftlucas)

---

#### Professional Summary

Experienced technology professional working in the field since 2007, Lucas has held various developer roles in companies spanning sectors from ERP systems to payroll credit banking.

He founded **Leλd∫oft** in 2018, aiming to optimize results and positively impact people's lives through high-quality software solutions for visionary businesses.

A truly **RavenDB** enthusiast since the first usage.

---

#### Vision

Lucas believes strongly in agile, flexible software development focused on delivering tangible business value and real-world impact. He is an official partner ans enthusiast of **RavenDB** in Brazil.

---

#### Leadership

A pragmatic leader, assertive communicator, and result-oriented professional, Lucas leads teams using agile and innovative methodologies. His approach fosters collaborative environments that consistently deliver strategic business outcomes.

---

#### Purpose

Lucas specializes in transforming complex ideas into practical, actionable solutions, driving business growth with intelligent technology.

---

#### Technical Expertise

Lucas is an expert in high-performance backend development, Web APIs using **.NET C#**, and databases using **RavenDB**. He is proficient in implementing **Hexagonal Architecture** and **Domain-Driven Design (DDD)**, enhancing software scalability, maintainability, and clarity.

Specializing in data modeling DDD (Domain-Driven Design) orientated, he designs solutions with robust data acquisition capabilities, optimal performance, and significant information generation.

---

#### Core Competencies

- Agile Software Development
- Backend Engineering
- High-Performance Web APIs
- RavenDB Implementation
- Domain-Driven Design (DDD)
- Hexagonal Architecture
- Data Modeling & Performance Optimization
- Team Leadership & Communication

---

#### Career Highlights

- Founder and CEO of LeadSoft (2018 - Present)
- Official Partner of RavenDB in Brazil
- Extensive background in ERP systems and financial software solutions
- Strong advocate for agile methodologies and innovative team management strategies

> Thank you for your time!