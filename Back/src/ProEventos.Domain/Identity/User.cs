using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Domain.Enum;

namespace ProEventos.Domain.Identity
{
    public class User:IdentityUser<int>
    {
       public string PrimeiroNome { get; set; } 
       public string UltimoNome { get; set; }
       public Titulo titulo { get; set; }
       public string Descricao { get; set; }
       public Funcao  funcao { get; set; }
       public string ImagemURL { get; set; }
       public IEnumerable<UserRole> UseRoles { get; set; }

    }
}