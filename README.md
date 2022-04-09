
# Introduction

***Fireplace*** is a discussion application with communities, posts, and comments, just like Reddit.

This project, ***Fireplace API***, provides an API for Fireplace, and it aims to be a real-world example of web API concepts with the ***`ASP.NET Core`*** framework. I needed to record the knowledge and experience that I have learned in my coding history. As a result, I have created this project for myself and everyone who considers it valuable.


# Architecture

<div align="center">
  <img src="./Static/Images/TheArchitecture.png" />
</div>

### Layers:

*According to Eric Evans's book [Domain Driven Design](https://domainlanguage.com/ddd/)*

> The DDD patterns help you understand the complexity of the domain.

- #### The domain model layer (Core)

	 - The heart of business software 
	 - Responsible for representing concepts of the business, information about the business situation, and business rules. 
	 -  Must completely ignore data persistence details.

- #### The application layer (API)
	- Defines the jobs the software is supposed to do
	- It interacts with the application layers of other systems
	- It does not contain business rules or knowledge
	- Must not directly depend on any infrastructure framework.

- #### The infrastructure layer (Infrastructure)
	- Defines how the data is persisted in databases or other persistent storage
	- Responsible for connecting to external systems and services
	- It does not contain business rules or knowledge