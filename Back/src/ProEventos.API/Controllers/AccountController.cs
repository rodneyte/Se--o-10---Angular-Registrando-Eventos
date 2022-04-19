using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Helpers;
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
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

         public AccountController(IAccountService accountService,
                                 ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }
        
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                //var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
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
                    if( await _accountService.UserExists(userDto.UserName))
                    return BadRequest("Usuário já existe");

                   var user =  await _accountService.CreateAccountAsync(userDto);
                   if(user != null){
                        return Ok( new
                        {
                            userName = user.UserName,
                            PrimeiroNome = user.PrimeiroNome,
                            token = _tokenService.CreateToken(user).Result
                        }); 
                    }
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
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.userName);
                
                if(user==null) return Unauthorized("Usuário ou senha inválidos");

                var result = await _accountService.CheckUserPasswordAsync(user,userLogin.passowrd);

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
                
                if(userUpdateDto.UserName != User.GetUserName())
                    return Unauthorized("Usuário Invalido");

                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if(user == null) return Unauthorized("usuário não encontrado");

                var userReturn = _accountService.UpdateAccount(userUpdateDto);
                if (userReturn == null) return  NoContent();

                return Ok (new
                {
                    userName = user.UserName,
                    PrimeroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });

            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar usuário. Erro: {ex.Message}");
                
            }
        }
    }
}