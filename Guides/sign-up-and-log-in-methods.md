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

If you are interested in the Google OAuth 2.0 implementation, you can check these links:

[Check the **GoogleGateway** class](Infrastructure/Gateways/GoogleGateway.cs)

[Check the **LogInWithGoogleInputQueryParameters** class](Api/Controllers/Parameters/UserParameters/LogInWithGoogleParameters.cs)

[Check the **LogInWithGoogleAsync** method in **UserOperator** Class](Core/Operators/UserOperator.cs#L91)

 <br/>