namespace Asistencia.Web.Interfaces;

public interface IApiService<T> where T : class
{
    Task<List<T>> GetAllAsync(string endpoint);
    Task<T> GetByIdAsync(string endpoint, object id);
    Task<T> PostAsync(string endpoint, object request);
    Task PutAsync(string endpoint, object id, object request);
    Task DeleteAsync(string endpoint, object id);
}
