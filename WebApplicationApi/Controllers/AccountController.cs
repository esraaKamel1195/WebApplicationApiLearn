using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplicationApi.DTO;
using WebApplicationApi.Models;

namespace WebApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration config;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config
        ) {
            this.userManager = userManager;
            //_signInManager = signInManager;
            this.config = config;
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Get()
        {
            return Ok("AccountController is working!");
        }

        [HttpGet]
        [Route("status")]
        public IActionResult Status()
        {
            return Ok("AccountController status is OK!");
        }

        [HttpGet]
        [Route("info")]
        public IActionResult Info()
        {
            return Ok("AccountController info endpoint.");
        }

        [HttpPost]
        [Route("createAccount")]
        public IActionResult CreateAccount([FromBody] object accountData)
        {
            if (accountData == null)
            {
                return BadRequest("Invalid account data.");
            }
            // Logic to create account would go here
            return Created("", accountData);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            // Logic to delete account would go here
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAccount(int id, [FromBody] object updatedAccountData)
        {
            if (updatedAccountData == null)
            {
                return BadRequest("Invalid account data.");
            }
            // Logic to update account would go here
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registrationData)
        {
            /*if (registrationData == null)
            {
                return BadRequest("Invalid registration data.");
            }
            // Logic to register user would go here
            return Created("", registrationData);*/

            if (ModelState.IsValid || registrationData != null)
            {
                ApplicationUser newUser = new ApplicationUser
                {
                    UserName = registrationData.UserName,
                    Email = registrationData.Email,
                    //Password = registrationData.Password
                };
                // create account
                IdentityResult user = await userManager.CreateAsync(newUser, registrationData.Password);
                if (user.Succeeded)
                {
                    return Created("", registrationData);
                }
                // Logic to register user would go here

                foreach (IdentityError error in user.Errors) {
                    ModelState.AddModelError("AccountError", error.Description);
                }
                return BadRequest(ModelState);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("login")]
        //[Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto loginData)
        {
            /*if (loginData == null)
            {
                return BadRequest("Invalid login data.");
            }
            // Logic to authenticate user would go here
            return Ok("Login successful.");*/

            if (ModelState.IsValid || loginData != null)
            {
                ApplicationUser user = await userManager.FindByNameAsync(loginData.UserName);
                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                } else if (!await userManager.CheckPasswordAsync(user, loginData.Password))
                {
                    return Unauthorized("Invalid username or password.");
                } else
                {
                    #region generateJWTtoken
                    /*var tokenHandler = new System.IdentityModel.Tokens.JWT.JwtSecurityTokenHandler();
                    var key = System.Text.Encoding.ASCII.GetBytes(config["JWT:Key"]);
                    var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                    {
                        Subject = new System.Security.Claims.ClaimsIdentity(new[]
                        {
                            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName),
                            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                    };*/
                    //var token = tokenHandler.CreateToken(tokenDescriptor);
                    //var jwtToken = tokenHandler.WriteToken(token);
                    //Response.Headers.Add("Authorization", "Bearer " + jwtToken);
                    // return token
                    //return Ok(new { Token = jwtToken });
                    #endregion

                    string jit = Guid.NewGuid().ToString();

                    IList<string> roles = await userManager.GetRolesAsync(user);

                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, jit)
                    };

                    if (roles != null)
                    {
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                    }

                    #region createToken with JwtSecurityToken
                    //used on create signing credentials object 
                    //SymmetricSecurityKey signInkey = new(Encoding.UTF8.GetBytes(config["JWT:key"]));

                    //SigningCredentials signInCredential = new(signInkey, SecurityAlgorithms.HmacSha256); 

                    var createToken = new JwtSecurityToken(
                        //issuer: "http://localhost:50691/", // provider the owner of API
                        issuer: config["JWT:Issuer"],
                        //audience: "http://localhost:4200", // the client that will use the API
                        audience: config["JWT: Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                            new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                                System.Text.Encoding.UTF8.GetBytes(config["JWT:Key"] ?? "ddfdfdfdfdrrrere7878dfdfdfdfdddfd")
                            ),
                            Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
                    );
                    #endregion

                    #region
                    /*var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                        issuer: config["Jwt:Issuer"],
                        audience: config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                            new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                            Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256)
                    );*/
                    #endregion

                    var jwtToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(createToken);

                    return Ok(new
                    {
                        Token = jwtToken,
                        Expiration = createToken.ValidTo
                    });
                }
            }
            else { return BadRequest(ModelState); }
        }
    }
}
