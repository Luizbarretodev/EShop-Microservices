using EShop.AuthApi.DTOs;
using EShop.AuthApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EShop.AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthController(UserManager<IdentityUser> userManager, IJwtTokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper()
            };

            // o CreateAsync faz a criptografia da senha automaticamente
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok("Usuário registrado com sucesso!");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            //verifica se o usuario existe e se a senha bate com a senha criptografada em hash no banco
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest("Email ou senha inválidos");
            }

            // gera o token para o usuário autenticado
            var token = _tokenGenerator.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                User = new { user.Id, user.Email, user.UserName }
            });
        }
    }
}
