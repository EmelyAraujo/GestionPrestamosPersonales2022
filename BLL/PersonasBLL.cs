using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
public class PersonasBLL
{
    private Contexto _contexto;

    public PersonasBLL(Contexto contexto)
    {
        this._contexto = contexto;
    }

    public async Task<bool> Guardar(Personas persona)
    {
        var existe = await Existe(persona.PersonaId);
        if (!existe)
        {
            return await Insertar(persona);
        }
        else
        {
            return await Modificar(persona);
        }
    }

    public async Task<bool> Existe(int personaId)
    {

        return await _contexto.Personas.AnyAsync(p => p.PersonaId == personaId);
    }

    public async Task<bool> Insertar(Personas persona)
    {
        _contexto.Personas.Add(persona);
        int cantidad = await _contexto.SaveChangesAsync();
        return cantidad > 0;
    }

    public async Task<bool> Modificar(Personas persona)
    {
        _contexto.Entry(persona).State = EntityState.Modified;
        return await _contexto.SaveChangesAsync() > 0;
    }
    public async Task<bool> Eliminar(Personas persona)
    {
        _contexto.Entry(persona).State = EntityState.Deleted;
        return await _contexto.SaveChangesAsync() > 0;
    }

    public async Task<Personas?> Buscar(int personaId)
    {
        return await _contexto.Personas
                .Where(o => o.PersonaId == personaId)
                .AsNoTracking()
                .SingleOrDefaultAsync();

    }

    public async Task<List<Personas>> GetList(Expression<Func<Personas, bool>> Criterio)
    {
        return await _contexto.Personas
            .AsNoTracking()
            .Where(Criterio)
            .ToListAsync();
    }
}