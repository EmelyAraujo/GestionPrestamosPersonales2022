using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Pagos
{
    [Key]
    public int PagoId { get; set; }

    [Required(ErrorMessage = "Favor de Ingresar La Fecha.")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Favor Selecccionar un ID")]
    public int PersonaId { get; set; }

    [Required(ErrorMessage = "Favor de Ingresar EL concepto")]
    public string? Concepto { get; set; }

    [Range(2500, 200000000)]
    [Required(ErrorMessage = "Favor de ingresar el monto")]
    public float? Monto { get; set; }

    [ForeignKey("PagoId")]
    public List<PagosDetalle> Detalle { get; set; } = new List<PagosDetalle>();

}