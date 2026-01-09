namespace Asistencia.Shared.DTOs.Requests;

public class CreateRolUsuarioRequest
{
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
}

public class DeleteRolUsuarioRequest
{
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
}
