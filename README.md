# Introduction

**Fireplace API** is a web API project developed with the ASP&#46;NET Core framework. It is like a simple version of Reddit API, which has communities, posts, and comments.

This project is a practice on how to design an API for real-world applications. It also can be used as a sample.

 <br/>
 
Check [**The Swagger UI**](https://api.fireplace.bitiano.com/docs/index.html) of the API

[**How to run a clone?**](#how-to-run-a-clone)

 <br/>

 
# Highlights

1. [The DDD Architecture](#the-ddd-architecture)
2. [The Swagger](#the-swagger)
3. CICD with GitHub Action ([CICD.yml](.github/workflows/CICD.yml))
4. Docker & Docker Compose ([Dockerfile](Dockerfile) & [docker-compose.yml](docker-compose.yml))
5. Supports Integration Testing
6. With PostgreSQL database
7. Supports pagination
8. Supports nested comments
9. Supports user sessions
10. Supports error codes
11. Prevents CSRF attacks
12. Has logging system with [NLog](https://nlog-project.org/)
13. [Sign up and Log in methods](#sign-up-and-log-in-methods)
14. Id Generation and Encoding ([Guides/id-generation-and-encoding.md](Guides/id-generation-and-encoding.md))


 <br/> 


# The DDD Architecture

This project has been divided into multiple subprojects to implement a domain-driven design structure:

- API
- Core
- Infrastructure
- Integration Tests

<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/the-architecture.png" />
</div>

### Layers:

*According to Eric Evans's book [Domain Driven Design](https://domainlanguage.com/ddd/)*

> The DDD patterns help you understand the complexity of the domain.

- #### The API Layer (Application Layer)
	- Defines the jobs the software is supposed to do
	- It interacts with the application layers of other systems
	- It does not contain business rules or knowledge
	- Must not directly depend on any infrastructure framework.

- #### The Core Layer (Domain Model Layer)

	 - The heart of business software 
	 - Responsible for representing concepts of the business, information about the business situation, and business rules. 
	 -  Must completely ignore data persistence details.

- #### The Infrastructure Layer
	- Defines how the data is persisted in databases or other persistent storage
	- Responsible for connecting to external systems and services
	- It does not contain business rules or knowledge

 <br/>


# The Swagger

With the ***swagger UI***, you can easily interact with the API and learn it. It shows all routes, inputs, outputs, models, and errors. It also generates a _[swagger.json](https://api.fireplace.bitiano.com/docs/v0.1/swagger.json)_ which describes the schema of the API that can be imported into your app.

[Check the **Swagger UI** website](https://api.fireplace.bitiano.com/docs/index.html)

 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/swagger-top.png" width="85%" />
</div>

 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/swagger-sample-execution.png" width="85%" />
  <p><i>a sample of an execution</i></p>
</div>

 <br/>  <br/>
 
 

More details? Check the [Guides/the-swagger.md](Guides/the-swagger.md)


 <br/> 


# Sign up and Log in methods


### - Sign up methods

- Google OAuth 2.0 (server-side)
- Email



### - Log in methods

- Google OAuth 2.0 (server-side)
- Email
- Username

<br/>

### - User will get an Access Token

Scheme: ***Bearer***

The access token can be placed at ***cookies*** or ***headers***.

<br/>

### The routes:

<br/>
  
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/various-log-in-sign-up.png" width="85%" />
</div>

<br/><br/>


More details? Check the [Guides/sign-up-and-log-in-methods.md](Guides/sign-up-and-log-in-methods.md)


 <br/>


# How to run a clone

**1. Create an empty PostgreSQL database & get its connection string**

The connection string should be something like this:
```
Host=<server-address>;Port=1234;Username=<username>;Password=<password>;Database=<database-name>;Include Error Detail=true;Log Parameters=true;
```

<br/>

**2. Clone the project**

```
> git clone https://github.com/soroushkavousi/fireplace-api.git
> cd fireplace-api
```

<br/>

**3. Set the two environment variables**

|Environment Name| Environment Value|
|--|--|
| ASPNETCORE_ENVIRONMENT | 'Development' or 'Production' |
| CONNECTION_STRING | &#60;connection-string> |

```
linux: 
> export ASPNETCORE_ENVIRONMENT='Development'
> export CONNECTION_STRING='<connection-string>'
```
```
windows powershell: 
> $env:ASPNETCORE_ENVIRONMENT = 'Development'
> $env:CONNECTION_STRING = '<connection-string>'
```

<br/>

**4. Apply the database migrations**
```
> dotnet ef database update --startup-project Api --project Infrastructure
```

<br/>
   

**5. Run the projects**

```
> dotnet run --project Api
```

Now you can check the docs:
 http://localhost:5000/docs

Note: At this stage, you may have noticed that some errors say there are no configs or errors in the database! So lets fix them.

<br/>

**6. Import the initial data (configs & errors) into the database**   

The file [Guides/initial-data.sql](Guides/initial-data.sql) has multiple insert queries to feed the initial data. You have two ways in order to inject the data to the database:

1. Using psql command

```
> psql -h localhost -p <port> -U <user> -d <database-name> -f "Guides/initial-data.sql"
```

2. Running queries directly

You can easily run queries directly by copying the file content .

<br/>

**Note**: Alter the configs data with your configurations details.

<br/>


**7. Test the project**

```
> dotnet test --logger "console;verbosity=detailed"
```

