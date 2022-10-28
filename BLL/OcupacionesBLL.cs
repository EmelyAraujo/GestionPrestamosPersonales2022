using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
public class OcupacionesBLL{
        private Contexto _contexto;

        public OcupacionesBLL(Contexto contexto){
            _contexto = contexto;
        }

        public async Task<bool> Existe(int ocupacionId)
        {
            return await _contexto.Ocupaciones.AnyAsync(o => o.OcupacionId == ocupacionId);
        }

        private async Task<bool> Insertar(Ocupaciones ocupacion){
            _contexto.Ocupaciones.Add(ocupacion);
            return await _contexto.SaveChangesAsync()> 0;
        }

        private async Task<bool> Modificar(Ocupaciones ocupacion){
            _contexto.Entry(ocupacion).State = EntityState.Modified;
            return await _contexto.SaveChangesAsync()> 0;
        }

        public async Task<bool> Guardar(Ocupaciones ocupacion){
            var existe = await Existe(ocupacion.OcupacionId);
            if (!existe)
                return  await this.Insertar(ocupacion);
            else
                return await this.Modificar(ocupacion);
        }

        public async Task<bool> Eliminar(Ocupaciones ocupacion){
            _contexto.Entry(ocupacion).State = EntityState.Deleted;
            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task<Ocupaciones?> Buscar(int ocupacionId){
            return await _contexto.Ocupaciones
                    .Where(o=> o.OcupacionId== ocupacionId)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
                    
        }
        public async  Task<List<Ocupaciones>> GetList(Expression<Func<Ocupaciones, bool>> Criterio){
            return await _contexto.Ocupaciones
                .AsNoTracking()
                .Where(Criterio)
                .ToListAsync();
        }

    }