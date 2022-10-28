using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class Personas
{
    [Key]
    public int PersonaId { get; set; }

    [Required(ErrorMessage = "Favor de Ingresar el nombre.")]
    public string? Nombre { get; set; }

   
    [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
    [Phone(ErrorMessage = "Favor de ingresar correctamente el numero Telefonico.")]
    public string? Telefono { get; set; }

    [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
    [Phone(ErrorMessage = "Favor de ingresar correctamente el numero celular.")]
    public string? Celular { get; set; }

    [Remote(action: "VerifyEmail", controller: "Users")]
    [EmailAddress(ErrorMessage ="Ingrese correctamente el Email.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Favor de Ingresar la direccion.")]
    public string? Direccion { get; set; }

    [Required(ErrorMessage = "Favor de Ingresar su fecha de nacimiento.")]
    public DateTime? FechaNacimiento { get; set; } = DateTime.Now;

    public int OcupacionId { get; set; }
    
    [Range(2500,2000000000)]
    public float? Balance {get; set;}

}