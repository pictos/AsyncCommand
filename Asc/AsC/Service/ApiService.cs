using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsC.Service
{
    class ApiService
    {
        static Lazy<ApiService> LazyApi = new Lazy<ApiService>(()=> new ApiService());

        public static ApiService Current => LazyApi.Value;

        readonly HttpClient _client;

        ApiService()
        {
            //https://apps.widenet.com.br/busca-cep/api-de-consulta
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://apps.widenet.com.br/busca-cep/api/cep/")
            };
        }

        public async Task<string> ObterCepAsync()
        {
            var url = "33400-000.json";
            try
            {
                var response = await _client.GetAsync(url).ConfigureAwait(false);
                var content  = await response.Content.ReadAsStringAsync();

                return content;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
