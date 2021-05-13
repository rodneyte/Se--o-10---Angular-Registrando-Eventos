using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersist
    {
        Task<Palestrante[]>GetAllPalestrantesByNomeAsync(string nome,bool incluirEventos);
        Task<Palestrante[]>GetAllPalestrantesByAsync(bool incluirEventos);
        Task<Palestrante>GetAllPalestranteByIdAsync(int palestranteId,bool incluirEventos);
    }
}