using FirstAPI.Dtos.User;
using FirstAPI.Enums;
using FirstAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) return NotFound();
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) return NotFound();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var claimList = new List<Claim>()
                    {
                       new Claim(ClaimTypes.Name, user.UserName),
                       new Claim(ClaimTypes.NameIdentifier,user.Id),
                       new Claim("FullName",user.FullName) 
                    };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var item in roles)
            {
                claimList.Add(new Claim("role", item));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["JWT:Audience"],
                Issuer = _configuration["JWT:Issuer"],
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), 
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
   
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            AppUser newUser = new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            result = await _userManager.AddToRoleAsync(newUser, "Admin");
            if (!result.Succeeded) return BadRequest(result?.Errors);

            return StatusCode(StatusCodes.Status200OK);
        }
        [HttpGet("Role")]
        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(RoleEnum)))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
            }
            return StatusCode(StatusCodes.Status200OK);
        }



    }
}
