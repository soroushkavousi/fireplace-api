# Id Generation and Encoding

How to generate and encode an id into a fixed 11-length string of characters:

 <br/> 
 
- ***Example:***

|Id| Encoded Id  |
|--|--|
| 1034467521726252594 | 9NJCx7XUMmo |


  <br/>   <br/>

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

 <br/> <br/>
 
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

[Check the **IdGenerator** class](Core/Tools/IdGenerator.cs)
