# Welcome to Fireplace API

Fireplace API is a **Reddit API clone** that has communities, posts, and nested comments. This project is just an individual effort to create a real-world Web API sample with some advanced features.

The API is developed via ASP&#46;NET Core framework and has many features such as DDD structure, Swagger UI, Integration Testing, various Sign-Up Methods, Error Handling, Logging System, Security Features, CICD, and Docker Deployment. <br/>

Stack: REST API, GraphQL, ASP&#46;NET Core (7.0), PostgreSQL, Swagger, Nginx, CICD, Docker, GitHub Actions, Google OAuth 2.0, Gmail API, NLog, xUnit <br/>

<br/>

<div align="center">
    <a href="https://api.fireplace.bitiano.com/swagger" target="_blank">
        <img src="https://files.fireplace.bitiano.com/api/swagger-top.png" width="85%" />
        <p style="margin-top: 5px">The Swagger UI</p>
    </a>
</div>

<br/>

<div align="center">
    <a href="https://api.fireplace.bitiano.com/graphql" target="_blank">
        <img src="https://files.fireplace.bitiano.com/api/graphql-playground.png" width="85%" />
        <p style="margin-top: 5px">The GraphQL Playground</p>
    </a>
</div>

<br/>
<br/>

\- Following Sections:

- [**The DDD Architecture**](#the-ddd-architecture)
- [**Summary**](#summary)
- [**The Swagger**](#the-swagger)
- [**How to run a clone**](#how-to-run-a-clone)

 <br/>
 <br/>

# The DDD Architecture

This project has been divided into multiple subprojects to implement a domain-driven design structure:

- Application
- Domain
- Infrastructure
- Integration Tests

<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/the-architecture.png" />
</div>

### Layers:

_According to Eric Evans's book [Domain Driven Design](https://domainlanguage.com/ddd/)_

> The DDD patterns help you understand the complexity of the domain.

- #### The Application Layer

  - Defines the jobs the software is supposed to do
  - It interacts with the application layers of other systems
  - It does not contain business rules or knowledge
  - Must not directly depend on any infrastructure framework.

- #### The Domain Layer

  - The heart of business software
  - Responsible for representing concepts of the business, information about the business situation, and business rules.
  - Must completely ignore data persistence details.

- #### The Infrastructure Layer
  - Defines how the data is persisted in databases or other persistent storage
  - Responsible for connecting to external systems and services
  - It does not contain business rules or knowledge

<br/> <br/>

# Summary

**1. The DDD Architecture**

- Layers as subprojects: Application, Domain, Infrastructure
- API: Handles the client requests
- Domain: Handles business rules & logic
- Infrastructure: Handles data persistence & third-party systems

**2. The Swagger**

- Check [The Swagger UI](https://api.fireplace.bitiano.com/swagger)
- Has the list of API actions to view or run
- Has examples of possible responses for each action
- Supports Bearer Authentication
- Has a custom Google Sign-In button
- Automatically save cookies after login
- Automatically use the CSRF token
- Has customizations with [custom-swagger-ui.css](Application/wwwroot/swagger-ui/custom-swagger-ui.css) & [custom-swagger-ui.js](Application/wwwroot/swagger-ui/custom-swagger-ui.js)

**3. GraphQL**

- Check [The GraphQL Playground](https://api.fireplace.bitiano.com/graphql), which is interactive and graphical IDE for GraphQL
- With GraphQL you can prevent over-fetching and under-fetching
- With GraphQL you can call multiple API actions with just a request
- With GraphQL playground you can check all the schema you can call
- Supports nested data such as community posts and post comments
- Supports self data such as created posts, created comments, and joined communities


**4. CICD with GitHub Action**

- Check [The CICD.yml](.github/workflows/CICD.yml)
- Has three steps: Test, Build, and Deploy
- At the test stage, integration tests will be run.
- At the build stage, a docker image will be created and pushed to Docker Hub
- At the deploy stage, the image will be run in a VPS with docker-compose

**5. Docker & Docker Compose**

- Check [The Dockerfile](Dockerfile) (For building)
- Check [The docker-compose.yml](docker-compose.yml) (For deploying)

**6. Integration Testing**

- With [xUnit](https://xunit.net/)
- Parallelism for each test class
- Each test class has its own database clone
- Each test function is independent

**7. Database**

- With [PostgreSQL](https://www.postgresql.org/)
- Has a repository for each entity
- Use Fluent API
- Use ulong Id
- Use case-insensitive collation
- Each entity inherits from a base class that has Id, CreationDate, and ModifiedDate

**8. Pagination vs Query Result**

- After implementing both stateful & stateless paginations, I decided to remove them!
  - Stateful paginations need a considerable amount of server resources to be run!
  - Stateless paginations can produce duplicate data and even miss some data of different sorts!
- Use Query Result instead of Pagination
  - The idea is to send a list of the remaining ids for each query
  - Query result is a collection of items and a list of more item ids
  - Also nested comments have child comments and a list of more child comment ids

**9. Supports nested comments**

- Each comment has a field for its replied comments
- Can get post comments along with their replies for a specific sort with the structure

**10. Prevents CSRF attacks**

- The token will be refreshed for every unsafe HTTP method call
- The token should be equal in both cookies and headers

**11. Error Handling**

- The error json has a code, a type, a field, and a message
- Error code is unique to a pair of error type and error field
- Error types are ALREADY_EXISTS, INVALID_FORMAT, AUTHENTICATION_FAILED, and etc
- Error field refers to the item that caused the error
- Error client messages are stored in the database
- Error codes can be changed freely in the database without changing the production codes
- Exception middleware catches ApiException, and the other Exceptions occured in the code
- Server Message is for the logging, and Client Message is for the users

**12. Has logging system**

- Use [NLog](https://nlog-project.org/)
- Check [The nlog.config](Application/Tools/NLog/nlog.config)
- Check [The LogExtensions.cs](Domain/Extensions/LogExtensions.cs)
- Has Sensitive Data Masking
- The directory of the log files can be specified in the environment variables
- Each log line has Request Info, Process Info, Code Location, Log Time, Execution Time, Parameters Involved, Title, Message, and Exception
- Each part of a log line has its padding and reserved space for readability

**13. Sign up and Log in methods**

- Use Bearer Scheme
- Sign Up: Google Sign-in, Email
- Log In: Google Sign-in, Email, Username
- Can specify the access token in both cookies and headers
- Use [Gmail API](https://developers.google.com/gmail/api) for sending emails

**14. Request Tracing & API Request Rate Limit**

- Request details will be stored in the database
  - Method, Action, URL, IP, User Agent, User ID, Duration, Status Code, Error Name, Request Time, …
- Currently, only allow a few API calls per one hour

**15. Supports user sessions**

- Can get the list of sessions and revoke them
- Currently, sessions only have IP, but I should also add the user-agent and the country of the IP

**16. Id Generation and Encoding**

- Check [Guides/id-generation-and-encoding.md](Guides/id-generation-and-encoding.md)

<br/> <br/>

# The Swagger

With the **_swagger UI_**, you can easily interact with the API and learn it. It shows all routes, inputs, outputs, models, and errors. It also generates a _[swagger.json](https://api.fireplace.bitiano.com/swagger/v1/swagger.json)_, which describes the schema of the API that can be imported into your app.

[Check the **Swagger UI** website](https://api.fireplace.bitiano.com/swagger)

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
  <img src="https://files.fireplace.bitiano.com/api/comment-routes.png" width="85%" />
</div>

 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/response-list-communities.png" width="85%" />
</div>

 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/response-bad-request.png" width="60%" />
</div>

 <br/>
 
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/create-a-post-request.png" width="85%" />
</div>

<br/> <br/>

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

| Environment Key                 |             Value             |    Default    | Required |
| ------------------------------- | :---------------------------: | :-----------: | :------: |
| FIREPLACE_API_CONNECTION_STRING |    &#60;connection-string>    |       -       | &#10004; |
| FIREPLACE_API_LOG_DIRECTORY     |         path/to/logs          | Project Root  | &#10006; |
| ASPNETCORE_ENVIRONMENT          | 'Development' or 'Production' | 'Development' | &#10006; |

<br/>

- **How to set?**
  <br/>

Option 1: Directly in shell

```
Linux:
> export ASPNETCORE_ENVIRONMENT='Development'
> export FIREPLACE_API_LOG_DIRECTORY='path/to/logs'
> export FIREPLACE_API_CONNECTION_STRING='<connection-string>'
```

```
Windows powershell:
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
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "FIREPLACE_API_LOG_DIRECTORY": "path/to/logs",
        "FIREPLACE_API_CONNECTION_STRING": "<connection-string>"
      }
    }
  }
}
```

Copy the json to a file at this path: `Application\Properties\launchSettings.json`

Note: In this project, the file 'launchSettings.json' will not be pushed to the git repository because it is placed in the .gitignore file. You can freely customize envrionment variables in it.

<br/>

**4. Apply the database migrations**

```
> dotnet ef database update --startup-project Application --project Infrastructure
```

<br/>

**5. Run the projects**

```
> dotnet run --project Application
```

Now you can check the swagger:
http://localhost:5000/swagger

Note: At this stage, you may have noticed that some errors say there are no configs or errors in the database! So we are going to fix them.

<br/>

**6. Import the initial data (configs & errors) into the database**

The file [Guides/db-initial-data.txt](Guides/db-initial-data.txt) has multiple insert queries to feed the initial data. You have two ways to inject the data into the database:

Option 1: Using psql command

```
> psql -h localhost -p <port> -U <user> -d <database-name> -f "Guides/db-initial-data.txt"
```

Option 2: Running queries directly

You can easily run queries directly by copying the file content.

<br/>

**7. Test the project**

```
> dotnet test --logger "console;verbosity=detailed"
```

Congratulations, you have almost run a clone.

<br/>

**8. Customize the configs data**

Global variables and sensitive data are stored in a table in the database. You fed sample config data at step 6, but you need to alter it with your project specifications. For instance, generate and set your own google client-id and client-secret to use Google OAuth 2.0.
