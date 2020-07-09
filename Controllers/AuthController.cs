using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vega.Core.Models;
using Vega.Persistence;

namespace Vega.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly VegaDbContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration config;

        public AuthController(VegaDbContext context, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IConfiguration config)
        {
            this.signInManager = signInManager;
            this.config = config;
            this.userManager = userManager;
            this.context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await signInManager.PasswordSignInAsync(loginViewModel.Email,loginViewModel.Password,true,false);
            if (!result.Succeeded)
            {
                return BadRequest("Please Confirm Account");
            }

            var user = context.Users.SingleOrDefault(user=>user.Email==loginViewModel.Email);


            string key = config.GetSection("JwtConfig").GetSection("secret").Value; //Secret key which will be used later during validation    
            var issuer = "https://localhost:5001";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, user.Id));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("userid", user.Id));
            permClaims.Add(new Claim("username", user.UserName));
            permClaims.Add(new Claim("email", user.Email));
            permClaims.Add(new Claim("name", user.NormalizedUserName));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            // return new { data = jwt_token };  
            // return token;





            // var token = await userManager.GenerateUserTokenAsync(user,"Default","Default");
            return Ok(new { data = jwt_token,expires= DateTime.Now.AddDays(1),user=user });

        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult getUser()
        {
            var identity = User.Identity as ClaimsIdentity;  
            if (identity != null) {  
                IEnumerable < Claim > claims = identity.Claims;  
                var userid = claims.Where(p => p.Type == "userid").FirstOrDefault() ? .Value;  
                var user = context.Users.SingleOrDefault(x=>x.Id==userid);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(new {user=user});

            }  
            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);
            return Ok(result);
        }

    }
}