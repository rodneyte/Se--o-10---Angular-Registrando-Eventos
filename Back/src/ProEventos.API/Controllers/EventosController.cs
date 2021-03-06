using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;

        public EventosController(IEventoService eventoService, 
        IWebHostEnvironment hostEnvironment,
        IAccountService accountService
        )
        {
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
            _eventoService = eventoService;
        }

         [HttpGet]
         public async Task<IActionResult> Get()
         {
             try
             {

                 var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), true);
                 if (eventos == null) return NoContent();
                 return Ok(eventos);
                 
             }
             catch (Exception ex)
             {
                 return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
             }
         }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(),id, true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }

        }
        [HttpGet("tema/{tema}")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var evento = await _eventoService.GetAllEventosByTemaAsync(User.GetUserId(),tema, true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar temas. Erro: {ex.Message}");
            }

        }
        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImagem(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(),eventoId);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    DeletaImagem(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }

                var EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(),eventoId, evento);

                return Ok(EventoRetorno);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar salvar eventos. Erro: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventoService.AddEvento(User.GetUserId(),model);
                if (evento == null) return BadRequest("Erro ao tentar adicionar o evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar salvar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventoService.UpdateEvento(User.GetUserId(),id, model);
                if (evento == null) return BadRequest("Erro ao tentar adicionar o evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(),id);
                if (evento == null) return NoContent();

                if(await _eventoService.DeleteEvento(User.GetUserId(),id))
                {
                    DeletaImagem(evento.ImagemURL);
                    return Ok(new { message = "Deletado" });
                }
                else
                {
                  throw new Exception("Ocorreu um problema não especicico ao tentar deletar o Evento");  
                } 
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
            }
        }
        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
            .Take(10)
            .ToArray()
            ).Replace(' ','-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";
           
           var imagemPath = Path.Combine(_hostEnvironment.ContentRootPath,@"resources/images",imageName);
        
           using(var fileStrem = new FileStream(imagemPath,FileMode.Create))
           {

                await imageFile.CopyToAsync(fileStrem);
           }
            return imageName;
        }
        [NonAction]
        public void DeletaImagem(string imagemName)
        {
            var imagemPath = Path.Combine(_hostEnvironment.ContentRootPath,@"resources/images",imagemName);

            if(System.IO.File.Exists(imagemPath))
                System.IO.File.Delete(imagemPath);
        }
    }
}
