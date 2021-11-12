using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestProjectLegioSoft.ControllerModels;


namespace TestProjectLegioSoft.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public UserController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Registers the user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST User/Register
        /// {
        ///     email: "ssss@gmail.com"
        ///     password: "12345"
        ///     passwordconfirm: "12345"
        /// }
        /// </remarks>
        /// <param name="model">The register model (RegisterControllerModel object)</param>
        /// <returns>Response, that user has successfully registered</returns>
        /// <response code ="200">Success</response>
        /// <response code = "500">If the user already registered or passwords don`t match</response>
        [HttpPost("/Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterControllerModel model)  
        {  
            var userExists = await _userManager.FindByNameAsync(model.Email);  
            if (userExists != null)  
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });  
  
            IdentityUser user = new IdentityUser()  
            {  
                Email = model.Email,  
                SecurityStamp = Guid.NewGuid().ToString(),  
                UserName = model.Email,
            };  
            var result = await _userManager.CreateAsync(user, model.Password);  
            if (!result.Succeeded)  
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });  
  
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });  
        }  
        
        /// <summary>
        /// Logins the user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST User/Login
        /// {
        ///     email: "ssss@gmail.com"
        ///     password: "12345"
        /// }
        /// </remarks>
        /// <param name="model">LoginControllerModel (object)</param>
        /// <returns>JWT token to login</returns>
        /// <response code ="200">Success</response>
        /// <response code = "401">If the user is unauthorized</response>
        [HttpPost("/Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginControllerModel model)  
        {  
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();
            var userRoles = await _userManager.GetRolesAsync(user);  
  
            var authClaims = new List<Claim>  
            {  
                new Claim(ClaimTypes.Name, user.UserName),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
            };
            
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));  
  
            var token = new JwtSecurityToken(  
                issuer: _configuration["JWT:ValidIssuer"],  
                audience: _configuration["JWT:ValidAudience"],  
                expires: DateTime.Now.AddHours(3),  
                claims: authClaims,  
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)  
            );  
  
            return Ok(new  
            {  
                token = new JwtSecurityTokenHandler().WriteToken(token),  
                expiration = token.ValidTo  
            });
        }

    }
}