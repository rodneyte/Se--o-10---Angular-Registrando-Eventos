using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext _context;
        public PalestrantePersist(ProEventosContext context)
        {
            this._context = context;

        }
            public async Task<Palestrante[]> GetAllPalestrantesByAsync(bool incluirEventos =false)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p=>p.RedesSociais);

            if(incluirEventos){
                query = query.Include(p=>p.PalestrantesEventos)
                .ThenInclude(pe=>pe.Evento);
            }

            query = query.OrderBy(p=>p.Id);
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool incluirEventos)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p=>p.RedesSociais);

            if(incluirEventos){
                query = query.Include(p=>p.PalestrantesEventos)
                .ThenInclude(pe=>pe.Evento);
            }

            query = query.OrderBy(p=>p.User.PrimeiroNome).
                          Where(p=>p.User.PrimeiroNome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }
 
        public async Task<Palestrante> GetAllPalestranteByIdAsync(int palestranteId, bool incluirEventos)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p=>p.RedesSociais);

            if(incluirEventos){
                query = query.Include(p=>p.PalestrantesEventos)
                .ThenInclude(pe=>pe.Evento);
            }

            query = query.OrderBy(p=>p.User.PrimeiroNome).
                          Where(p=>p.Id==palestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}