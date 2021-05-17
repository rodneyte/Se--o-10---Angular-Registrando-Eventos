using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralpersist;
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;
        public EventoService(IGeralPersist geralpersist, 
                             IEventoPersist eventoPersist,
                             IMapper mapper)
        {
            this._geralpersist = geralpersist;
            this._eventoPersist = eventoPersist;
            this._mapper = mapper;

        }
        public async Task<EventoDto> AddEvento(EventoDto model)
        {
             try
             {
                 var evento = _mapper.Map<Evento>(model);

                 _geralpersist.Add<Evento>(evento);

                 if(await _geralpersist.SaveChangesAsync())
                 {
                     var EventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id);
                     
                     return _mapper.Map<EventoDto>(EventoRetorno);
                 }
                 return null;
             }
             catch (Exception ex)
             {
                
                 throw new Exception(ex.Message);
             }
            
        }
        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            
             try
             {
                 var evento = await _eventoPersist.GetEventoByIdAsync(eventoId,false);
                 if(evento==null)return null;

                 model.Id = evento.Id;

                _mapper.Map(model,evento);
                
                 _geralpersist.Update<Evento>(evento);
                 if(await _geralpersist.SaveChangesAsync())
                 {
                    
                    var EventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id);
                     
                     return _mapper.Map<EventoDto>(EventoRetorno);

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

        public async Task<EventoDto[]> GetAllEventoAsync(bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(incluirPalestrantes);
                if(eventos==null)return null;

               var resultado = _mapper.Map<EventoDto[]>(eventos);
                
                return resultado;  
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool incluirPalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema,incluirPalestrantes);
                if(eventos==null)return null;

               var resultado = _mapper.Map<EventoDto[]>(eventos);
                
                return resultado; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool incluirPalestrantes = false)
        {
             try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId,incluirPalestrantes);
                if(evento==null)return null;

                var resultado = _mapper.Map<EventoDto>(evento);

                return resultado; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
    }
}