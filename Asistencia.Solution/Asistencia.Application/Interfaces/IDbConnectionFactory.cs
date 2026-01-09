using System.Data;
using System.Data.Common;

namespace Asistencia.Application.Interfaces;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}
