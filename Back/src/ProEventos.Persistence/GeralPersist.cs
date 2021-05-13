using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    
    public class GeralPersist : IGeralPersist
    {
        private readonly ProEventosContext _context;
        
        public GeralPersist(ProEventosContext context)
        {
            this._context = context;

        }
        public void Add<T>(T entity) where T : class
        {
           _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArry) where T : class
        {
            _context.RemoveRange(entityArry);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return(await _context.SaveChangesAsync()) > 0;
        }
        
    }
}