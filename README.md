# Introduction

**Fireplace API** is a web API project developed with the ASP&#46;NET Core framework. It is like a simple version of Reddit API, which has communities, posts, and nested comments.

This project is just a practice on how to design an API for real-world applications. It also can be used as a sample. <br/>

**ASP&#46;NET Core version: 6.0**

Check [**The Swagger UI**](https://api.fireplace.bitiano.com/docs/index.html) of the API

[**How to run a clone?**](#how-to-run-a-clone) 
 
 <br/>


# Highlights

**1. The DDD Architecture**
	
- Layers as subprojects: API, Core, Infrastructure
- API: Web Interface
- Core: Business rules & logic
- Infrastructure: Data persistence & third-party projects
	
**2. The Swagger**

- Check [The Swagger UI](https://api.fireplace.bitiano.com/docs/index.html)
- Has the list of API actions to view or run
- Has examples of possible responses for each action
- Supports Bearer Authentication
- Has a custom Google Sign-In button
- Automatically save cookies after login
- Automatically use CSRF token
- Has customization with [custom-swagger-ui.css](Api/wwwroot/swagger-ui/custom-swagger-ui.css) & [custom-swagger-ui.js](Api/wwwroot/swagger-ui/custom-swagger-ui.js)

**3. CICD with GitHub Action**

- Check [The CICD.yml](.github/workflows/CICD.yml)
- Has three steps: Test, Build, Deploy
- At the build stage, a docker image will be created and pushed to Docker Hub
- At the Deploy stage, the image will be run in a VPS with docker-compose

**4. Docker & Docker Compose** 

- Check [The Dockerfile](Dockerfile) (For building)
- Check [The docker-compose.yml](docker-compose.yml) (For deploying)

**5. Integration Testing**

- With [xUnit](https://xunit.net/)
- Parallelism for each test class
- Each test class has it's own database clone
- Each test function is independent 

**7. Database**

- With [PostgreSQL](https://www.postgresql.org/)
- Has a repository for each entity
- Use Fluent API
- Use ulong Id
- Use case-insensitive collation
- Each entity inherits from a base class which has Id, CreationDate, and ModifiedDate

**8. Pagination vs Query Result**

- After implementing both stateful & stateless paginations, I decided to remove them!
	- Stateful paginations need a considerable amount of server resources!
	- Stateless paginations can produce duplicate data and even miss some data for different sorts! 
- Use Query Result instead of Pagination
	- The idea is to send a list of the remaining ids for each query
	- Query result is a collection of items and more item ids
	- Also nested comments have child comment and more child comment ids
	

**9. Supports nested comments**

- Each comment has a field for its replied comments
- Can get post comments along with their replies for a specific sort with the structure

**10. Prevents CSRF attacks**

- The token will be refreshed for every unsafe HTTP method call
- The token should be equal in both cookies and headers

**11. Error Handling**

- Returning error is a JSON of error code and error client message
- Error client messages are in the database, which can be found by error name (Enum), not error code, because of readability and flexibility
- Error codes can be changed freely in the database without changing the production codes
- Exception middleware catches ApiException, which contains the error name and the error server message
- Server Message is for logging, and Client Message is for users

**12. Has logging system**

- Use [NLog](https://nlog-project.org/)
- Check [The nlog.config](Api/Tools/NLog/nlog.config)
- Check [The LogExtensions.cs](Core/Extensions/LogExtensions.cs)
- Has Sensitive Data Masking
- Log directory path can be specified in the environment variables
- Each log line has Request Info, Process Info, Code Location, Log Time, Execution Time, Parameters Involved, Title, Message, and Exception
- Each part of a log line has its padding and reserved space


**13. Sign up and Log in methods**

 - Use Bearer Scheme
 - Sign Up: Google Sign-in, Email
 - Log In: Google Sign-in, Email, Username
 - Can specify the access token in both cookies and headers
 - Use [Gmail API](https://developers.google.com/gmail/api) for sending emails


**14. Supports user sessions**

- Can get the list of sessions and revoke them
- Currently, sessions only have IP, but I should also add the user-agent and the country of the IP


**15. Id Generation and Encoding**

- Check [Guides/id-generation-and-encoding.md](Guides/id-generation-and-encoding.md)

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

With the ***swagger UI***, you can easily interact with the API and learn it. It shows all routes, inputs, outputs, models, and errors. It also generates a _[swagger.json](https://api.fireplace.bitiano.com/docs/v1/swagger.json)_ which describes the schema of the API that can be imported into your app.

[Check the **Swagger UI** website](https://api.fireplace.bitiano.com/docs/index.html)

 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/swagger-top.png" width="85%" />
</div>

 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/swagger-sample-execution.png" width="85%" />
</div>

 <br/> 
  
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/various-log-in-sign-up.png" width="85%" />
</div>


 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/response-list-communities.png" width="85%" />
</div>

 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/response-bad-request.png" width="60%" />
</div>


<br/>  <br/>



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

**3. Set the environment variables**

<br/>

| Environment Key | Value | Default | Required | 
|--|:--:|:--:|:--:|
| FIREPLACE_API_CONNECTION_STRING| &#60;connection-string> | - |  &#10004;|
| FIREPLACE_API_LOG_DIRECTORY| path/to/logs | Project Root | &#10006;|
| ASPNETCORE_ENVIRONMENT | 'Development' or 'Production' | 'Development'|  &#10006;



<br/>

- **How to set?**
<br/>

Option 1: Directly in shell

```
linux: 
> export ASPNETCORE_ENVIRONMENT='Development'
> export FIREPLACE_API_LOG_DIRECTORY='path/to/logs'
> export FIREPLACE_API_CONNECTION_STRING='<connection-string>'
```

```
windows powershell: 
> $env:ASPNETCORE_ENVIRONMENT = 'Development'
> $env:FIREPLACE_API_LOG_DIRECTORY = 'path/to/logs'
> $env:FIREPLACE_API_CONNECTION_STRING = '<connection-string>'
```

<br/>

Option 2: Via launchSettings.json

```json
{
  "profiles": {
    "FireplaceApi": {
      "commandName": "Project",
      "applicationUrl": "http://localhost:5000",
      "launchBrowser": true,
      "launchUrl": "docs",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "FIREPLACE_API_LOG_DIRECTORY": "path/to/logs",
        "FIREPLACE_API_CONNECTION_STRING": "<connection-string>"
      }
    }
  }
}
```

Copy the json to a file at this path: `Api\Properties\launchSettings.json`

Note: In this project, the file 'launchSettings.json' will not be pushed to the git repository because it is placed in the .gitignore file. You can freely customize envrionment variables in it.

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

Note: At this stage, you may have noticed that some errors say there are no configs or errors in the database! So we are going to fix them.

<br/>

**6. Import the initial data (configs & errors) into the database**   

The file [Guides/db-initial-data.sql](Guides/db-initial-data.sql) has multiple insert queries to feed the initial data. You have two ways in order to inject the data to the database:

Option 1:  Using psql command

```
> psql -h localhost -p <port> -U <user> -d <database-name> -f "Guides/db-initial-data.sql"
```

Option 2: Running queries directly

You can easily run queries directly by copying the file content .


<br/>


**7. Test the project**

```
> dotnet test --logger "console;verbosity=detailed"
```

Congratulations, you have almost run a clone.

<br/>

**8. Customize the configs data**

Global variables and sensitive data are stored in a table in the database. You fed sample config data at step 6, but you need to alter it with your project specifications. For instance, generate and set your own google client-id and client-secret to use Google OAuth 2.0.

