namespace Asistencia.Shared.DTOs;

public class HorarioDto
{
    public int IdHorario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public TimeSpan HoraEntrada { get; set; }
    public TimeSpan HoraSalida { get; set; }
    public int ToleranciaEntrada { get; set; }
    public int ToleranciaSalida { get; set; }
    public int IdEmpresa { get; set; }
}
