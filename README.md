
 <br/>
 
# Introduction

***Fireplace*** is a discussion application with communities, posts, and comments, just like Reddit.

This project, ***Fireplace API***, provides an API for Fireplace, and it aims to be a real-world example of web API concepts with the ***`ASP.NET Core`*** framework. I needed to record the knowledge and experience that I have learned in my coding history. As a result, I have created this project for myself and everyone who considers it valuable.


# Highlights

1. [Architecture](#architecture)
2. [Id Generation and Encoding](#id-generation-and-encoding)

# Architecture

<div align="center">
  <img src="./Static/Images/TheArchitecture.png" />
</div>

### Layers:

*According to Eric Evans's book [Domain Driven Design](https://domainlanguage.com/ddd/)*

> The DDD patterns help you understand the complexity of the domain.

- #### The Core Layer (Domain Model Layer)

	 - The heart of business software 
	 - Responsible for representing concepts of the business, information about the business situation, and business rules. 
	 -  Must completely ignore data persistence details.

- #### The API Layer (Application Layer)
	- Defines the jobs the software is supposed to do
	- It interacts with the application layers of other systems
	- It does not contain business rules or knowledge
	- Must not directly depend on any infrastructure framework.

- #### The Infrastructure Layer
	- Defines how the data is persisted in databases or other persistent storage
	- Responsible for connecting to external systems and services
	- It does not contain business rules or knowledge

# Id Generation and Encoding

How to generate and encode an id into a fixed 11-length string of characters:

 <br/>

1.  **Generate a random unsigned long number**
	- between min and max

```csharp
ulong _min = (ulong)Math.Pow(2, 10);
ulong _max = ulong.MaxValue - 1;
var id = Utils.GenerateRandomUlongNumber(_min, _max);
```
 <br/>

The `GenerateRandomUlongNumber` method:
```csharp
private static readonly Random _random = new Random();

public static ulong GenerateRandomUlongNumber(ulong min, ulong max)
{
    ulong uRange = (max - min);
    ulong ulongRand;
    do
    {
        byte[] buf = new byte[8];
        _random.NextBytes(buf);
        ulongRand = BitConverter.ToUInt64(buf, 0);
    } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

    return (ulongRand % uRange) + min;
}
```
 <br/>

2.  **Get bytes of the number**
```csharp
var bytes = BitConverter.GetBytes(id);
```

 <br/>

3. **Encode the bytes with a base58 mapper**

```csharp
var encodedId = Base58.Bitcoin.Encode(bytes);
```

- Why `Base58`? 

Just like a bitcoin address:
<div align="center">
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/3/38/Original_source_code_bitcoin-version-0.1.0_file_base58.h.png/799px-Original_source_code_bitcoin-version-0.1.0_file_base58.h.png" />
</div>
Note: the non-alphanumeric characters in base-64 is "+" and "/"

 <br/>
 
4. **I also added some filters**

```csharp
// To filter 10-length encoded ids
 if (id % 256 < 6)
     return false;

// To filter encoded ids which has three same characters in a row
Regex _encodedIdWrongRepetitionRegex = new(@"(\S)\1{2}");
if (_encodedIdWrongRepetitionRegex.IsMatch(encodedId))
    return false;
```
 <br/>

[See the **IdGenerator** class](Core/Tools/IdGenerator.cs)

