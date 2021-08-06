using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersist
    {
        /// <summary>
        /// Método get que retornara uma lista de lotes por eventoid
        /// </summary>
        /// <param name="eventoId">Código chave da tabela eventos</param>
        /// <returns>lista de lotes</returns>
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        /// <summary>
        /// Método get que retornara apenas um lote
        /// </summary>
        /// <param name="eventoId">Código chave da tabela eventos</param>
        /// <param name="id">Código chave do meu lote</param>
        /// <returns>apenas um lote</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int id);
    }
}