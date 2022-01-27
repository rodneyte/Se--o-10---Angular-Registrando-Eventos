using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accoutService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accoutService,
                                 ITokenService tokenService)
        {
            _accoutService = accoutService;
            _tokenService = tokenService;
        }
        
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                //var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                var userName = User.GetUserName();
                var user = await _accoutService.GetUserByUserNameAsync(userName);
                return Ok(user);

            }
            catch( Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }
       
        [HttpPost("Register")]
        [AllowAnonymous]//permite chamar um metodo sem o toker
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                    if( await _accoutService.UserExists(userDto.UserName))
                    return BadRequest("Usuário já existe");

                   var user =  await _accoutService.CreateAccountAsync(userDto);
                   if(user != null)
                    return  Ok(user); 

                    return BadRequest("Usuário não criado tente novamente mais tarde!");

            }
            catch( Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar registrar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            try
            {

                
                var user = await _accoutService.GetUserByUserNameAsync(userLoginDto.UserName);
                if(user==null) return Unauthorized("Usuário ou senha inválidos");

                var result = await _accoutService.CheckUserPasswordAsync(user,userLoginDto.Passoword);

                if(!result.Succeeded) return Unauthorized();
                return Ok( new
                {
                    userName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });

            }
            catch( Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar realizar login. Erro: {ex.Message}");
            }
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult>UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _accoutService.GetUserByUserNameAsync(User.GetUserName());
                if(user == null) return Unauthorized("usuário não encontrado");

                var userReturn = _accoutService.UpdateAccount(userUpdateDto);
                if (userReturn == null) return  NoContent();

                return Ok(userReturn);

            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar usuário. Erro: {ex.Message}");
                
            }
        }
    }
}