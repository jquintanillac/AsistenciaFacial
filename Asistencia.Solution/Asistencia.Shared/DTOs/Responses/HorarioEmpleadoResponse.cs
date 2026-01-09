namespace Asistencia.Shared.DTOs.Responses;

public class HorarioEmpleadoResponse
{
    public int IdHorarioEmpleado { get; set; }
    public int IdEmpleado { get; set; }
    public int IdHorario { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
