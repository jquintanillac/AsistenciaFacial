namespace Asistencia.Shared.DTOs.Responses;

public class EnrolamientoResponse
{
    public int IdEnrolamiento { get; set; }
    public int IdEmpleado { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public byte[] IdentificadorBiometrico { get; set; } = Array.Empty<byte>();
    public string? RutaImagen { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool Estado { get; set; }
}
