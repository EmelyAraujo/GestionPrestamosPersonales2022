using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
public class PrestamosBLL
{
    private Contexto _contexto;

    public PrestamosBLL(Contexto contexto)
    {
        this._contexto = contexto;
    }

    public async Task<bool> Guardar(Prestamos prestamo)
    {
        var existe = await Existe(prestamo.PrestamoId);
        if (!existe)
        {
            return await Insertar(prestamo);
        }
        else
        {
            return await Modificar(prestamo);
        }
    }

    public async Task<bool> Existe(int prestamoId)
    {
        return await _contexto.Prestamos.AnyAsync(p => p.PrestamoId == prestamoId);
    }

    public async Task<bool> Insertar(Prestamos prestamo)
    {
        _contexto.Prestamos.Add(prestamo);
        var persona = await _contexto.Personas.FindAsync(prestamo.PersonaId);
        persona!.Balance += prestamo.Monto;
        prestamo.Balance = prestamo.Monto;
        int cantidad = await _contexto.SaveChangesAsync();
        return cantidad > 0;
    }

    public async Task<bool> Modificar(Prestamos prestamo)
    {
        var pAnterior = await _contexto.Prestamos
        .Where(x => x.PrestamoId == prestamo.PrestamoId)
        .AsNoTracking()
        .SingleOrDefaultAsync();

        var personaAnterior = await _contexto.Personas.FindAsync(pAnterior!.PersonaId);
        personaAnterior!.Balance -= pAnterior.Monto;

        _contexto.Entry(prestamo).State = EntityState.Modified;

        var persona = await _contexto.Personas.FindAsync(prestamo.PersonaId);
        persona!.Balance += prestamo.Monto;

        var cantidad = await _contexto.SaveChangesAsync() > 0;
        
        _contexto.Entry(prestamo).State = EntityState.Detached;
        return cantidad;
    }
    public async Task<bool> Eliminar(Prestamos prestamo)
    {

        var persona = await _contexto.Personas.FindAsync(prestamo!.PersonaId);
        persona!.Balance -= prestamo.Monto;
        _contexto.Entry(prestamo).State = EntityState.Deleted;
        return await _contexto.SaveChangesAsync() > 0;
    }

    public async Task<Prestamos?> Buscar(int prestamoId)
    {
        return await _contexto.Prestamos
                .Where(p => p.PrestamoId == prestamoId)
                .AsNoTracking()
                .SingleOrDefaultAsync();

    }

    public async Task<List<Prestamos>> GetList(Expression<Func<Prestamos, bool>> Criterio)
    {
        return await _contexto.Prestamos
            .AsNoTracking()
            .Where(Criterio)
            .ToListAsync();
    }
}