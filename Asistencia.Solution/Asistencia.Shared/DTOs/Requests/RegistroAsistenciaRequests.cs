namespace Asistencia.Shared.DTOs.Requests;

public class CreateRegistroAsistenciaRequest
{
    public int IdEmpleado { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime? HoraEntrada { get; set; }
    public DateTime? HoraSalida { get; set; }
    public int MinutosTarde { get; set; }
    public decimal HorasTrabajadas { get; set; }
    public string EstadoAsistencia { get; set; } = string.Empty;
}

public class UpdateRegistroAsistenciaRequest
{
    public int IdRegistro { get; set; }
    public int IdEmpleado { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime? HoraEntrada { get; set; }
    public DateTime? HoraSalida { get; set; }
    public int MinutosTarde { get; set; }
    public decimal HorasTrabajadas { get; set; }
    public string EstadoAsistencia { get; set; } = string.Empty;
}
