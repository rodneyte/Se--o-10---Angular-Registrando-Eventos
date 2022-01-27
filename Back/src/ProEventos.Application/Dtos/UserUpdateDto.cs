using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string UserName { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string Email{get;set;}
        public string ProneNumber { get; set; }
        public string Funcao { get; set; }
        public string Descricao { get; set; }
        public string Passoword { get; set; }
        public string Token { get; set; }
    }
}