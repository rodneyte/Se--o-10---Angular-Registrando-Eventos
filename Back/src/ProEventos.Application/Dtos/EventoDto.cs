using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }   
        public string DataEvento { get; set; }
        [Required(ErrorMessage="O campo {0} é obrigatório."),
        //MinLength(3,ErrorMessage="O campo {0} deve ter no minimo {1} caracteres"),
        //MaxLength(50,ErrorMessage="O campo {0} deve ter no maxíco {1} caracteres")
        StringLength(50,MinimumLength=3,ErrorMessage="Intervalo permitido de 3 a 50 caracteres.")
        ]
        public string Tema { get; set; }

        [Display(Name="Qdt. Pessoas")]
        [Range(1,12000,ErrorMessage="{0} não pode ser menor que {1} e maior que {2}")]
        public int QtdPessoas { get; set; }
        
        [RegularExpression(@".*\.(gif|jpg?g|bmp|png)$",
        ErrorMessage="não é uma imagem valída (gif,jp?g,bmp,png)")]
        public string ImagemURL { get; set; }
        
        [Required(ErrorMessage="O campo {0} é obrigatório.")]
        [Phone(ErrorMessage="O campo {0} está com número inválido.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage="O {0} é obrigatório."),
        Display(Name="e-mail"),
        EmailAddress(ErrorMessage="O campo {0} dever ser um {0} valido.")]
        public string Email { get; set; }
        
        public int UserId { get; set; }
        public UserDto  UserDto { get; set; }
        public IEnumerable<LoteDto> Lotes { get; set; }
        
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        
        public IEnumerable<PalestranteDto> PalestrantesDto { get; set; }
    }
}