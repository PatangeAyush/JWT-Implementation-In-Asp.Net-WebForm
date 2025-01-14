using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(API.Startup))]

namespace API
{
    public class Startup
    {
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
    }
}