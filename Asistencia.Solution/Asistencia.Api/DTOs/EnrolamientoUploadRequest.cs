using System.ComponentModel.DataAnnotations;

namespace Asistencia.Api.DTOs;

public class EnrolamientoUploadRequest
{
    [Required]
    public int IdEmpleado { get; set; }
    
    [Required]
    public IFormFile Imagen { get; set; } = default!;
    
    [Required]
    public string DescriptorFacial { get; set; } = string.Empty;
}
