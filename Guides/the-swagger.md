
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
 
 
### *Special Features*:

1. Log in with google ([Guides/swagger-google-login-button.md](Guides/swagger-google-login-button.md))
2. Full examples of possible responses

<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/swagger-log-in-with-google.png" width="30%" />
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


### How to inject 'Log in with google' button inside Swagger UI



<br/>
<div align="center">
  <img src="https://files.fireplace.bitiano.com/api/swagger-log-in-with-google.png" width="30%" />
</div>

<br/>

This was possible by ***injecting a CSS file*** to Swagger UI:
```csharp
app.UseSwaggerUI(options =>
{
	options.InjectStylesheet("/swagger-ui/custom-swagger-ui.css");
});
```
[See the **custom-swagger-ui.css** file](Api/wwwroot/swagger-ui/custom-swagger-ui.css)

and the HTML which is added to the ***swagger description*** section:

```csharp
description_html += $@"
 <a id=""google-btn"" target=""_blank"" href=""{Configs.Instance.Api.BaseUrlPath}/v0.1/users/open-google-log-in-page"">
     <div id=""google-icon-wrapper"">
         <img id=""google-icon"" src=""https://upload.wikimedia.org/wikipedia/commons/5/53/Google_%22G%22_Logo.svg""/>
     </div>
     <p id=""btn-text""><b>Log in with Google</b></p>
 </a>
 ";
```