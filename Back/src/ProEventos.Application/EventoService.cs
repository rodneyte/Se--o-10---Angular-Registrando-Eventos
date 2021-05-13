using System;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralpersist;
        private readonly IEventoPersist _eventoPersist;
        public EventoService(IGeralPersist geralpersist, IEventoPersist eventoPersist)
        {
            this._eventoPersist = eventoPersist;
            this._geralpersist = geralpersist;

        }
        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                _geralpersist.Add<Evento>(model);
                if(await _geralpersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetEventoByIdAsync(model.Id);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId,false);
                if(evento==null)return null;
                model.Id = evento.Id;

                _geralpersist.Update(model);
                if(await _geralpersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetEventoByIdAsync(model.Id,false);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteEvento(int eventoId)
        {
             var evento = await _eventoPersist.GetEventoByIdAsync(eventoId,false);
                if(evento==null) throw new Exception("Evento para delete não foi encontrado");
                
                _geralpersist.Delete<Evento>(evento);

                return await _geralpersist.SaveChangesAsync();
                
        }

        public async Task<Evento[]> GetAllEventoAsync(bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(incluirPalestrantes);
                if(eventos==null)return null;

                return eventos; 
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema,incluirPalestrantes);
                if(eventos==null)return null;

                return eventos; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool incluirPalestrantes = false)
        {
             try
            {
                var eventos = await _eventoPersist.GetEventoByIdAsync(eventoId,incluirPalestrantes);
                if(eventos==null)return null;

                return eventos; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
    }
}