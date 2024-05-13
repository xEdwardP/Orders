namespace Orders.Frontend.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url); // Get -> Mostrar informacion
															  
		Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model); // Post -> Guardar sin retorno
																			 
		Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model); // Post -> Guardar con retorno
																									   
		Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url); // Delete -> Borrar
																	  
		Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model); // Put -> Actualizar sin retorno

		Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model); // Put -> Actualizar con retorno

        Task<HttpResponseWrapper<object>> GetAsync(string url);
	}
}
