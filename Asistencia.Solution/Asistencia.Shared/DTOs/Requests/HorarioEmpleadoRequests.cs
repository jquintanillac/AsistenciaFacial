namespace Asistencia.Shared.DTOs.Requests;

public class CreateHorarioEmpleadoRequest
{
    public int IdEmpleado { get; set; }
    public int IdHorario { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}

public class UpdateHorarioEmpleadoRequest
{
    public int IdHorarioEmpleado { get; set; }
    public int IdEmpleado { get; set; }
    public int IdHorario { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
