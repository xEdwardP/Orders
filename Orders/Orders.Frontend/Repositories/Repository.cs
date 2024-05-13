using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace Orders.Frontend.Repositories
{
	public class Repository : IRepository
	{
		private readonly HttpClient _httpClient;

		private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		};

		public Repository(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		// Traer informacion
		public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
		{
			var responseHttp = await _httpClient.GetAsync(url);
			if (responseHttp.IsSuccessStatusCode)
			{
				var response = await UnserializeAnswerAsync<T>(responseHttp);
				return new HttpResponseWrapper<T>(response, false, responseHttp);
			}

			return new HttpResponseWrapper<T>(default, true, responseHttp);
		}

		// Guardar sin retorno
		public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
		{
			var messageJSON = JsonSerializer.Serialize(model);
			var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
			var responseHttp = await _httpClient.PostAsync(url, messageContet);
			return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
		}

		//Guardar con retorno
		public async Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
		{
			var messageJSON = JsonSerializer.Serialize(model);
			var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
			var responseHttp = await _httpClient.PostAsync(url, messageContet);
			if (responseHttp.IsSuccessStatusCode)
			{
				var response = await UnserializeAnswerAsync<TActionResponse>(responseHttp);
				return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
			}

			return new HttpResponseWrapper<TActionResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
		}

		// Borrar
		public async Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url)
		{
			var responseHttp = await _httpClient.DeleteAsync(url);
			return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
		}

		// Actualizar sin retorno
		public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
		{
			var messageJSON = JsonSerializer.Serialize(model);
			var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
			var responseHttp = await _httpClient.PutAsync(url, messageContet);
			return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
		}

		//Actualizar con retorno
		public async Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model)
		{
			var messageJSON = JsonSerializer.Serialize(model);
			var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
			var responseHttp = await _httpClient.PutAsync(url, messageContet);
			if (responseHttp.IsSuccessStatusCode)
			{
				var response = await UnserializeAnswerAsync<TActionResponse>(responseHttp);
				return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
			}

			return new HttpResponseWrapper<TActionResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
		}

		// Deserializa el contenido de una respuesta HTTP en un objeto de tipo T.
		private async Task<T> UnserializeAnswerAsync<T>(HttpResponseMessage responseHttp)
		{
			var response = await responseHttp.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<T>(response, _jsonDefaultOptions)!;
		}

        public async Task<HttpResponseWrapper<object>> GetAsync(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
			return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }
    }
}