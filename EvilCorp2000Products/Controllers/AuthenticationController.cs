using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace EvilCorp2000Products.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //verwendet, um auf Konfigurationsdaten zuzugreifen, die in der appsettings.json gespeichert sind
        private readonly IConfiguration _configuration;

        public class AuthenticationRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        public class ProductAPIUser
        {
            public ProductAPIUser(int usertId, string userName, string firstName, string lastName, string country)
            {
                UserId = usertId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                Country = country;
            }

            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Country { get; set; }
        }


        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authRequestBody)
        {
            //Validate username/pw
            var user = ValidateUserCredentialsMockUser(authRequestBody.Username, authRequestBody.Password);
            //wenn null, dann 401
            if (user == null)
            { 
                return Unauthorized();
            }

            //wenn secret for key == null dann 401
            if (_configuration["Authentication:SecretForKey"] == null)
            {
                return Unauthorized();
            }

            //create a key from secret for key
            var key = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
            
            //create signing credentials with key --> Header
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //get claims for token from userdata --> Payload
            var claimsForToken = new List<Claim>();

            //sub: standardized key for the unique user identifier
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("country", user.Country));

            //create new token
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                credentials);

            //create tokenstring
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            //return ok + token
            return Ok(tokenToReturn);
        }


        private ProductAPIUser ValidateUserCredentialsMockUser(string? username, string? password)
        {
            //normally passed in credentials have to be checked against info in db and check if valid

            return new ProductAPIUser(1, "Evil", "Carmen", "Rabbit", "Exaggernation");
        }
    }

}
