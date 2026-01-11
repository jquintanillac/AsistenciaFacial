namespace Asistencia.Shared.Models;

public class Enrolamiento
{
    public int IdEnrolamiento { get; set; }
    public int IdEmpleado { get; set; }
    public string? Tipo { get; set; }
    public byte[] IdentificadorBiometrico { get; set; } = Array.Empty<byte>();
    public string? RutaImagen { get; set; }
    public string? DescriptorFacial { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public bool Estado { get; set; }
}
