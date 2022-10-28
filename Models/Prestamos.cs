using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class Prestamos{
    [Key]
    public int PrestamoId { get; set; }

    [Required(ErrorMessage = "Favor de Ingresar La Fecha de inicio.")]
    public DateTime Fecha {get; set;} = DateTime.Now;

    [Required(ErrorMessage = "Favor de Ingresar la fecha de vencimiento")]
    public DateTime Vence {get; set;}

    [Range(10000,200000000)]
    [Required(ErrorMessage = "Favor de ingresar el monto")]
    public float? Monto { get; set; }

    [Required(ErrorMessage = "Favor Selecccionar un ID")]
    public int PersonaId { get; set; }

    [Required(ErrorMessage = "Favor de Ingresar EL concepto")]
    public string? Concepto {get; set;}

    [Required(ErrorMessage = "Favor de Ingresaar el balance")]
    public float? Balance {get; set;}
}