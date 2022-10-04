
# Log in with google inside Swagger UI

I did customize the swagger UI to have a `log-in-with-google` button. 
how? [guides/swagger-google-login-button.md](guides/swagger-google-login-button.md)

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