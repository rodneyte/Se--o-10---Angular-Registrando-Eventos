using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<Evento>AddEvento(Evento model);
        Task<Evento>UpdateEvento(int eventoId, Evento model);
        Task<bool>DeleteEvento(int eventoId);
        Task<Evento[]>GetAllEventoAsync(bool incluirPalestrantes=false);
        Task<Evento[]>GetAllEventosByTemaAsync(string tema,bool incluirPalestrantes=false);
        Task<Evento>GetEventoByIdAsync(int eventoId,bool incluirPalestrantes=false);
    }
}