using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
public class PagosBLL
{
    private Contexto _contexto;

    public PagosBLL(Contexto contexto)
    {
        this._contexto = contexto;
    }

    public async Task<bool> Guardar(Pagos pago)
    {
        var existe = await Existe(pago.PagoId);
        if (!existe)
        {
            return await Insertar(pago);
        }
        else
        {
            return await Modificar(pago);
        }
    }

    public async Task<bool> Existe(int pagoId)
    {
        return await _contexto.Pagos.AnyAsync(p => p.PagoId == pagoId);
    }

    public async Task<bool> Insertar(Pagos pago)
    {
        await _contexto.Pagos.AddAsync(pago);
        foreach (var item in pago.Detalle)
        {
            var prestamo = await _contexto.Prestamos.FindAsync(item.PrestamoId);
            prestamo!.Monto -= item.ValorPagado;

        }

        var persona = await _contexto.Personas.FindAsync(pago.PersonaId);
        persona!.Balance -= pago.Monto;
        var cantidad = await _contexto.SaveChangesAsync() > 0;
        _contexto.Entry(pago).State = EntityState.Detached;
        return cantidad;
    }
    
    public async Task<bool> Modificar(Pagos pago)
    {
        var pagoAnterior = await _contexto.Pagos
        .Where(x => x.PagoId == pago.PagoId)
        .AsNoTracking()
        .SingleOrDefaultAsync();

        var personaAnterior = await _contexto.Prestamos.FindAsync(pagoAnterior!.PersonaId);
        personaAnterior!.Balance += pagoAnterior.Monto;

        foreach (var ante in pagoAnterior.Detalle)
        {
            var prestamos = await _contexto.Prestamos.FindAsync(ante.PrestamoId);
            prestamos!.Balance += ante.ValorPagado;
        }
        _contexto.Database.ExecuteSqlRaw($"DELETE FROM pagosDetalle WHERE PagoId={pago.PagoId};");
        foreach (var actu in pago.Detalle)
        {
            _contexto.Entry(actu).State = EntityState.Added;
            var prestamo = await _contexto.Prestamos.FindAsync(actu!.PrestamoId);
            prestamo!.Balance -= actu.ValorPagado;
        }
        _contexto.Entry(pago).State = EntityState.Modified;
        var guardar = await _contexto.SaveChangesAsync() > 0;
        _contexto.Entry(pago).State = EntityState.Detached;
        return guardar;
    }
    public async Task<bool> Eliminar(Pagos pago)
    {
        var persona = await _contexto.Personas.FindAsync(pago.PersonaId);
        persona!.Balance += pago.Monto;
        
        foreach (var item in pago.Detalle)
        {
            var prestamos = await _contexto.Prestamos.FindAsync(item.PrestamoId);
            prestamos!.Balance +=item.ValorPagado;
        }
        _contexto.Entry(pago).State = EntityState.Deleted;
        var guardar = await _contexto.SaveChangesAsync() > 0;
        _contexto.Entry(pago).State = EntityState.Detached;
        return guardar;
    }

    public async Task<Pagos?> Buscar(int pagoId)
    {
        return await _contexto.Pagos
                .Where(p => p.PagoId == pagoId)
                .Include(x => x.Detalle)
                .AsNoTracking()
                .SingleOrDefaultAsync();

    }

    public async Task<List<Pagos>> GetList(Expression<Func<Pagos, bool>> Criterio)
    {
        return await _contexto.Pagos
            .AsNoTracking()
            .Where(Criterio)
            .ToListAsync();
    }
}