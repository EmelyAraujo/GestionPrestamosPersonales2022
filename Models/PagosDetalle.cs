using System.ComponentModel.DataAnnotations;

public class PagosDetalle{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Favor de ingresar pago Id")]
    public int PagoId { get; set; }

    [Required(ErrorMessage = "Favor de Ingresar EL PrestamoId")]
    public int PrestamoId { get; set; }

    [Range(minimum: 2000, maximum: 2000000, ErrorMessage = "El salario no esta dentro del rango requerido (entre 2,000 y 2,000,000)")]
    public float? ValorPagado { get; set; }


}