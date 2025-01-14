                                                       
							     JWT Over View

1) JWT (JSON Web Token) tokens Are Used Transfer Data from client to server Wise Versa.

2) Ye Session ka Upgraded Version Bol Sakte hai Kyuki Session sirf Ek server ke sath deal karta hai lekin JWT multiple server ke sath kaam kar sakta hai 

3) Session me session ID create hoti hai aur JWT me jwt Token Generate hota hai . Jisme Header,Payload,Signature.

4) Header me JWT ka type , Payload me Claims(Data) hota hai, Signature me kuch Unique hota hai.

						  JWT Implementation in Asp.Net Web Forms
------------------------------
1) First Step Ek StartUp class banao jisme tumhe 

public void Configuration(IAppBuilder app)
{
    ConfigureOAuth(app);
}

public static void ConfigureOAuth(IAppBuilder app)
{
    // Secret key ko bytes me convert karte hain
    var key = Encoding.UTF8.GetBytes("R2z5UdTn9XmKsd3jN9P2QaH4FjWp0uLg1n5W0A5n5Mw");

    // JWT authentication middleware ko setup karte hain
    app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
    {
        TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Issuer ko validate karna
            ValidateAudience = true, // Audience ko validate karna
            ValidateIssuerSigningKey = true, // Signing key validate karna
            ValidIssuer = "YourAppName", // Token banane wala
            ValidAudience = "YourAppUsers", // Token use karne wala
            IssuerSigningKey = new SymmetricSecurityKey(key) // Signing key
        }
    });
}

-------------------------------
2) Apne DAL file me 

private const string SecretKey = "R2z5UdTn9XmKsd3jN9P2QaH4FjWp0uLg1n5W0A5n5Mw";
private const int TokenExpiryMinutes = 30;

 public static string GenerateJwtToken(string username)
 {
     var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

     var claims = new[]
     {
     new Claim(ClaimTypes.Name, username),
     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
 };

     var token = new JwtSecurityToken(
         issuer: "YourAppName",
         audience: "YourAppUsers",
         claims: claims,
         expires: DateTime.Now.AddMinutes(TokenExpiryMinutes),
         signingCredentials: credentials);

     return new JwtSecurityTokenHandler().WriteToken(token);
 }
--------------------------------
3) Apne Controller me Ek IHttpActionResult ka Method Banao jisme
public IHttpActionResult Login(LoginDTO login)
{
    
    if (login.username != null && login.password != null)
    {               
        string token = EmployeeBAL.GenerateJwtToken(login.username);
        
        return Ok(new { Token = token });
    }
    else
    {
        // Invalid credentials ka response
        return Unauthorized();
    }
}

-----------------------------------
4) then Login ke Controller Method ko Chod kar Baki Sabhi Methods par [Authorize] ka Attribute add kardo

